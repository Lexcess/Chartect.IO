/**
 * General ideas of the Hebrew charset recognition
 *
 * Four main charsets exist in Hebrew:
 * "ISO-8859-8" - Visual Hebrew
 * "windows-1255" - Logical Hebrew
 * "ISO-8859-8-I" - Logical Hebrew
 * "x-mac-hebrew" - ?? Logical Hebrew ??
 *
 * Both "ISO" charsets use a completely identical set of code points, whereas
 * "windows-1255" and "x-mac-hebrew" are two different proper supersets of
 * these code points. windows-1255 defines additional characters in the range
 * 0x80-0x9F as some misc punctuation marks as well as some Hebrew-specific
 * diacritics and additional 'Yiddish' ligature letters in the range 0xc0-0xd6.
 * x-mac-hebrew defines similar additional code points but with a different
 * mapping.
 *
 * As far as an average Hebrew text with no diacritics is concerned, all four
 * charsets are identical with respect to code points. Meaning that for the
 * main Hebrew alphabet, all four map the same values to all 27 Hebrew letters
 * (including final letters).
 *
 * The dominant difference between these charsets is their directionality.
 * "Visual" directionality means that the text is ordered as if the renderer is
 * not aware of a BIDI rendering algorithm. The renderer sees the text and
 * draws it from left to right. The text itself when ordered naturally is read
 * backwards. A buffer of Visual Hebrew generally looks like so:
 * "[last word of first line spelled backwards] [whole line ordered backwards
 * and spelled backwards] [first word of first line spelled backwards]
 * [end of line] [last word of second line] ... etc' "
 * adding punctuation marks, numbers and English text to visual text is
 * naturally also "visual" and from left to right.
 *
 * "Logical" directionality means the text is ordered "naturally" according to
 * the order it is read. It is the responsibility of the renderer to display
 * the text from right to left. A BIDI algorithm is used to place general
 * punctuation marks, numbers and English text in the text.
 *
 * Texts in x-mac-hebrew are almost impossible to find on the Internet. From
 * what little evidence I could find, it seems that its general directionality
 * is Logical.
 *
 * To sum up all of the above, the Hebrew probing mechanism knows about two
 * charsets:
 * Visual Hebrew - "ISO-8859-8" - backwards text - Words and sentences are
 * backwards while line order is natural. For charset recognition purposes
 * the line order is unimportant (In fact, for this implementation, even
 * word order is unimportant).
 * Logical Hebrew - "windows-1255" - normal, naturally ordered text.
 *
 * "ISO-8859-8-I" is a subset of windows-1255 and doesn't need to be
 * specifically identified.
 * "x-mac-hebrew" is also identified as windows-1255. A text in x-mac-hebrew
 * that contain special punctuation marks or diacritics is displayed with
 * some unconverted characters showing as question marks. This problem might
 * be corrected using another model prober for x-mac-hebrew. Due to the fact
 * that x-mac-hebrew texts are so rare, writing another model prober isn't
 * worth the effort and performance hit.
 *
 * *** The Prober ***
 *
 * The prober is divided between two nsSBCharSetProbers and an nsHebrewProber,
 * all of which are managed, created, fed data, inquired and deleted by the
 * nsSBCSGroupProber. The two nsSBCharSetProbers identify that the text is in
 * fact some kind of Hebrew, Logical or Visual. The final decision about which
 * one is it is made by the nsHebrewProber by combining final-letter scores
 * with the scores of the two nsSBCharSetProbers to produce a final answer.
 *
 * The nsSBCSGroupProber is responsible for stripping the original text of HTML
 * tags, English characters, numbers, low-ASCII punctuation characters, spaces
 * and new lines. It reduces any sequence of such characters to a single space.
 * The buffer fed to each prober in the SBCS group prober is pure text in
 * high-ASCII.
 * The two nsSBCharSetProbers (model probers) share the same language model:
 * Win1255Model.
 * The first nsSBCharSetProber uses the model normally as any other
 * nsSBCharSetProber does, to recognize windows-1255, upon which this model was
 * built. The second nsSBCharSetProber is told to make the pair-of-letter
 * lookup in the language model backwards. This in practice exactly simulates
 * a visual Hebrew model using the windows-1255 logical Hebrew model.
 *
 * The nsHebrewProber is not using any language model. All it does is look for
 * final-letter evidence suggesting the text is either logical Hebrew or visual
 * Hebrew. Disjointed from the model probers, the results of the nsHebrewProber
 * alone are meaningless. nsHebrewProber always returns 0.00 as confidence
 * since it never identifies a charset by itself. Instead, the pointer to the
 * nsHebrewProber is passed to the model probers as a helper "Name Prober".
 * When the Group prober receives a positive identification from any prober,
 * it asks for the name of the charset identified. If the prober queried is a
 * Hebrew model prober, the model prober forwards the call to the
 * nsHebrewProber to make the final decision. In the nsHebrewProber, the
 * decision is made according to the final-letters scores maintained and Both
 * model probers scores. The answer is returned in the form of the name of the
 * charset identified, either "windows-1255" or "ISO-8859-8".
 *
 */
namespace Chartect.IO.Core
{
    using System;

    /// <summary>
    /// This prober doesn't actually recognize a language or a charset.
    /// It is a helper prober for the use of the Hebrew model probers
    /// </summary>
    public class HebrewProber : CharsetProber
    {
        public const string VisualHebrewName = "ISO-8859-8";
        public const string LogicalHebrewName = "windows-1255";

        // windows-1255 / ISO-8859-8 code points of interest
        private const byte FINALKAF = 0xEA;
        private const byte NORMALKAF = 0xEB;
        private const byte FINALMEM = 0xED;
        private const byte NORMALMEM = 0xEE;
        private const byte FINALNUN = 0xEF;
        private const byte NORMALNUN = 0xF0;
        private const byte FINALPE = 0xF3;
        private const byte NORMALPE = 0xF4;
        private const byte FINALTSADI = 0xF5;
        private const byte NORMALTSADI = 0xF6;

        // Minimum Visual vs Logical final letter score difference.
        // If the difference is below this, don't rely solely on the final letter score distance.
        private const int MINFINALCHARDISTANCE = 5;

        // Minimum Visual vs Logical model score difference.
        // If the difference is below this, don't rely at all on the model score distance.
        private const float MINMODELDISTANCE = 0.01f;

        // The two last bytes seen in the previous buffer.
        private byte prev;

        // owned by the group prober.
        private CharsetProber logicalProber;

        // owned by the group prober.
        private CharsetProber visualProber;
        private int finalCharLogicalScore;
        private int finalCharVisualScore;

        // The two last bytes seen in the previous buffer.
        private byte beforePrev;

        public HebrewProber()
        {
            this.Reset();
        }

        protected CharsetProber VisualProber
        {
            get
            {
                return this.visualProber;
            }

            set
            {
                this.visualProber = value;
            }
        }

        protected int FinalCharLogicalScore
        {
            get
            {
                return this.finalCharLogicalScore;
            }

            set
            {
                this.finalCharLogicalScore = value;
            }
        }

        protected int FinalCharVisualScore
        {
            get
            {
                return this.finalCharVisualScore;
            }

            set
            {
                this.finalCharVisualScore = value;
            }
        }

        protected CharsetProber LogicalProber
        {
            get
            {
                return this.logicalProber;
            }

            set
            {
                this.logicalProber = value;
            }
        }

        protected byte Prev
        {
            get
            {
                return this.prev;
            }

            set
            {
                this.prev = value;
            }
        }

        protected byte BeforePrev
        {
            get
            {
                return this.beforePrev;
            }

            set
            {
                this.beforePrev = value;
            }
        }

        public void SetModelProbers(CharsetProber logical, CharsetProber visual)
        {
            this.LogicalProber = logical;
            this.VisualProber = visual;
        }

        // Final letter analysis for logical-visual decision.
        // Look for evidence that the received buffer is either logical Hebrew or
        // visual Hebrew.
        // The following cases are checked:
        // 1) A word longer than 1 letter, ending with a final letter. This is an
        // indication that the text is laid out "naturally" since the final letter
        // really appears at the end. +1 for logical score.
        // 2) A word longer than 1 letter, ending with a Non-Final letter. In normal
        // Hebrew, words ending with Kaf, Mem, Nun, Pe or Tsadi, should not end with
        // the Non-Final form of that letter. Exceptions to this rule are mentioned
        // above in isNonFinal(). This is an indication that the text is laid out
        // backwards. +1 for visual score
        // 3) A word longer than 1 letter, starting with a final letter. Final letters
        // should not appear at the beginning of a word. This is an indication that
        // the text is laid out backwards. +1 for visual score.
        //
        // The visual score and logical score are accumulated throughout the text and
        // are finally checked against each other in GetCharSetName().
        // No checking for final letters in the middle of words is done since that case
        // is not an indication for either Logical or Visual text.
        //
        // The input buffer should not contain any white spaces that are not (' ')
        // or any low-ascii punctuation marks.
        public override ProbingState HandleData(byte[] buf, int offset, int len)
        {
            // Both model probers say it's not them. No reason to continue.
            if (this.GetState() == ProbingState.NotMe)
            {
                return ProbingState.NotMe;
            }

            int max = offset + len;

            for (int i = offset; i < max; i++)
            {
                byte b = buf[i];

                // a word just ended
                if (b == 0x20)
                {
                    // *(curPtr-2) was not a space so prev is not a 1 letter word
                    if (this.BeforePrev != 0x20)
                    {
                        // case (1) [-2:not space][-1:final letter][cur:space]
                        if (IsFinal(this.Prev))
                        {
                            this.FinalCharLogicalScore++;
                        }

                        // case (2) [-2:not space][-1:Non-Final letter][cur:space]
                        else if (IsNonFinal(this.Prev))
                        {
                            this.FinalCharVisualScore++;
                        }
                    }
                }
                else
                {
                    // case (3) [-2:space][-1:final letter][cur:not space]
                    if ((this.BeforePrev == 0x20) && IsFinal(this.Prev) && (b != ' '))
                    {
                        ++this.FinalCharVisualScore;
                    }
                }

                this.BeforePrev = this.Prev;
                this.Prev = b;
            }

            // Forever detecting, till the end or until both model probers
            // return NotMe (handled above).
            return ProbingState.Detecting;
        }

        // Make the decision: is it Logical or Visual?
        public override string GetCharsetName()
        {
            // If the final letter score distance is dominant enough, rely on it.
            int finalsub = this.FinalCharLogicalScore - this.FinalCharVisualScore;
            if (finalsub >= MINFINALCHARDISTANCE)
            {
                return LogicalHebrewName;
            }

            if (finalsub <= -MINFINALCHARDISTANCE)
            {
                return VisualHebrewName;
            }

            // It's not dominant enough, try to rely on the model scores instead.
            float modelsub = this.LogicalProber.GetConfidence() - this.VisualProber.GetConfidence();
            if (modelsub > MINMODELDISTANCE)
            {
                return LogicalHebrewName;
            }

            if (modelsub < -MINMODELDISTANCE)
            {
                return VisualHebrewName;
            }

            // Still no good, back to final letter distance, maybe it'll save the day.
            if (finalsub < 0)
            {
                return VisualHebrewName;
            }

            // (finalsub > 0 - Logical) or (don't know what to do) default to Logical.
            return LogicalHebrewName;
        }

        public override void Reset()
        {
            this.FinalCharLogicalScore = 0;
            this.FinalCharVisualScore = 0;
            this.Prev = 0x20;
            this.BeforePrev = 0x20;
        }

        public override ProbingState GetState()
        {
            // Remain active as long as any of the model probers are active.
            if (this.LogicalProber.GetState() == ProbingState.NotMe &&
                this.VisualProber.GetState() == ProbingState.NotMe)
            {
                return ProbingState.NotMe;
            }

            return ProbingState.Detecting;
        }

        public override void DumpStatus()
        {
            Console.WriteLine(
                "  HEB: {0} - {1} [Logical-Visual score]",
               this.FinalCharLogicalScore,
               this.FinalCharVisualScore);
        }

        public override float GetConfidence()
        {
            return 0.0f;
        }

        protected static bool IsFinal(byte b)
        {
            return b == FINALKAF || b == FINALMEM || b == FINALNUN
                    || b == FINALPE || b == FINALTSADI;
        }

        protected static bool IsNonFinal(byte b)
        {
            // The normal Tsadi is not a good Non-Final letter due to words like
            // 'lechotet' (to chat) containing an apostrophe after the tsadi. This
            // apostrophe is converted to a space in FilterWithoutEnglishLetters causing
            // the Non-Final tsadi to appear at an end of a word even though this is not
            // the case in the original text.
            // The letters Pe and Kaf rarely display a related behavior of not being a
            // good Non-Final letter. Words like 'Pop', 'Winamp' and 'Mubarak' for
            // example legally end with a Non-Final Pe or Kaf. However, the benefit of
            // these letters as Non-Final letters outweighs the damage since these words
            // are quite rare.
            return b == NORMALKAF || b == NORMALMEM || b == NORMALNUN
                    || b == NORMALPE;
        }
    }
}

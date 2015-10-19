namespace Chartect.IO.Core
{
    using System;

    internal class SingleByteCharsetGroupProber : CharsetProber
    {
        private const int PROBERSNUM = 13;
        private CharsetProber[] probers = new CharsetProber[PROBERSNUM];
        private bool[] isActive = new bool[PROBERSNUM];
        private int bestGuess;
        private int activeNum;

        public SingleByteCharsetGroupProber()
        {
            this.probers[0] = new SingleByteCharSetProber(new Win1251CyrillicModel());
            this.probers[1] = new SingleByteCharSetProber(new Koi8rCyrillicModel());
            this.probers[2] = new SingleByteCharSetProber(new Latin5CyrillicModel());
            this.probers[3] = new SingleByteCharSetProber(new MacCyrillicModel());
            this.probers[4] = new SingleByteCharSetProber(new Ibm866CyrillicModel());
            this.probers[5] = new SingleByteCharSetProber(new Ibm855CyrillicModel());
            this.probers[6] = new SingleByteCharSetProber(new Latin7GreekModel());
            this.probers[7] = new SingleByteCharSetProber(new Win1253GreekModel());
            this.probers[8] = new SingleByteCharSetProber(new Latin5BulgarianModel());
            this.probers[9] = new SingleByteCharSetProber(new Win1251BulgarianModel());
            HebrewProber hebprober = new HebrewProber();
            this.probers[10] = hebprober;

            // Logical
            this.probers[11] = new SingleByteCharSetProber(new Win1255HebrewModel(), false, hebprober);

            // Visual
            this.probers[12] = new SingleByteCharSetProber(new Win1255HebrewModel(), true, hebprober);
            hebprober.SetModelProbers(this.probers[11], this.probers[12]);

            // disable latin2 before latin1 is available, otherwise all latin1
            // will be detected as latin2 because of their similarity.
            // probers[13] = new SingleByteCharSetProber(new Latin2HungarianModel());
            // probers[14] = new SingleByteCharSetProber(new Win1250HungarianModel());
            this.Reset();
        }

        public override ProbingState HandleData(byte[] buf, int offset, int len)
        {
            ProbingState st = ProbingState.NegativeDetection;

            // apply filter to original buffer, and we got new buffer back
            // depend on what script it is, we will feed them the new buffer
            // we got after applying proper filter
            // this is done without any consideration to KeepEnglishLetters
            // of each prober since as of now, there are no probers here which
            // recognize languages with English characters.
            byte[] newBuf = buf.FilterWithoutEnglishLetters(offset, len);
            if (newBuf.Length == 0)
            {
                return this.State; // Nothing to see here, move on.
            }

            for (int i = 0; i < PROBERSNUM; i++)
            {
                if (!this.isActive[i])
                {
                    continue;
                }

                st = this.probers[i].HandleData(newBuf, 0, newBuf.Length);

                if (st == ProbingState.Detected)
                {
                    this.bestGuess = i;
                    this.State = ProbingState.Detected;
                    break;
                }
                else if (st == ProbingState.NegativeDetection)
                {
                    this.isActive[i] = false;
                    this.activeNum--;
                    if (this.activeNum <= 0)
                    {
                        this.State = ProbingState.NegativeDetection;
                        break;
                    }
                }
            }

            return this.State;
        }

        public override float GetConfidence()
        {
            float bestConf = 0.0f, cf;
            switch (this.State)
            {
            case ProbingState.Detected:
                return 0.99f; // sure yes
            case ProbingState.NegativeDetection:
                return 0.01f;  // sure no
            default:
                for (int i = 0; i < PROBERSNUM; i++)
                {
                    if (!this.isActive[i])
                        {
                            continue;
                        }

                        cf = this.probers[i].GetConfidence();
                    if (bestConf < cf)
                    {
                        bestConf = cf;
                        this.bestGuess = i;
                    }
                }

                break;
            }

            return bestConf;
        }

        public override void DumpStatus()
        {
            float cf = this.GetConfidence();
            System.Diagnostics.Debug.WriteLine(" SBCS Group Prober --------begin status");
            for (int i = 0; i < PROBERSNUM; i++)
            {
                if (!this.isActive[i])
                {
                    System.Diagnostics.Debug.WriteLine(
                        " inactive: [{0}] (i.e. confidence is too low).", this.probers[i].GetCharsetName());
                }
                else
                {
                    this.probers[i].DumpStatus();
                }
            }

            System.Diagnostics.Debug.WriteLine(
                " SBCS Group found best match [{0}] confidence {1}.", this.probers[this.bestGuess].GetCharsetName(), cf);
        }

        public override void Reset()
        {
            this.activeNum = 0;
            for (int i = 0; i < PROBERSNUM; i++)
            {
                if (this.probers[i] != null)
                {
                    this.probers[i].Reset();
                    this.isActive[i] = true;
                    this.activeNum++;
                }
                else
                {
                    this.isActive[i] = false;
                }
            }

            this.bestGuess = -1;
            this.State = ProbingState.Detecting;
        }

        public override string GetCharsetName()
        {
            // if we have no answer yet
            if (this.bestGuess == -1)
            {
                this.GetConfidence();

                // no charset seems positive
                if (this.bestGuess == -1)
                {
                    this.bestGuess = 0;
                }
            }

            return this.probers[this.bestGuess].GetCharsetName();
        }
    }
}

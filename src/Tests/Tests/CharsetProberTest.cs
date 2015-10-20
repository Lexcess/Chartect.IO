namespace Chartect.IO.Tests
{
    using System;
    using Chartect.IO.Core;
    using NUnit.Framework;

    [TestFixture]
    public class CharsetProberTest
    {
        [Test]
        public void TestFilterWithEnglishLetter()
        {
            // '¿', 'h', '!', '!', 'e', 'l', 'o', '!', '!'
            byte[] input = { 0xBF, 0x68, 0x21, 0x21, 0x65, 0x6C, 0x6F, 0x21, 0x21 };
            byte[] expected = { 191, 104, 32, 101, 108, 111, 32 };
            var actual = input.FilterWithEnglishLetters(0, input.Length);
            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public void TestFilterWithoutEnglishLetter()
        {
            // 'î', '!', 'l', '!', 'î', 'l', 'l'
            byte[] input = { 0xEE, 0x21, 0x6C, 0x21, 0xEE, 0x6C, 0x6C };
            byte[] expected = { 238, 32, 238, 108, 108 };
            var actual = input.FilterWithoutEnglishLetters(0, input.Length);
            Assert.That(actual, Is.EquivalentTo(expected));
        }
    }
}
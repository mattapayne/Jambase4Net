using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using Jambase4Net;

namespace Jambase4Net.Tests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void ItShouldRemoveAllTabs()
        {
            string s = "This is a \t test \t string";
            Assert.AreEqual("This is a  test  string", s.EscapeWhiteSpace());
        }

        [Test]
        public void ItShouldRemoveAllNewlines()
        {
            string s = "This is a \n test \n string";
            Assert.AreEqual("This is a  test  string", s.EscapeWhiteSpace());
        }
    }
}

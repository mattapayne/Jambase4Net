using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;

namespace Jambase4Net.Tests.ImplementationTests
{
    [TestFixture]
    public class DefaultWebConnectionTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ItShouldThrowAnExceptionIfTheUrlPassedInIsNull()
        {
            IWebConnection cnn = new DefaultWebConnection();
            cnn.MakeRequest(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ItShouldThrowAnExceptionIfTheUrlPassedInIsEmpty()
        {
            IWebConnection cnn = new DefaultWebConnection();
            cnn.MakeRequest(String.Empty);
        }
    }
}

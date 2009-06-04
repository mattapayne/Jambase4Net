using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;

namespace Jambase4Net.Tests.ImplementationTests
{
    [TestFixture]
    public class DefaultConfiguratorTests
    {
        private MockRepository mocks;

        [SetUp]
        public void Setup()
        {
            mocks = new MockRepository();
        }

        [TearDown]
        public void Teardown()
        {
            mocks = null;
        }

        [Test]
        public void ItShouldGetTheAPIKeyFromTheConfigFile()
        {
            IAPI api = mocks.StrictMock<IAPI>();
            api.APIKey = "A key";
            IConfigurator conf = new DefaultConfigurator();
            mocks.ReplayAll();
            conf.Configure(api);
            mocks.VerifyAll();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ItShouldThrowAnExceptionIfTheAPIObjectIsNull()
        {
            IConfigurator conf = new DefaultConfigurator();
            conf.Configure(null);
        }
    }
}

using System;
using System.Collections.Generic;
using Jambase4Net;
using NUnit.Framework;
using Rhino.Mocks;

namespace Jambase4Net.Tests.ImplementationTests
{
    [TestFixture]
    public class APITests
    {
        private class MockConfigurator : IConfigurator
        {

            #region IConfigurator Members

            void IConfigurator.Configure(IAPI api)
            {
                api.APIKey = "Test";
            }

            #endregion
        }

        private class MockBuilder : IEventBuilder
        {

            #region IEventBuilder Members

            IList<IEvent> IEventBuilder.Build(string xml)
            {
                return new List<IEvent>();
            }

            #endregion
        }

        private class MockConnection : IWebConnection
        {

            #region IWebConnection Members

            string IWebConnection.MakeRequest(string url)
            {
                return "Test";
            }

            #endregion
        }

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
            API.Configure(conf => 
            { 
                conf.APIKey = null;
                conf.Builder = null;
                conf.Connection = null;
            });
        }

        [Test]
        public void ItShouldBeConfiguredWithADelegate()
        {
            API.Configure(conf => {
                conf.APIKey = "A key";
            });

            Assert.AreEqual("A key", API.Instance.APIKey);
        }

        [Test]
        public void ItShouldConfigureItselfIfConfigureIsNotCalled()
        {
            Assert.AreEqual("A key", API.Instance.APIKey);
        }

        [Test]
        public void ItShouldHaveADefaultConfiguration()
        {
            Assert.AreEqual("A key", API.Instance.APIKey);
        }

        [Test]
        public void ItShouldBeConfiguredByAnyClassImplementingIConfigurator()
        {
            API.Configure(new MockConfigurator());
            Assert.AreEqual("Test", API.Instance.APIKey);
        }

        [Test]
        public void ItShouldUseTheDefaultWebConnectionIfNotOtherwiseSpecified()
        {
            Assert.IsInstanceOf(typeof(DefaultWebConnection), API.Instance.Connection);
        }

        [Test]
        public void ItShouldUseTheDefaultBuilderIfNotOtherwiseSpecified()
        {
            Assert.IsInstanceOf(typeof(DefaultEventBuilder), API.Instance.Builder);
        }

        [Test]
        public void ItShouldUseACustomBuilderIfSpecified()
        {
            API.Configure(conf => {
                conf.Builder = new MockBuilder();
                conf.APIKey = "test";
            });
            Assert.IsInstanceOf(typeof(MockBuilder), API.Instance.Builder);
        }

        [Test]
        public void ItShouldACustomWebConnectionIfSpecified()
        {
            API.Configure(conf => {
                conf.Connection = new MockConnection();
                conf.APIKey = "test";
            });
            Assert.IsInstanceOf(typeof(MockConnection), API.Instance.Connection);
        }

        [Test]
        public void ItShouldProperlyConstructTheQueryString()
        {
            IWebConnection conn = mocks.StrictMock<IWebConnection>();
            conn.Expect(c => c.MakeRequest("http://api.jambase.com/search?band=The+Dead&user=matt&zip=90210&radius=10&apikey=test")).Return(null);
            API.Configure(conf =>
            {
                conf.Connection = conn;
                conf.APIKey = "test";
            });
            mocks.ReplayAll();
            API.Instance.
                Search().
                ByBand("The Dead").
                ByRadius(10).
                ByUser("matt").
                ByZipCode("90210").List();
            mocks.VerifyAll();
        }

        [Test]
        public void ItShouldMakeTheWebRequestIfSearchArgsAreProvided()
        {
            IWebConnection conn = mocks.StrictMock<IWebConnection>();
            Expect.Call(conn.MakeRequest("http://api.jambase.com/search?band=The+Dead&apikey=test")).Return(null);
            API.Configure(conf =>
            {
                conf.Connection = conn;
                conf.APIKey = "test";
            });
            mocks.ReplayAll();
            API.Instance.
                Search().
                ByBand("The Dead").List();
            mocks.VerifyAll();
        }

        [Test]
        public void ItShouldReturnAnEmptyListIfNoSearchArgsSpecified()
        {
            API.Configure(conf =>
            {
                conf.Connection = new MockConnection();
                conf.APIKey = "test";
            });
            IList<IEvent> results = API.Instance.Search().List();
            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void ItShouldNotMakeTheWebCallIfNoSearchArgsSpecified()
        {
            IWebConnection conn = mocks.StrictMock<IWebConnection>();
            API.Configure(conf =>
            {
                conf.Connection = conn;
                conf.APIKey = "test";
            });
            mocks.ReplayAll();
            IList<IEvent> events = API.Instance.Search().List();
            conn.AssertWasNotCalled(c => c.MakeRequest(null));
        }

        [Test]
        public void ItShouldUseTheBuilderToCreateTheEventsResultSet()
        {
            IEventBuilder bldr = mocks.StrictMock<IEventBuilder>();
            IWebConnection conn = mocks.Stub<IWebConnection>();
            conn.Stub(c => c.MakeRequest("test")).IgnoreArguments().Return("Xml of events");
            bldr.Expect(b => b.Build("Xml of events")).Return(new List<IEvent>());
            API.Configure(conf =>
            {
                conf.APIKey = "test";
                conf.Builder = bldr;
                conf.Connection = conn;
            });
            mocks.ReplayAll();
            IList<IEvent> events = API.Instance.Search().ByZipCode("90210").List();
            mocks.VerifyAll();
        }
    }
}

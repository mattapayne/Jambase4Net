using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;

namespace Jambase4Net.Tests.ImplementationTests
{
    [TestFixture]  
    public class SearchArgsTests
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
        [ExpectedException(typeof(ArgumentNullException))]
        public void ItShouldThrowAnExceptionIfPassedANullAPIObject()
        {
            ISearchArgs args = SearchArgs.Create(null);
        }

        [Test]
        public void ItShouldCreateSearchArgsIfPassedAValidAPIObject()
        {
            IAPI api = mocks.Stub<IAPI>();
            ISearchArgs args = null;
            Assert.DoesNotThrow(delegate {
                args = SearchArgs.Create(api);
            });
            Assert.IsNotNull(args);
        }

        [Test]
        public void ItShouldCreateADictionaryWithBandSetIfBandSpecified()
        {
            ISearchArgs args = SearchArgs.Create(mocks.Stub<IAPI>()).ByBand("The Dead");
            Assert.AreEqual("The Dead", args.ToArgs()["band"]);
        }

        [Test]
        public void ItShouldCreateADictionaryWithUserSetIfUserSpecified()
        {
            ISearchArgs args = SearchArgs.Create(mocks.Stub<IAPI>()).ByUser("A User");
            Assert.AreEqual("A User", args.ToArgs()["user"]);
        }

        [Test]
        public void ItShouldCreateADictionaryWithZipSetIfZipCodeSpecified()
        {
            ISearchArgs args = SearchArgs.Create(mocks.Stub<IAPI>()).ByZipCode("90210");
            Assert.AreEqual("90210", args.ToArgs()["zip"]);
        }

        [Test]
        public void ItShouldCreateADictionaryWithRadiusSetIfRadiusAsDoubleSpecified()
        {
            ISearchArgs args = SearchArgs.Create(mocks.Stub<IAPI>()).ByRadius(33.33);
            Assert.AreEqual("33.33", args.ToArgs()["radius"]);
        }

        [Test]
        public void ItShouldCreateADictionaryWithRadiusSetIfRadiusAsIntSpecified()
        {
            ISearchArgs args = SearchArgs.Create(mocks.Stub<IAPI>()).ByRadius(10);
            Assert.AreEqual("10", args.ToArgs()["radius"]);
        }

        [Test]
        public void ItShouldCreateADictionaryWithRadiusSetIfRadiusAsStringSpecified()
        {
            ISearchArgs args = SearchArgs.Create(mocks.Stub<IAPI>()).ByRadius("22");
            Assert.AreEqual("22", args.ToArgs()["radius"]);
        }
    }
}

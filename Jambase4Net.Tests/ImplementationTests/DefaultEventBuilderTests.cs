using System;
using System.Collections.Generic;
using Jambase4Net;
using NUnit.Framework;
using Rhino.Mocks;
using System.IO;
using System.Reflection;

namespace Jambase4Net.Tests.ImplementationTests
{
    [TestFixture]
    public class DefaultEventBuilderTests
    {
        private IEventBuilder bldr;
        private static string eventXml = null;

        [SetUp]
        public void Setup()
        {
            bldr = new DefaultEventBuilder();
        }

        [TearDown]
        public void Teardown()
        {
            bldr = null;
        }

        [Test]
        public void ItShouldReturnAnEmptyListIfTheXmlPassedInIsNull()
        {
            IList<IEvent> events = bldr.Build(null);
            Assert.IsNotNull(events);
            Assert.AreEqual(0, events.Count);
        }

        [Test]
        public void ItShouldReturnAnEmptyListIfTheXmlPassedInIsEmpty()
        {
            IList<IEvent> events = bldr.Build(String.Empty);
            Assert.IsNotNull(events);
            Assert.AreEqual(0, events.Count);
        }

        [Test]
        public void ItShouldReturnAnEmptyListIfTheXmlPassedInContainsNoData()
        {
            IList<IEvent> events = bldr.Build(NoXmlData);
            Assert.IsNotNull(events);
            Assert.AreEqual(0, events.Count);
        }

        [Test]
        public void ItShouldProperlyBuildAListOfEventsFromXml()
        {
            IList<IEvent> events = bldr.Build(LoadTestData());
            Assert.IsNotNull(events);
            Assert.AreEqual(1, events.Count);
            Assert.IsNotNullOrEmpty(events[0].Date);
            Assert.IsNotNullOrEmpty(events[0].ID);
            Assert.IsNotNullOrEmpty(events[0].Url);
            Assert.IsNotNullOrEmpty(events[0].TicketUrl);
        }

        [Test]
        public void ItShouldProperlyBuildAVenueFromXml()
        {
            IList<IEvent> events = bldr.Build(LoadTestData());
            IVenue venue = events[0].Venue;
            Assert.IsNotNull(venue);
            Assert.IsNotNullOrEmpty(venue.ID);
            Assert.IsNotNullOrEmpty(venue.Name);
            Assert.IsNotNullOrEmpty(venue.City);
            Assert.IsNotNullOrEmpty(venue.State);
            Assert.IsNotNullOrEmpty(venue.ZipCode);
        }

        [Test]
        public void ItShouldProperlyBuildAListOfArtistsFromXml()
        {
            IList<IEvent> events = bldr.Build(LoadTestData());
            Assert.IsNotNull(events[0].Artists);
            Assert.Greater(events[0].Artists.Count, 0);
        }

        private String NoXmlData
        {
            get
            {
                return "<JamBase_Data><errorNode>Band not found</errorNode></JamBase_Data>";
            }
        }

        private string LoadTestData()
        {
            //only load the data once
            if (String.IsNullOrEmpty(eventXml))
            {
                string path = Path.GetFullPath(".");
                DirectoryInfo di = new DirectoryInfo(path);
                di = di.Parent.Parent;
                string testDataPath = Path.Combine(di.FullName, "ImplementationTests");
                testDataPath = Path.Combine(testDataPath, "results.xml");
                using (FileStream fs = File.OpenRead(testDataPath))
                {
                    using (StreamReader sw = new StreamReader(fs))
                    {
                        eventXml = sw.ReadToEnd();
                    }
                }
            }
            return eventXml;
        }
    }
}

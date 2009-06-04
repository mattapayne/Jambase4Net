using System;
using System.Collections.Generic;
using System.Xml;
using Jambase4Net;
using System.Text.RegularExpressions;

namespace Jambase4Net
{
    internal class DefaultEventBuilder : IEventBuilder
    {
        private static Regex REGEX = new Regex(@"^.*errorNode.*errorNode.*$");

        #region JambaseObjectData

        private abstract class JambaseObjectData : IJambaseObject
        {
            protected String id;

            protected JambaseObjectData(XmlNode node)
            {
                Build(node);
            }

            protected abstract void Build(XmlNode node);

            #region IJambaseObject Members

            string IJambaseObject.ID
            {
                get { return id; }
            }

            #endregion

            protected String GetValueForElement(XmlNode node)
            {
                if (node == null)
                {
                    return null;
                }

                return node.InnerText;
            }
        }

        #endregion

        #region ArtistData

        private class ArtistData : JambaseObjectData, IArtist
        {
            private String name;
            private const String ID = "artist_id";
            private const String NAME = "artist_name";

            internal ArtistData(XmlNode node)
                : base(node)
            {

            }

            #region IArtist Members

            string IArtist.Name
            {
                get { return name; }
            }

            #endregion

            protected override void Build(XmlNode node)
            {
                this.id = GetValueForElement(node.SelectSingleNode(ID));
                this.name = GetValueForElement(node.SelectSingleNode(NAME));
            }
        }

        #endregion

        #region VenueDate

        private class VenueData : JambaseObjectData, IVenue
        {
            private const String NAME = "venue_name";
            private const String CITY = "venue_city";
            private const String STATE = "venue_state";
            private const String ZIP = "venue_zip";
            private const String ID = "venue_id";

            private String name;
            private String city;
            private String state;
            private String zip;

            internal VenueData(XmlNode node)
                : base(node)
            {

            }

            protected override void Build(XmlNode node)
            {
                this.id = GetValueForElement(node.SelectSingleNode(ID));
                this.name = GetValueForElement(node.SelectSingleNode(NAME));
                this.city = GetValueForElement(node.SelectSingleNode(CITY));
                this.state = GetValueForElement(node.SelectSingleNode(STATE));
                this.zip = GetValueForElement(node.SelectSingleNode(ZIP));
            }

            #region IVenue Members

            string IVenue.Name
            {
                get { return name; }
            }

            string IVenue.City
            {
                get { return city; }
            }

            string IVenue.State
            {
                get { return state; }
            }

            string IVenue.ZipCode
            {
                get { return zip; }
            }

            #endregion
        }

        #endregion

        #region EventData

        private class EventData : JambaseObjectData, IEvent
        {
            private const String ID = "event_id";
            private const String URL = "event_url";
            private const String TICKET_URL = "ticket_url";
            private const String DATE = "event_date";
            private const String VENUE = "venue";
            private const String ARTISTS = "artists";
            private const String ARTIST = "artist";

            private String url;
            private String ticketUrl;
            private String date;
            private IList<IArtist> artists;
            private IVenue venue;

            internal EventData(XmlNode node)
                : base(node)
            {
                
            }

            protected override void Build(XmlNode node)
            {
                artists = new List<IArtist>();

                this.id = GetValueForElement(node.SelectSingleNode(ID));
                this.url = GetValueForElement(node.SelectSingleNode(URL));
                this.ticketUrl = GetValueForElement(node.SelectSingleNode(TICKET_URL));
                this.date = GetValueForElement(node.SelectSingleNode(DATE));

                XmlNodeList artistNodes = node.SelectNodes(ARTISTS);

                foreach (XmlNode artistNode in artistNodes)
                {
                    artists.Add(new ArtistData(artistNode));
                }

                XmlNode venueNode = node.SelectSingleNode(VENUE);
                this.venue = new VenueData(venueNode);
            }

            #region IEvent Members

            string IEvent.Date
            {
                get { return date; }
            }

            string IEvent.Url
            {
                get { return url; }
            }

            string IEvent.TicketUrl
            {
                get { return ticketUrl; }
            }

            IList<IArtist> IEvent.Artists
            {
                get { return artists; }
            }

            IVenue IEvent.Venue
            {
                get
                {
                    return venue;
                }
            }

            #endregion
        }

        #endregion

        private const String EVENT = "event";

        internal DefaultEventBuilder()
        {

        }

        #region IEventBuilder Members

        IList<IEvent> IEventBuilder.Build(string xml)
        {
            IList<IEvent> events = new List<IEvent>();

            if (String.IsNullOrEmpty(xml))
            {
                return events;
            }

            xml = xml.EscapeWhiteSpace();

            //get out quick if no data
            if (REGEX.IsMatch(xml))
            {
                return events;
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNodeList eventNodes = doc.SelectNodes(String.Format("//{0}", EVENT));

            if (eventNodes != null)
            {
                foreach (XmlNode eventNode in eventNodes)
                {
                    IEvent e = new EventData(eventNode);
                    events.Add(e);
                }
            }

            return events;
        }

        #endregion
    }
}

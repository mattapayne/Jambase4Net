using System;
using System.Collections.Generic;

namespace Jambase4Net
{
    public interface IEvent : IJambaseObject
    {
        String Date { get; }
        String Url { get; }
        String TicketUrl { get; }
        IList<IArtist> Artists { get; }
        IVenue Venue { get; }
    }
}

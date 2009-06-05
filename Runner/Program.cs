using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jambase4Net;

namespace Runner
{
    class Program
    {

        static void Main(string[] args)
        {
            API.Configure(api => api.APIKey = "Your Jambase API key here");
            IList<IEvent> events = API.Instance.Search().ByBand("The Dead").List();
            
            foreach (var evt in events)
            {
                Console.WriteLine(evt.Venue.Name);
            }
        }
    }
}

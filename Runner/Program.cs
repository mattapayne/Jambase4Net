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
            API.Configure(api => api.APIKey = "fsz2pg4zuzmpj9c2xd9335dt");
            IList<IEvent> events = API.Instance.Search().ByBand("The Dead").List();
            
            foreach (var evt in events)
            {
                Console.WriteLine(evt.Venue.Name);
            }
        }
    }
}

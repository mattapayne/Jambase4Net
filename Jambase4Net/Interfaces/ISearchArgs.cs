using System;
using System.Collections.Generic;

namespace Jambase4Net
{
    public interface ISearchArgs
    {
        IDictionary<String, String> ToArgs();
        bool IsEmpty { get; }
        IList<IEvent> List();
        ISearchArgs ByBand(String band);
        ISearchArgs ByZipCode(String zipCode);
        ISearchArgs ByRadius(String radius);
        ISearchArgs ByRadius(double radius);
        ISearchArgs ByUser(String userName);
    }
}

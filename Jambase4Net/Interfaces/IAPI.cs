using System;
using System.Collections.Generic;

namespace Jambase4Net
{
    public interface IAPI
    {
        ISearchArgs Search();
        IList<IEvent> DoSearch(ISearchArgs args);
        string APIKey {get; set;}
        IWebConnection Connection { get; set; }
        IEventBuilder Builder { get; set; }
    }
}

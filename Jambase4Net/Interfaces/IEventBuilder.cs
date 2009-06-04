using System;
using System.Collections.Generic;

namespace Jambase4Net
{
    public interface IEventBuilder
    {
        IList<IEvent> Build(String xml);
    }
}

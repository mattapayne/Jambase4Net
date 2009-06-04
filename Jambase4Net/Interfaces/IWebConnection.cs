using System;

namespace Jambase4Net
{
    public interface IWebConnection
    {
        string MakeRequest(string url);
    }
}

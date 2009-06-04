using System;
using Jambase4Net;
using System.Net;

namespace Jambase4Net
{
    class DefaultWebConnection : IWebConnection
    {
        #region IWebConnection Members

        string IWebConnection.MakeRequest(string url)
        {
            if (String.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            WebClient wc = new WebClient();
            return wc.DownloadString(url);
        }

        #endregion
    }
}

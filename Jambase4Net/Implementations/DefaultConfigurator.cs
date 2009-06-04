using System;
using System.Collections.Generic;
using Jambase4Net;
using System.Configuration;

namespace Jambase4Net
{
    /// <summary>
    /// Reads the API key from app.config/web.config and sets it
    /// Uses the DefaultWebConnection and DefaultEventBuilder to
    /// retrieve and populate results.
    /// </summary>
    internal class DefaultConfigurator : IConfigurator
    {
        private const String API_KEY = "Jambase4Net_API_Key";

        internal DefaultConfigurator()
        {

        }

        #region IConfigurator Members

        void IConfigurator.Configure(IAPI api)
        {
            if (api == null)
            {
                throw new ArgumentNullException("api");
            }
            api.APIKey = this.APIKey;
        }

        #endregion

        private String APIKey
        {
            get
            {
                return ConfigurationManager.AppSettings[API_KEY];
            }
        }
    }
}

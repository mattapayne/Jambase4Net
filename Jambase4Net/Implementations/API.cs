using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Jambase4Net
{
    public delegate void APIConfigurationHandler(IAPI api);

    public class API : IAPI
    {
        private const String API_URL = "http://api.jambase.com/search";
        private static IAPI instance = new API();

        private IWebConnection connection = null;
        private String apiKey = null;
        private IEventBuilder builder = null;

        private API()
        {
            
        }

        public static IAPI Instance
        {
            get 
            {
                EnsureConfigured();
                return instance;
            }
        }

        public static void Configure()
        {
            Configure(new DefaultConfigurator());
        }

        public static void Configure(APIConfigurationHandler handler)
        {
            handler(instance);
        }

        public static void Configure(IConfigurator configurator)
        {
            configurator.Configure(instance);
        }

        #region IAPI Members

        ISearchArgs IAPI.Search()
        {
            return SearchArgs.Create(this);
        }

        IList<IEvent> IAPI.DoSearch(ISearchArgs args)
        {
            IList<IEvent> events = new List<IEvent>();

            if (args == null || args.IsEmpty)
            {
                return events;
            }

            String xml = ((IAPI)this).Connection.MakeRequest(
                String.Format("{0}?{1}", API_URL, ConstructQueryString(args.ToArgs()))
                );
            
            if (String.IsNullOrEmpty(xml))
            {
                return events;
            }

            events = ((IAPI)this).Builder.Build(xml);
            return events;
        }

        string IAPI.APIKey
        {
            get
            {
                return apiKey;
            }
            set
            {
                apiKey = value;
            }
        }

        IWebConnection IAPI.Connection
        {
            get
            {
                if (connection == null)
                {
                    connection = new DefaultWebConnection();
                }
                return connection;
            }
            set
            {
                connection = value;
            }
        }

        IEventBuilder IAPI.Builder
        {
            get
            {
                if (builder == null)
                {
                    builder = new DefaultEventBuilder();
                }

                return builder;
            }
            set
            {
                builder = value;
            }
        }

        #endregion

        private static void EnsureConfigured()
        {
            if (String.IsNullOrEmpty(instance.APIKey))
            {
                Configure();
            }
        }

        private String ConstructQueryString(IDictionary<String, String> args)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<String, String> kvp in args)
            {
                sb.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(kvp.Key), 
                    HttpUtility.UrlEncode(kvp.Value));
            }
            sb.AppendFormat("apikey={0}", apiKey);
            return sb.ToString();
        }
    }
}

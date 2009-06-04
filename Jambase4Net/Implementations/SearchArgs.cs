using System;
using System.Collections.Generic;

namespace Jambase4Net
{
    public class SearchArgs : ISearchArgs
    {
        private const String BAND = "band";
        private const String RADIUS = "radius";
        private const String ZIP = "zip";
        private const String USER = "user";

        private String band;
        private String zip;
        private String user;
        private String radius;

        private readonly IAPI api;

        private SearchArgs(IAPI api)
        {
            if (api == null)
            {
                throw new ArgumentNullException("api");
            }
            this.api = api;
        }

        public static ISearchArgs Create(IAPI api)
        {
            return new SearchArgs(api);
        }

        #region ISearchArgs Members

        IDictionary<string, string> ISearchArgs.ToArgs()
        {
            IDictionary<String, String> args = new Dictionary<String, String>();
            if (!String.IsNullOrEmpty(this.band))
            {
                args[BAND] = this.band.Trim();
            }
            if (!String.IsNullOrEmpty(this.user))
            {
                args[USER] = this.user.Trim();
            }
            if (!String.IsNullOrEmpty(this.zip))
            {
                args[ZIP] = this.zip.Trim();
            }
            if (!String.IsNullOrEmpty(this.radius))
            {
                args[RADIUS] = this.radius.Trim();
            }
            return args;
        }

        bool ISearchArgs.IsEmpty
        {
            get { return ((ISearchArgs)this).ToArgs().Count == 0; }
        }

        IList<IEvent> ISearchArgs.List()
        {
            return api.DoSearch(this);
        }

        ISearchArgs ISearchArgs.ByBand(string band)
        {
            this.band = band;
            return this;
        }

        ISearchArgs ISearchArgs.ByZipCode(string zipCode)
        {
            this.zip = zipCode;
            return this;
        }

        ISearchArgs ISearchArgs.ByRadius(string radius)
        {
            this.radius = radius;
            return this;
        }

        ISearchArgs ISearchArgs.ByRadius(double radius)
        {
            return ((ISearchArgs)this).ByRadius(radius.ToString());
        }

        ISearchArgs ISearchArgs.ByUser(string userName)
        {
            this.user = userName;
            return this;
        }

        #endregion
    }
}

using System;

namespace WebClient.Hosting.Core
{
    public class AppContext
    {
        public string ApiUrl { get; }
        public bool HeavyLoad { get; }

        public AppContext(string apiUrl, bool heavyLoad)
        {
            if (string.IsNullOrEmpty(apiUrl))
            {
                throw new ArgumentNullException(nameof(apiUrl));
            }

            ApiUrl = apiUrl;
            HeavyLoad = heavyLoad;
        }
    }
}

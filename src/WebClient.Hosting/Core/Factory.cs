using System.Collections.Concurrent;
using System.Net;
using System.Net.Http;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebClient.Hosting.Core
{
    public static class Factory
    {
        private static readonly object _lockObj = new object();
        public static readonly ConcurrentQueue<int> Ids = new ConcurrentQueue<int>();

        public static HttpClient HttpClient { get; }

        public static AppContext AppContext { get; set; }

        static Factory()
        {
            HttpClient = new HttpClient();
            // Reserved Ids to test-purpose
            for (int i = 1; i < 2000; i++)
            {
                Ids.Enqueue(i);
            }
        }

        public static void Configure(IServiceProvider services)
        {
            var config = services.GetService<IConfigurationRoot>();
            var apiUrl = config.GetValue<string>("AppSettings:ApiUrl");
            var heavyLoad = config.GetValue<bool>("AppSettings:HeavyLoad");
            AppContext = new AppContext(apiUrl, heavyLoad);
        }

        public static int GetId()
        {
            lock (_lockObj)
            {
                var id = 0;
                if (Ids.TryDequeue(out id))
                {
                    return id;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(Ids));
                }
            }
        }
    }
}

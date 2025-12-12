using Gremlin.Net.Driver;
using Microsoft.Extensions.Configuration;


namespace Hackathon.Services
{
    public interface IGremlinWrapper
    {
        public static string Host { get; set; }

        public static string PrimaryKey { get; set; }

        public static string Database { get; set; }

        public static string Container { get; set; }

        public IConfiguration Configuration { get; set; }

        public GremlinClient getGremlinClient();

    }
}

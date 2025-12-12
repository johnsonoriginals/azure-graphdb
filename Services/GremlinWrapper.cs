using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.WebSockets;

namespace Hackathon.Services
{
    public class GremlinWrapper : IGremlinWrapper
    {
        public static string Host { get; set; }

        public static string PrimaryKey { get; set; }

        public static string Database { get; set; }

        public static string Container { get; set; }

        public IConfiguration Configuration { get; set; }

        public GremlinWrapper(string container, string database, string primaryKey, string host)
        {
            Container = container;
            Database = database;
            PrimaryKey = primaryKey;
            Host = host;
        }
        public GremlinClient getGremlinClient()
        {
            GremlinServer gremlinServer = getGremlinServer();

            ConnectionPoolSettings connectionPoolSettings = getConnectionPoolSettings();

            Action<ClientWebSocketOptions> webSocketConfiguration = getWebSocketConfiguration();

            return new GremlinClient(
                gremlinServer,
                new GraphSON2Reader(),
                new GraphSON2Writer(),
                GremlinClient.GraphSON2MimeType,
                connectionPoolSettings,
                webSocketConfiguration);
        }

        private static bool EnableSSL
        {
            get
            {
                if (Environment.GetEnvironmentVariable("EnableSSL") == null)
                {
                    return true;
                }

                if (!bool.TryParse(Environment.GetEnvironmentVariable("EnableSSL"), out bool value))
                {
                    throw new ArgumentException("Invalid env var: EnableSSL is not a boolean");
                }

                return value;
            }
        }

        private static int Port
        {
            get
            {
                if (Environment.GetEnvironmentVariable("Port") == null)
                {
                    return 443;
                }

                if (!int.TryParse(Environment.GetEnvironmentVariable("Port"), out int port))
                {
                    throw new ArgumentException("Invalid env var: Port is not an integer");
                }

                return port;
            }
        }

        private static Action<ClientWebSocketOptions> getWebSocketConfiguration()
        {
            return new Action<ClientWebSocketOptions>(options =>
            {
                options.KeepAliveInterval = TimeSpan.FromSeconds(10);
            });
        }

        private static ConnectionPoolSettings getConnectionPoolSettings()
        {
            return new ConnectionPoolSettings()
            {
                MaxInProcessPerConnection = 10,
                PoolSize = 30,
                ReconnectionAttempts = 3,
                ReconnectionBaseDelay = TimeSpan.FromMilliseconds(500)
            };
        }

        private static GremlinServer getGremlinServer()
        {
            string containerLink = "/dbs/" + Database + "/colls/" + Container;
            Console.WriteLine($"Connecting to: host: {Host}, port: {Port}, container: {containerLink}, ssl: {EnableSSL}");
            var gremlinServer = new GremlinServer(Host, Port, enableSsl: EnableSSL,
                                                    username: containerLink,
                                                    password: PrimaryKey);
            return gremlinServer;
        }

    }
}

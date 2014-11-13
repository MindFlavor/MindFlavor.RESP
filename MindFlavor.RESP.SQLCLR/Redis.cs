using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Server;
using System.Data.SqlTypes;
using MindFlavor.RESP;

namespace MindFlavor.RESP.SQLCLR
{

    public class Redis
    {
        const string APPDOMAIN_CACHE_NAME = "MindFlavor.RESP.SQLCLR.RedisCache:";

        [SqlProcedure]
        public static void Publish(SqlString redisServerAddress, SqlInt32 redisServerPort, SqlString channel, SqlString message)
        {
            using (RedisConnection rc = GetRedisConnection(redisServerAddress.Value, redisServerPort.Value))
            {
                rc.Publish(channel.Value, message.Value);
            }
        }

        private static RedisConnection GetRedisConnection(string address, int port)
        {
            RedisConnection redisConnection = null;

            System.Net.IPAddress ip;
            if (!System.Net.IPAddress.TryParse(address, out ip))
            {
                ip = System.Net.Dns.GetHostEntry(address).AddressList[0];
            }

            System.Net.IPEndPoint ie = new System.Net.IPEndPoint(ip, port);

            redisConnection = new RedisConnection(ie);
            redisConnection.Open();

            return redisConnection;
        }
    }
}

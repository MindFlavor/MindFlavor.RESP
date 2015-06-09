using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindFlavor.RESP;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.Write("Opening connection to redis...");
            RedisConnection redis = null;

            string address = "ubuntu-spare.pelucchi.local";
            int port = 6379;

            System.Net.IPAddress ip;
            if (!System.Net.IPAddress.TryParse(address, out ip))
            {
                ip = System.Net.Dns.GetHostEntry(address).AddressList[0];
            }

            System.Net.IPEndPoint ie = new System.Net.IPEndPoint(ip, port);

            redis = new RedisConnection(ie);
            redis.Open();
            Console.WriteLine("done!");

            redis.Publish("channel", "I was an DBA like you once, then I took an arrow to the knee...");

            redis.SendSingleLineRaw("PING" + RedisConnection.LINE_SEPARATOR);
            Console.WriteLine(redis.ReceiveResponseRaw());

            //redis.SendSingleLineRaw("GET prova" + RedisConnection.LINE_SEPARATOR);
            //RedisObject ret = redis.ReceiveBulk();
            //Console.WriteLine(ret);

            //redis.Set("random", "StringaTestuale");

            string s = "my mult\u1244iline cool \n{}\r\nkey";
            
            redis.Set("multi", s);
            
            redis.Set("number", 1500);

            int iNum = (RedisInt) redis.Get("number");
            string strRet = (RedisString)redis.Get("multi");

            redis.Publish("channel", "test!");

            redis.Publish("channel", 6000);

            //string str;
            //while((str = Console.ReadLine().ToLower()) != "quit")
            //{
            //}

            Console.WriteLine("exiting...");
        }
    }
}

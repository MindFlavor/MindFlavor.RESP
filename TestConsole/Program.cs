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
            RedisConnection redis = new RedisConnection(new System.Net.IPEndPoint(
                System.Net.IPAddress.Parse("10.50.50.1"),
                6379));
            redis.Open();
            Console.WriteLine("done!");

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



            //string str;
            //while((str = Console.ReadLine().ToLower()) != "quit")
            //{
            //}

            Console.WriteLine("exiting...");
        }
    }
}

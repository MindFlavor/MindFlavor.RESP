using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindFlavor.RESP;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest
    {
        #region Configuration: set this before testing!
        public const string REDIS_IP = "10.50.50.1";
        public const int REDIS_PORT = 6379;
        #endregion

        [TestMethod]
        public void Connect()
        {
            using (
            RedisConnection conn = new RedisConnection(new System.Net.IPEndPoint(
                System.Net.IPAddress.Parse(REDIS_IP),
                REDIS_PORT)))
            {
                conn.Open();
            }
        }

        [TestMethod]
        public void SetAndRetreiveString()
        {
            string str = "my mult\u1244iline cool \n{}\r\nstring with lot of stupid typos";

            using (
            RedisConnection conn = new RedisConnection(new System.Net.IPEndPoint(
                System.Net.IPAddress.Parse(REDIS_IP),
                REDIS_PORT)))
            {
                conn.Open();

                conn.Set("testkey", str);

                string strReturn = (RedisString)conn.Get("testkey");

                Assert.AreEqual<string>(str, strReturn);
            }
        }


        [TestMethod]
        public void SetAndRetreiveInteger()
        {
            const int VAL = 20244;

            using (
            RedisConnection conn = new RedisConnection(new System.Net.IPEndPoint(
                System.Net.IPAddress.Parse(REDIS_IP),
                REDIS_PORT)))
            {
                conn.Open();

                conn.Set("testkey", VAL);

                int iReturn = (RedisInt)conn.Get("testkey");

                Assert.AreEqual<int>(VAL, iReturn);
            }
        }

        [TestMethod]
        public void PublishInt()
        {
            using (
               RedisConnection conn = new RedisConnection(new System.Net.IPEndPoint(
                   System.Net.IPAddress.Parse(REDIS_IP),
                   REDIS_PORT)))
            {
                conn.Open();
                conn.Publish("test", 500);
            }
        }

        [TestMethod]
        public void PublishString()
        {
            using (
               RedisConnection conn = new RedisConnection(new System.Net.IPEndPoint(
                   System.Net.IPAddress.Parse(REDIS_IP),
                   REDIS_PORT)))
            {
                conn.Open();
                conn.Publish("test", "Pellerossa");
            }
        }
    }
}

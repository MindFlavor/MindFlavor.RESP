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
    }
}

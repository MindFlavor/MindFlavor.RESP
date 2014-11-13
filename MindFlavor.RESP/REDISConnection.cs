using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace MindFlavor.RESP
{
    public class RedisConnection : IDisposable
    {
        #region Constants
        public const string LINE_SEPARATOR = "\r\n";
        #endregion

        #region Properties
        public System.Net.IPEndPoint IPEndPoint { get; protected set; }
        #endregion

        #region Members
        protected Socket socket;
        private byte[] bBuffer = new byte[32 * 1024];
        #endregion

        public RedisConnection(System.Net.IPEndPoint IPEndPoint)
        {
            this.IPEndPoint = IPEndPoint;
        }

        public void Open()
        {
            socket = new Socket(SocketType.Stream, ProtocolType.IP);
            socket.Connect(IPEndPoint);
        }

        public void Close()
        {
            if (socket != null)
            {
                SendSingleLineRaw("quit" + LINE_SEPARATOR);
                string ret = (RedisReturnCode)DeserializeObject(ReceiveResponseRaw());

                socket.Close();
                socket = null;
            }
        }


        public void SendSingleLineRaw(string str)
        {
            byte[] bToSend = System.Text.Encoding.UTF8.GetBytes(str);
            socket.Send(bToSend);
        }

        public byte[] ReceiveResponseRaw()
        {
            Array.Clear(bBuffer, 0, bBuffer.Length);
            int iReceived = socket.Receive(bBuffer, bBuffer.Length, SocketFlags.None);

            byte[] bRet = new byte[iReceived];
            Array.Copy(bBuffer, bRet, iReceived);
            return bRet;
        }

        public string[] MangleResponseInStrings(string strRaw)
        {
            string[] strResults = strRaw.Split(new string[] { LINE_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
            return strResults;
        }

        public RedisObject DeserializeObject(byte[] bBuffer, int iPos = 0)
        {
            int iOut;
            return DeserializeObject(bBuffer, iPos, out iOut);
        }

        public RedisObject DeserializeObject(byte[] bBuffer, int iPos, out int iNewPos)
        {
#if DEBUG
            string strDebug = System.Text.Encoding.UTF8.GetString(bBuffer);
#endif

            iNewPos = -1;

            if (bBuffer[iPos] == '+')
            {
                #region Return code
                string str = System.Text.Encoding.UTF8.GetString(bBuffer, iPos, bBuffer.Length - iPos);
                iNewPos = iPos + System.Text.Encoding.UTF8.GetBytes(str).Length;
                return new RedisReturnCode() { Value = str.Substring(1) };
                #endregion
            }
            else if (bBuffer[iPos] == '$')
            {
                #region Single entry
                iPos++;

                int iIdxEndLength = iPos;

                while (!((bBuffer[iIdxEndLength - 1] == LINE_SEPARATOR[0]) && (bBuffer[iIdxEndLength] == LINE_SEPARATOR[1])))
                    iIdxEndLength++;

                int iLenInBytes = int.Parse(System.Text.Encoding.UTF8.GetString(bBuffer, iPos, iIdxEndLength - iPos));

                if (iLenInBytes == -1)
                    return null;

                iPos = iIdxEndLength + 1;

                byte[] bOutput = new byte[iLenInBytes];

                string strEntry = System.Text.Encoding.UTF8.GetString(bBuffer, iPos, iLenInBytes);

                iNewPos = iPos + iLenInBytes + LINE_SEPARATOR.Length;

                if (strEntry.StartsWith(":"))
                {
                    return new RedisInt() { Value = int.Parse(strEntry.Substring(1)) };
                }
                else
                    return new RedisString() { Value = strEntry };
                #endregion
            }
            else
            {
                throw new ArgumentException("Expected $ or +, received " + (char)bBuffer[iPos] + " as first char.");
            }
        }

        public RedisObject Get(string key)
        {
            SendSingleLineRaw("GET " + key + LINE_SEPARATOR);

            return DeserializeObject(ReceiveResponseRaw());
        }

        public void Set(string key, RedisString rs)
        {
            Set(key, (RedisObject)rs);
        }
        public void Set(string key, RedisInt rs)
        {
            Set(key, (RedisObject)rs);
        }

        public void Set(string key, RedisObject value)
        {
            List<RedisObject> lTokens = new List<RedisObject>();

            lTokens.Add((RedisObject)"SET");
            lTokens.Add((RedisObject)key);
            lTokens.Add(value);

            string s = SerializeArray(lTokens);
            SendSingleLineRaw(s);

            string strReturn = System.Text.Encoding.UTF8.GetString(ReceiveResponseRaw());
        }

        protected string SerializeArray(IEnumerable<RedisObject> rObjects)
        {
            StringBuilder sb = new StringBuilder();
            int iCount = 0;

            foreach (RedisObject ro in rObjects)
            {
                sb.Append(SerializeObject(ro));
                iCount++;
            }

            string sCount = "*" + iCount + LINE_SEPARATOR;
            string sArray = sb.ToString();

            string sReturn = sCount + sArray;

            return sReturn;
        }


        protected string SerializeObject(RedisString value)
        {
            return SerializeObject((RedisObject)value);
        }
        protected string SerializeObject(RedisInt value)
        {
            return SerializeObject((RedisObject)value);
        }

        protected string SerializeObject(RedisObject value)
        {
            StringBuilder sb = new StringBuilder();
            string strToOutput = null;

            if (value is RedisString)
            {
                strToOutput = value.ToString();
            }
            else if (value is RedisInt)
            {
                strToOutput = ":" + value.ToString();
            }
            else
            {
                throw new Exception("Unsupported type");
            }

            sb.Append("$" + StringLenInBytes(strToOutput) + LINE_SEPARATOR);
            sb.Append(strToOutput.ToString() + LINE_SEPARATOR);

            string s = sb.ToString();
            return s;
        }

        public static int StringLenInBytes(string str)
        {
            return System.Text.Encoding.UTF8.GetBytes(str).Length;
        }

        void IDisposable.Dispose()
        {
            Close();
        }
    }
}

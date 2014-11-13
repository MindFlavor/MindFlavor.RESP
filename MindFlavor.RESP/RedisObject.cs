using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindFlavor.RESP
{
    public abstract class RedisObject
    {
        public object Value { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }

        static public explicit operator string(RedisObject rs)
        {
            return (RedisString)rs.Value;
        }

        static public explicit operator int(RedisObject rs)
        {
            return (RedisInt)rs.Value;
        }

        static public explicit operator RedisObject(string str)
        {
            return new RedisString() { Value = str };
        }

        static public explicit operator RedisObject(int iVal)
        {
            return new RedisInt() { Value = iVal };
        }
    }

}

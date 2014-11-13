using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindFlavor.RESP
{
    public class RedisString : RedisObject
    {
        static public implicit operator string(RedisString rs)
        {
            return (string)rs.Value;
        }

        static public implicit operator RedisString(string str)
        {
            return new RedisString() { Value = str };
        }
    }
}

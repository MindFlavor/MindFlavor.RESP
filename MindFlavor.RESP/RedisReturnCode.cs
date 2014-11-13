using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindFlavor.RESP
{
    class RedisReturnCode : RedisObject
    {
        static public implicit operator string(RedisReturnCode rs)
        {
            return (string)rs.Value;
        }

        static public implicit operator RedisReturnCode(string str)
        {
            return new RedisReturnCode() { Value = str };
        }
    }
}

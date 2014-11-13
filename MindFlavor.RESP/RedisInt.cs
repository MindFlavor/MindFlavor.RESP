using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindFlavor.RESP
{
    public class RedisInt : RedisObject
    {
        static public implicit operator int(RedisInt rs)
        {
            return (int)rs.Value;
        }

        static public implicit operator RedisInt(int str)
        {
            return new RedisInt() { Value = str };
        }
    }
}
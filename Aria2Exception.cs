﻿using System;

namespace Aria2Sharp
{
    public class Aria2Exception : Exception
    {
        public Aria2Exception(object code, object message) : base($"{code}: {message}.")
        {
        }
    }
}
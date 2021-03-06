﻿using System;

namespace Brickweave.Messaging.ServiceBus.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsBasicType(this Type type)
        {
            return type == typeof(short) || type == typeof(short?)
                   || type == typeof(int) || type == typeof(int?)
                   || type == typeof(long) || type == typeof(long?)
                   || type == typeof(ulong) || type == typeof(ulong?)
                   || type == typeof(decimal) || type == typeof(decimal?)
                   || type == typeof(float) || type == typeof(float?)
                   || type == typeof(double) || type == typeof(double?)
                   || type == typeof(char) || type == typeof(char?)
                   || type == typeof(bool) || type == typeof(bool?)
                   || type == typeof(DateTime) || type == typeof(DateTime?)
                   || type == typeof(Guid) || type == typeof(Guid?)
                   || type == typeof(string);
        }
    }
}

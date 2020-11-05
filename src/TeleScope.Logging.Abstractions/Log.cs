using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace TeleScope.Logging.Abstractions
{
    public static class Log
    {
        public static ILoggerFactory Factory { get; private set; } = new NullLoggerFactory();

        public static void Initialize(ILoggerFactory loggerFactory)
        {
            Factory = loggerFactory ?? new NullLoggerFactory();
        }
    }
}

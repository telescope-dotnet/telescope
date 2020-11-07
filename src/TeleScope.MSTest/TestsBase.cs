using System;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeleScope.Logging;

namespace TeleScope.MSTest
{
    public abstract class TestsBase
    {

        // -- fields


        protected const string SKIP_PLC_TESTS = "SkipPlcTests";

        // -- base methods

        public virtual void Arrange()
        {
            if (!LoggingProvider.IsInitialized)
			{
                LoggingProvider.Initialize(LoggerFactory.Create(builder => builder.AddConsole()));
            }
        }

        public virtual void Cleanup()
        {

        }

        /// <summary>
        /// Helper method to throw specific exceptions and returns the properties
        /// </summary>
        /// <param name="context">The test context</param>
        /// <param name="key">The key to get the value of the property</param>
        /// <returns>The value of the property as a string.</returns>
        protected static string GetProperty(TestContext context, string key)
        {
            if (context.Properties[key] == null)
            {
                throw new ArgumentException($"The test property {key} does not exist, does not have a value, or a test setting is not selected");
            }
            else
            {
                return context.Properties[key].ToString();
            }
        }
    }
}
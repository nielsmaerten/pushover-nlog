using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pushover.NLog;
using NLog;
using NLog.Config;

namespace Pushover.NLog.Tests
{
    [TestClass]
    public class PushoverNLogTests
    {
        private readonly Logger logger;
        /// <summary>
        /// Configure PushoverTarget using NLog API
        /// </summary>
        public PushoverNLogTests()
        {
            //Get logging configuration
            var logConfig = LogManager.Configuration ?? new LoggingConfiguration();

            //Initialize PushoverTarget using API keys
            var target = new PushoverTarget()
            {
                AppToken = "YOUR PUSHOVER APP TOKEN. ref: https://pushover.net ",
                UserOrGroupKey = "YOUR PUSHOVER USER KEY. ref: https://pushover.net ",

                //Optional properties
                EmergencyMessageRetryInterval = 30,
                EmergencyMessageExpiration = 3600,
                ApiBase = "https://api.pushover.net/1/messages.json",
                Device = null,
                Layout = "${message}",
                Title = "${level} event occurred",
                Url = "http://example.com",
                UrlTitle = "example.com"
            };

            //Register target with NLog
            logConfig.AddTarget(target);
            logConfig.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, target));

            //Update configuration
            LogManager.Configuration = logConfig;
            LogManager.ReconfigExistingLoggers();

            //Get logger
            logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// Pump out a test message for every log level
        /// </summary>
        [TestMethod]
        public void DebugTest()
        {
            logger.Debug("Debug test");
            logger.Trace("Trace test");
            logger.Info("Info test");
            logger.Warn("Warn test");
            logger.Error("Error test");
            logger.Fatal("Fatal test");
        }
    }
}

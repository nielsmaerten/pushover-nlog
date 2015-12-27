# Pushover.NLog
An NLog target for Pushover.
Uses PushoverDotNet: https://github.com/nielsmaerten/pushover-dotnet

## Quick start
Configure using NLog API, or use nlog.config:  

```cs
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
    logConfig.LoggingRules.Add(new LoggingRule("*", LogLevel.Error, target));

    //Update configuration
    LogManager.Configuration = logConfig;
    LogManager.ReconfigExistingLoggers();
```
Now, logs with ERROR or FATAL levels will be sent to your Pushover account:
```cs
    var logger = LogManager.GetCurrentClassLogger();
    logger.Error("Error test");
    logger.Fatal("Fatal test");
```

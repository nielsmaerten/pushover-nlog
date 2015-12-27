# Pushover.NLog
NLog target for Pushover.net.  
Use Pushover as a pager, and get alerted immediately when your application encounters an error.

## Quick start
### Configure using NLog API:
```cs
    //Get logging configuration
    var logConfig = LogManager.Configuration ?? new LoggingConfiguration();

    //Initialize PushoverTarget using API keys
    var target = new PushoverTarget()
    {
        AppToken = "YOUR PUSHOVER.NET APP TOKEN",
        UserOrGroupKey = "YOUR PUSHOVER.NET USER KEY",

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

### Configure using NLog.config file
````xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog >
  <extensions>
    <add assembly="Pushover.NLog" />
  </extensions>

  <targets>
    <target 
      type="Pushover" Name="Pushover"
      AppToken="YOUR PUSHOVER.NET APP TOKEN"
      UserOrGroupKey = "YOUR PUSHOVER.NET USER KEY"
      EmergencyMessageRetryInterval = "30"
      EmergencyMessageExpiration = "3600"
      ApiBase = "https://api.pushover.net/1/messages.json"
      Layout = "${message}"
      Title = "${level} event occurred"
      Url = "http://example.com"
      UrlTitle = "example.com" />
  </targets>

  <rules>
    <logger name="*" minlevel="Error" writeTo="Pushover" />
  </rules>
</nlog>
````

Now, logs with ERROR or FATAL levels will be sent to your Pushover account:
```cs
    var logger = LogManager.GetCurrentClassLogger();
    logger.Error("Error test");
    logger.Fatal("Fatal test");
```

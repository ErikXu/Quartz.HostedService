# Quartz.HostedService
Use .Net Core Generic Service and Quartz to Implement Background Schedule Tasks.

## How to use?

Create a console application. And install the package:
```
PM> Install-Package Quartz.HostedService
```

These packages may also need:
```
PM> Install-Package Microsoft.Extensions.Hosting
PM> Install-Package Microsoft.Extensions.Configuration.Json
PM> Install-Package Microsoft.Extensions.Logging.Console
PM> Install-Package Microsoft.Extensions.Logging.Debug
```

Edit the quartz configuration in the appsettings.json:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "quartz": {
    "scheduler": {
      "instanceName": "Quartz.HostedService.Sample"
    },
    "threadPool": {
      "type": "Quartz.Simpl.SimpleThreadPool, Quartz",
      "threadPriority": "Normal",
      "threadCount": 10
    },
    "plugin": {
      "jobInitializer": {
        "type": "Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz.Plugins",
        "fileNames": "quartz_jobs.xml"
      }
    }
  }
}
```

Write your job(s):
```csharp
public class TestJob : IJob
{
    private readonly ILogger _logger;

    public TestJob(ILogger<TestJob> logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation(string.Format("[{0:yyyy-MM-dd hh:mm:ss:ffffff}]Test job is running...", DateTime.Now));
        return Task.CompletedTask;
    }
}
```

Prepare the quartz_jobs.xml:
```xml
<?xml version="1.0" encoding="UTF-8"?>

<job-scheduling-data xmlns="http://quartznet.sourceforge.net/JobSchedulingData"
                     xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                     version="2.0">

  <processing-directives>
    <overwrite-existing-data>true</overwrite-existing-data>
  </processing-directives>

  <schedule>
    <job>
      <name>TestJob</name>
      <group>TestGroup</group>
      <description>Test Job</description>
      <job-type>Quartz.HostedService.Sample.TestJob, Quartz.HostedService.Sample</job-type>
      <durable>true</durable>
      <recover>false</recover>
    </job>
    <trigger>
      <simple>
        <name>TestTrigger</name>
        <group>TestGroup</group>
        <description>Test Trigger</description>
        <job-name>TestJob</job-name>
        <job-group>TestGroup</job-group>
        <repeat-count>-1</repeat-count>
        <repeat-interval>2000</repeat-interval>
      </simple>
    </trigger>

    <!--<trigger>
      <cron>
        <name>TestTrigger</name>
        <group>TestGroup</group>
        <description>Test Trigger</description>
        <job-name>TestJob</job-name>
        <job-group>TestGroup</job-group>
        <cron-expression>0/2 * * * * ?</cron-expression>
      </cron>
    </trigger>-->
  </schedule>
</job-scheduling-data>
```

Add generic host, register quartz hosted service and job(s) in the Program.cs:
```csharp
static void Main()
{
    var host = new HostBuilder()
        .ConfigureHostConfiguration(configHost =>
        {
            configHost.SetBasePath(Directory.GetCurrentDirectory());
        })
        .ConfigureAppConfiguration((hostContext, configApp) =>
        {
            configApp.AddJsonFile("appsettings.json", true);
        })
        .ConfigureServices((hostContext, services) =>
        {
            services.AddLogging();
            services.AddQuartzHostedService(hostContext.Configuration);
            services.AddSingleton<TestJob, TestJob>();
        })
        .ConfigureLogging((hostContext, configLogging) =>
        {
            configLogging.AddConsole();
            configLogging.AddDebug();
        })
        .UseConsoleLifetime()
        .Build();

    host.Run();
}
```
For more details about generic host please refer:  
[https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-2.1](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-2.1)

Your can also view or download the sample by  
[https://github.com/ErikXu/Quartz.HostedService/tree/master/src/Quartz.HostedService.Sample](https://github.com/ErikXu/Quartz.HostedService/tree/master/src/Quartz.HostedService.Sample)

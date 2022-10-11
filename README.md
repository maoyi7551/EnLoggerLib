# EnLoggerLib
The logger build on top of log4net. easy the steps of create the logger in asp.net, doen't support asp.netcore.
allows:
1. redirect the log file in case you don't want to use the default file in config file. Project requires generating the different log files under different libs or funcs.
2. log in .netcore and netframework is in the same lib. (for .netcore still need to do extra step as it requires)


# Important: 

Enlogger.config point to using 
"<add key="log4netconfig_dev" value ="log4net_Dev.config"/>"
log4net_Dev.config contain appender
console
rollingfile
email if error



# Explain 
Handles the third party invoke the program and .netcore web app, log isn't abel to correct loading config file, because of the the program path is not the execution path. 
For example window service, Time Schedule and .NetCore, because the execution path (etc. /system32) is different from the program true path.
It is a must to explicitely spec which the log config path is.
for example the config folder is created under "C:\WindowServices\demo\bin\Debug\", in the application appsetting or web.config file add the entry of "ENLogger_config_path"

  <appSettings>
    <add key="ENLogger_config_path" value="C:\WindowServices\demo\bin\Debug\" />
  </appSettings>
/config

# Steps in .netCore web app, Window service and TimeSchedule:

1. copy the config folder from the ENLogger under the project.
2. add "ENLogger_config_path" in the application appsetting.jso 
3.   "AppSettings": {
    "ENLogger_config_path": "C:\\Demo\\Demo"
  }
3. then create ILog using ENLoggerManager with name of logger, and location and name of log file.
static ILog enlog = ENLoggerManager.GetLogger("Demo Web", @".\log\Demo.log");

# Steps in .netCore app and .netframe work 

1. copy the config folder from the ENLogger under the project.
3. then create ILog using ENLoggerManager with name of logger, and location and name of log file.
static ILog enlog = ENLoggerManager.GetLogger("Demo App", @".\log\Demo.log");



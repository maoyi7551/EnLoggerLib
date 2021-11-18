# EnLoggerLib
The logger build on use log4net. easy the steps of create the logger in asp.net and asp.netcore, allow you to redirect the log file in case you don't want to use the file in config file.


To use in web application:
1. copy the config folder from the ENLogger under the project.
2. then declare ILog use ENLoggerManager with name of logger, and location of log file.
static ILog enlog = ENLoggerManager.GetLogger("Demo", @".\log\Demo.log");

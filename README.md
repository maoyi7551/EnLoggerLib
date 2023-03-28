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
## configuration setting
| Tables        | Are           | Cool  |
| ------------- |:-------------:| -----:|
|  "APPLICATIONINSIGHTS_CONNECTION_STRING"                  | string | <appInsight>/Configure/Properties/"Connection String"
|  "ConnectionStrings:EcomDb_ConnectionString"              | string | <azSql>/Setting/Connection strings"
|  "GraphOptions:ClientId"                                  | string | AAD/AppRegistration/<App>/Application ID
|  "GraphOptions:ClientSecret"                              | string | AAD/AppRegistration/<AppName>/Keys
|  "GraphOptions:TenantId"                                  | string | AAD/Properties/Tenant ID
|  "AuthorizationConfig:ResourcesId"                        | string | AAD/<App>/ObjectID
|  "AuthorizationConfig:AdminRoleId"                        | string | "Administrators role defined on the eCommerce API. Different GUID per environment."
|  "AuthorizationConfig:MandatoryScope"                     | string | "eCommerce.FullAccess"
|  "Abacus:PolicyAccess:PolicyStoreOptions:ApiBaseAddress"  | string | "https://<APIM Hostname>/auth-policies"
|  "Abacus:PolicyAccess:PolicyStoreOptions:ApiClientId"     | string | AAD/AppRegistration/<App>/Application ID
|  "Abacus:PolicyAccess:PolicyStoreOptions:ApiClientSecret" | string | AAD/AppRegistration/<AppName>/Keys|
|  "Abacus:PolicyAccess:PolicyStoreOptions:ApiScope"        | string | "<App ID URI>/.default"|
|  "Abacus:PolicyAccess:PolicyStoreOptions:ApiTokenEndpoint"| string | "https://login.microsoftonline.com/<tenantID>/oauth2/v2.0/token"
|  "Abacus:PolicyAccess:PolicyStoreOptions:CacheKeyPrefix"  | string | "Abacus."
|  "Abacus:PolicyAccess:PolicyStoreOptions:CacheLifetime"   | string | "00:05:00"
|  "Abacus:PolicyAccess:PolicyStoreOptions:FallbackBlobStorageConnectionString"| string | StorageAccounts/<Storage>/Security + netWorking/Access Keys/Connection string
|  "Abacus:PolicyAccess:PolicyStoreOptions:FallbackBlobStorageContainerName"| string| "abacus-policies-fallback"
|  "Abacus:PolicyAccess:PolicyStoreOptions:IsCacheEnabled"  | bool   | true/false
|  "Abacus:PolicyAccess:PolicyStoreOptions:PolicyName"      | string | "cpchem.ecom.api"
|  "Abacus:PolicyAccess:PolicyStoreOptions:RefreshInterval" | string | ""
|  "Abacus:PolicyAccess:PolicyStoreOptions:UseFallbackBlobStorage"| bool| true/false
|  "Abacus:PolicyAccess:PolicyEnforcement:DefaultScope"     | string| "cpchem.ecom.api"

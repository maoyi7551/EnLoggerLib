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
| Key        | Type  | Source and Value |Defintion
| -----------|:-----:| :-----:|-----:|
|  "APPLICATIONINSIGHTS_CONNECTION_STRING"|  string | <appInsight>/Configure/Properties/"Connection String"|Connection string to the AppInsight
|  "ConnectionStrings:EcomDb_ConnectionString"| string | <azSql>/Setting/Connection strings"|Connection string to the Az SqlServer
|  "GraphOptions:ClientId": string | AAD/AppRegistration/<App>/Application ID|Application ID of app registration in AAD that represents the eCommerce API
|  "GraphOptions:ClientSecret": string | AAD/AppRegistration/<AppName>/Keys|The Secret key from the Application
|  "GraphOptions:TenantId": string | AAD/Properties/Tenant ID|the ID of tenant
|  "AuthorizationConfig:ResourcesId"| string| AAD/<App>/ObjectID|Object ID of app registration in AAD that represents the eCommerce API
|  "AuthorizationConfig:AdminRoleId"| string | "Administrators role defined on the eCommerce API. Different GUID per environment."|Represent the admin role in Ecommerce
|  "AuthorizationConfig:MandatoryScope": string | "eCommerce.FullAccess"|Authentication Claim scope
|  "Abacus:PolicyAccess:PolicyStoreOptions:ApiBaseAddress": string | "https://<APIM Hostname>/auth-policies"|Dev = api-dev.cpchem.com
Test = api-test.cpchem.com
Prod = api.cpchem.com
|  "Abacus:PolicyAccess:PolicyStoreOptions:ApiClientId": string | AAD/AppRegistration/<App>/Application ID|Application ID of app registration in AAD that represents the eCommerce API
|  "Abacus:PolicyAccess:PolicyStoreOptions:ApiClientSecret": string | AAD/AppRegistration/<AppName>/Keys|The Secret key from the Application
|  "Abacus:PolicyAccess:PolicyStoreOptions:ApiScope": string | "<App ID URI>/.default"|scope is requested by the Abacus package for accessing the Abacus Policy Store API
|  "Abacus:PolicyAccess:PolicyStoreOptions:ApiTokenEndpoint": string | "https://login.microsoftonline.com/<tenantID>/oauth2/v2.0/token"|the URl verify the JWT token
|  "Abacus:PolicyAccess:PolicyStoreOptions:CacheKeyPrefix": string | "Abacus."|Prefix for the cache key
|  "Abacus:PolicyAccess:PolicyStoreOptions:CacheLifetime": string | "00:05:00"|the cache life time. etc. 5 min.
|  "Abacus:PolicyAccess:PolicyStoreOptions:FallbackBlobStorageConnectionString"| string | StorageAccounts/<Storage>/Security + netWorking/Access Keys/Connection string|Blob storage connection string
|  "Abacus:PolicyAccess:PolicyStoreOptions:FallbackBlobStorageContainerName"| string| "abacus-policies-fallback"|Fallback blob Storage contain name
|  "Abacus:PolicyAccess:PolicyStoreOptions:IsCacheEnabled"| boolean | true/false|enable cache flag.
|  "Abacus:PolicyAccess:PolicyStoreOptions:PolicyName"| string | "cpchem.ecom.api"|the name of policy
|  "Abacus:PolicyAccess:PolicyStoreOptions:RefreshInterval"| string | ""|Interval of refresh the policy
|  "Abacus:PolicyAccess:PolicyStoreOptions:UseFallbackBlobStorage"| boolean| true/false|the flag to use fallback blob storage
|  "Abacus:PolicyAccess:PolicyEnforcement:DefaultScope"| string| "cpchem.ecom.api"|default claim scope

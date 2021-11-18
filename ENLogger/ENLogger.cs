using log4net;
using log4net.Appender;
using log4net.Config;
using System;
using System.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration;
// 1) Microsoft.Extensions.Configuration, for ConfigurationBuilder
//2) Microsoft.Extensions.Configuration.Json, for AddJsonFile()
//3) Microsoft.Extensions.Configuration.FileExtensions, for SetBasePath()
namespace ENLogger
{
    public class ENLoggerManager
    {
        static string appenderName = "RollingFile"; // this is fixed name in the config

        //static bool isProd = false;

        //[Conditional("PROD")]
        //private static void IsProd()
        //{
        //    isProd = true;
        //}


        private static string getCorrectPath(ref bool isWebApp, bool isCore = false)
        {
            // if appsetting is used, ignore everything else. use the path instead.
            // the app invoke by task need be told where the app is located. 
            string path = null;
            string webAppPath = null;
            if(isCore)
            {
                var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();                
                path = config.GetSection("AppSettings:ENLogger_config_path").Value;
            }
            else
            {   
                path = ConfigurationManager.AppSettings["ENLogger_config_path"]; 
                // if this is .netcore this has to comment out because web hosting not working in .net core 
                //webAppPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                //if (path != null && path.Length > 0)
                //{
                //    if (webAppPath != null)
                //    {
                //        isWebApp = true;
                //        // if the exec invoke from system32, load from appsetting.
                //        if (webAppPath.Contains("system32"))
                //        {
                //            isWebApp = false;
                //        }
                //        else
                //        {
                //            // if this is web app, prevent the user set to the wrong directory. here the code update the path.
                //            path = webAppPath;
                //        }
                //    }
                //}
                //// remove the "\"
                //if (path != null && path.EndsWith("\\"))
                //    path = path.Remove(path.LastIndexOf("\\"), 1);
                //return path;
            }

            // if appsetting not set, then figure out the current directory from the code.
            // return the web app path. throw exception if it is invoke from system32 (must set from appsetting)

            if (webAppPath!=null)
            {
                Console.WriteLine("webAppPath:" + webAppPath);
                // if the exec invoke from system32, load from appsetting.
                if(webAppPath.Contains("system32"))
                {
                    string mesg = String.Format("isWebApp {0}, exe invoke from system32, you must set [ENLogger_config_path] in appsetting from app.config!!!", isWebApp);
                    throw new Exception(mesg);
                }
                else
                {
                    isWebApp = true;
                     if (webAppPath!=null &&webAppPath.EndsWith("\\"))                
                        webAppPath = webAppPath.Remove(webAppPath.LastIndexOf("\\"), 1);
                    //webAppPath = webAppPath + @"\bin\config";
                    return webAppPath;
                }
            }
            else
            {
                if (path == null)
                    path = Directory.GetCurrentDirectory();
                if (path!=null && path.Contains("system32"))
                {
                    throw new Exception("exe invoke from system32, you must set [path] in appsetting from app.config!!!");
                }

                Console.WriteLine("path:" + path);
                if (path.EndsWith("\\"))
                path = path.Remove(path.LastIndexOf("\\"), 1); 
                return path;
            } 
        }

        public static ILog GetLogger(string logName, string file)
        {
            bool isWebApp = false;
            string path = getCorrectPath(ref isWebApp,true);
            
            string configpath = path+@"\config\";
            if(isWebApp)
            {
                configpath = path + @"\bin\config\";
            }
            string Enlogger_configFile = configpath + @"\Enlogger.config";
            System.Configuration.ConfigurationFileMap fileMap;
            if (File.Exists(Enlogger_configFile))
            {
                fileMap = new ConfigurationFileMap(Enlogger_configFile);
            }
            else
            {
                throw new Exception("can't locate the config file in " + Enlogger_configFile);
                //Enlogger_configFile = path + @"\bin\Enlogger.config";
                //fileMap = new ConfigurationFileMap(Enlogger_configFile); 
            }
            System.Configuration.Configuration configuration = System.Configuration.ConfigurationManager.OpenMappedMachineConfiguration(fileMap);

            //IsProd(); 
            string configFile = configpath+ configuration.AppSettings.Settings["log4netconfig_dev"].Value;
            // if (isProd)
           // {
            //     configFile = configpath+ configuration.AppSettings.Settings["log4netconfig_prod"].Value; 
            //}
            if (file.Length > 0)
            {
                if (!File.Exists(configFile))
                    throw new Exception(string.Format(" {0}  isn't existing, you might create the template from ENLogger/config, and copy the file to this folder {1}", configFile,path));
                XmlConfigurator.Configure(new FileInfo(configFile));
                changeLogFile(file);
            }
            ILog log = (ILog)log4net.LogManager.GetLogger(logName);
            //log.Info(string.Format("log4net init... config: {0}",configFile));
            return log;
        }

        private static void ChangeFileLocation2(string _CustomerName, string _Project)
        {
            XmlConfigurator.Configure();
            log4net.Repository.Hierarchy.Hierarchy h = (log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository();

            foreach (IAppender a in h.Root.Appenders)
            {
                if (a is FileAppender)
                {
                    FileAppender fa = (FileAppender)a;
                    string sNowDate = DateTime.Now.ToLongDateString();
                    // Programmatically set this to the desired location here
                    string FileLocationinWebConfig = fa.File;
                    string logFileLocation = FileLocationinWebConfig + _Project + "\\" + _CustomerName + "\\" + sNowDate + ".log";

                    fa.File = logFileLocation;
                    fa.ActivateOptions();
                    break;
                }
            }
        }

        private static bool changeLogFile(string newFilename)
        {
            var rootRepository = log4net.LogManager.GetRepository();
            foreach (var appender in rootRepository.GetAppenders())
            {
                if (appender.Name.Equals(appenderName) && appender is log4net.Appender.FileAppender)
                {
                    var fileAppender = appender as log4net.Appender.FileAppender;
                    fileAppender.File = newFilename;
                    fileAppender.ActivateOptions();
                    return true;  // Appender found and name changed to NewFilename
                }
            }
            return false; // appender not found 
        }
    }
}

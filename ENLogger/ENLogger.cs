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

       
        private static string getCorrectPath(bool isCoreWebApp = false)
        {
            // if appsetting is used, ignore everything else. use the path instead.
            // the app invoke by task need be told where the app is located. 
            string path = null; 
            if(isCoreWebApp)
            {
                var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();                
                path = config.GetSection("AppSettings:ENLogger_config_path").Value;
            }
            else
            {   
                path = ConfigurationManager.AppSettings["ENLogger_config_path"];
                if(path!=null)
                {
                    path = Directory.GetCurrentDirectory();
                }
            }

            // if appsetting not set, then figure out the current directory from the code.
            // return the web app path. throw exception if it is invoke from system32 (must set from appsetting)
 
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

        public static ILog GetLogger(string logName, string file, bool isCoreWebApp=false)
        { 
            string path = getCorrectPath(isCoreWebApp);
            
            string configpath = path+@"\config\";
             
            string Enlogger_configFile = configpath + @"\Enlogger.config";
            System.Configuration.ConfigurationFileMap fileMap;
            if (File.Exists(Enlogger_configFile))
            {
                fileMap = new ConfigurationFileMap(Enlogger_configFile);
            }
            else
            {
                configpath = path + @"\bin\config\";
                Enlogger_configFile = configpath + @"\Enlogger.config";
                if (File.Exists(Enlogger_configFile))
                {
                    fileMap = new ConfigurationFileMap(Enlogger_configFile);
                }
                else
                    throw new Exception("can't locate the config file in " + Enlogger_configFile); 
            }
             Configuration configuration =  ConfigurationManager.OpenMappedMachineConfiguration(fileMap);
             
            string configFile = configpath+ configuration.AppSettings.Settings["log4netconfig_dev"].Value;
           
            if (file.Length > 0)
            {
                if (!File.Exists(configFile))
                    throw new Exception(string.Format(" {0}  isn't existing, you might create the template from ENLogger/config, and copy the file to this folder {1}", configFile,path));
                XmlConfigurator.Configure(new FileInfo(configFile));
                changeLogFile(file);
            }
            ILog log = (ILog)log4net.LogManager.GetLogger(logName); 
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

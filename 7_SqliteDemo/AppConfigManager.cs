using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Framework
{
    public static class AppConfigManager
    {
        public static void Initialize()
        {
            //设置DataDirectory文件夹
            SetDataDir();
        }


        public static void UpdateConnectionString(string newConnectionStr)
        {
            // Get the configuration file.
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            ConnectionStringsSection csSection = config.ConnectionStrings;
            csSection.ConnectionStrings["Pitman"].ConnectionString = newConnectionStr;

            // Save the configuration file.
            config.AppSettings.SectionInformation.ForceSave = true;
            config.Save(ConfigurationSaveMode.Modified);

            // Force a reload of the changed section.
            ConfigurationManager.RefreshSection("connectionStrings"); 
        }

        public static void RestoreToDefaultConnectionString()
        {
            UpdateConnectionString(@"data source=|DataDirectory|\Pitman.db");
        }

        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["Pitman"].ConnectionString;
        }

        private static void SetDataDir()
        {
            DirectoryInfo baseDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string data_dir = baseDir.FullName;
            if ((baseDir.Name.ToLower() == "debug" || baseDir.Name.ToLower() == "release")
                && (baseDir.Parent.Name.ToLower() == "bin"))
            {
                data_dir = Path.Combine(baseDir.Parent.Parent.FullName, "App_Data");
            }

            AppDomain.CurrentDomain.SetData("DataDirectory", data_dir);
        }

    }
}

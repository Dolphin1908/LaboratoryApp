using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.Support.Helpers
{
    public static class AppConfigHelper
    {
        public static string GetKey(string key)
        {
            string secretPath = "App.secret.config";

            // Nếu có file secret
            if (File.Exists(secretPath))
            {
                var configMap = new ExeConfigurationFileMap { ExeConfigFilename = secretPath };
                var secretConfig = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

                var secretValue = secretConfig.AppSettings.Settings[key]?.Value;
                if (!string.IsNullOrEmpty(secretValue))
                {
                    return secretValue; // Ưu tiên key trong file secret
                }
            }

            // Nếu không có file hoặc không có key trong file, fallback sang App.config
            return ConfigurationManager.AppSettings[key];
        }

    }
}

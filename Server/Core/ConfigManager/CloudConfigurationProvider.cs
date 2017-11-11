using Microsoft.WindowsAzure;

namespace Server.Core.ConfigManager
{
    public class CloudConfigurationProvider : IConfigurationProvider
    {
        public string GetSetting(string settingName)
        {
            return CloudConfigurationManager.GetSetting(settingName);
        }
    }
}

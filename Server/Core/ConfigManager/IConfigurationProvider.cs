
namespace Server.Core.ConfigManager
{
    public interface IConfigurationProvider
    {
        string GetSetting(string settingName);
    }
}

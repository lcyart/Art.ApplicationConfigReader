namespace Art.ConfigurationReader.Storages
{
    public interface IStorageProvider
    {
        void InitializeStorage();
        string GetConfigurationListJSON();
        T GetValue<T>(string key);
    }
}

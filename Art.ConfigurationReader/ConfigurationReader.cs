using Art.ConfigurationReader.Storages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Art.ConfigurationReader
{
    public class ConfigurationReader
    {
        IStorageProvider storage;
        public ConfigurationReader(string applicationName, string connectionString, int refreshTimerIntervalInMs)
        {
            if (connectionString.Contains("mongodb"))
            {
                storage = new Storages.MongoDB.MongoDBStorage(applicationName, connectionString, refreshTimerIntervalInMs);
            }
        }
        public string GetConfigurationListJSON()
        {
            return storage.GetConfigurationListJSON();
        }
        public T GetValue<T>(string key)
        {
            return storage.GetValue<T>(key);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Art.ConfigurationReader.Storages
{
    public interface IStorageProvider
    {
        void InitializeStorage();
        string GetConfigurationListJSON();
        T GetValue<T>(string key);
    }
}

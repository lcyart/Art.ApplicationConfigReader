using Newtonsoft.Json;
using Art.ConfigurationReader.Storages.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

namespace Art.ConfigurationReader.Storages.MongoDB
{
    class MongoDBStorage : IStorageProvider, IDisposable
    {
        private IMongoClient _client;
        private IMongoDatabase _database;
        private IMongoCollection<ApplicationConfig> _configCollection;
        private string _connectionString;
        private string _applicationName;
        private int _refreshTimerIntervalInMs;
        private ConcurrentDictionary<string, ApplicationConfig> _cacheList;

        public MongoDBStorage(string applicationName, string connectionString, int refreshTimerIntervalInMs)
        {
            _connectionString = connectionString;
            _applicationName = applicationName;
            _refreshTimerIntervalInMs = refreshTimerIntervalInMs;

            InitializeStorage();
        }

        public void InitializeStorage()
        {
            try
            {
                var mongoUrl = new MongoUrl(_connectionString);
                _client = new MongoClient(mongoUrl);
                _database = _client.GetDatabase(mongoUrl.DatabaseName);

                _configCollection = _database.GetCollection<ApplicationConfig>("ApplicationConfig");
                if (_configCollection.Find(_ => true).ToList().Count == 0)
                {
                    _database.CreateCollection("ApplicationConfig");
                    _configCollection = _database.GetCollection<ApplicationConfig>("ApplicationConfig");
                    _configCollection.InsertOne(new ApplicationConfig { ID = 1, Name = "SiteName", Type = "String", Value = "x", IsActive = true, ApplicationName = "SERVICE-A" });
                    _configCollection.InsertOne(new ApplicationConfig { ID = 2, Name = "IsBasketEnabled", Type = "Boolean", Value = "true", IsActive = true, ApplicationName = "SERVICE-B" });
                    _configCollection.InsertOne(new ApplicationConfig { ID = 3, Name = "MaxItemCount", Type = "Int", Value = "50", IsActive = true, ApplicationName = "SERVICE-A" });
                }

                StartSync();
            }
            catch (Exception ex)
            {
                throw new Exception("Connection Error! Please Check Connection String!", ex.InnerException);
            }
        }

        private void StartSync()
        {
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMilliseconds(_refreshTimerIntervalInMs);

            var timer = new System.Threading.Timer((e) =>
            {
                SyncActiveItemsAsync();
            }, null, startTimeSpan, periodTimeSpan);
        }

        private void SyncActiveItemsAsync()
        {
            var result = GetConfigValues();
            _cacheList = new ConcurrentDictionary<string, ApplicationConfig>();
            foreach (KeyValuePair<string, ApplicationConfig> configItem in result)
            {
                ApplicationConfig appConfig;
                if (_cacheList.TryGetValue(configItem.Key, out appConfig))
                    _cacheList[configItem.Key] = configItem.Value;
                else
                    _cacheList.TryAdd(configItem.Key, configItem.Value);
            }
        }
        public string GetConfigurationListJSON()
        {
            return JsonConvert.SerializeObject(_cacheList);
        }

        private ConcurrentDictionary<string, ApplicationConfig> GetConfigValues()
        {
            ConcurrentDictionary<string, ApplicationConfig> configValues = new ConcurrentDictionary<string, ApplicationConfig>();
            List<ApplicationConfig> configCollection = _configCollection.Find(config => config.ApplicationName == _applicationName && config.IsActive == true).ToList();

            foreach (ApplicationConfig appConfig in configCollection)
            {
                configValues.TryAdd(appConfig.Name, appConfig);
            }

            return configValues;
        }

        public T GetValue<T>(string key)
        {
            ApplicationConfig appConfig;
            if (_cacheList.TryGetValue(key, out appConfig))
            {
                return (T)Convert.ChangeType(appConfig.Value, typeof(T));
            }

            throw new Exception("Format Error!");
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _cacheList = null;
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
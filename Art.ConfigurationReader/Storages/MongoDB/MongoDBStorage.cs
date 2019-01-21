using Newtonsoft.Json;
using Art.ConfigurationReader.Storages.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using MongoDB.Bson;

namespace Art.ConfigurationReader.Storages.MongoDB
{
    class MongoDBStorage : IStorageProvider
    {
        private IMongoClient _client;
        private IMongoDatabase _database;
        private IMongoCollection<ApplicationConfig> _configCollection;
        private string _connectionString;
        private string _applicationName;
        private int _refreshTimerIntervalInMs;
        private List<ApplicationConfig> cacheList;
        private bool isBusy;

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

                isBusy = false;
                StartSync();
            }
            catch (Exception ex)
            {
                throw new Exception("Bağlantı Kurulamadı! Lütfen Sunucu Adresinizi Kontrol Ediniz!", ex.InnerException);
            }
        }

        private void StartSync()
        {
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMilliseconds(_refreshTimerIntervalInMs);

            var timer = new System.Threading.Timer((e) =>
            {
                SyncActiveItemsAsync().GetAwaiter().GetResult();
            }, null, startTimeSpan, periodTimeSpan);
        }

        private void SyncActiveItems()
        {
            if (!isBusy)
            {
                isBusy = true;
                var result = _configCollection.Find(config => config.ApplicationName == _applicationName && config.IsActive == true).ToList();
                cacheList = new List<ApplicationConfig>();
                foreach (var item in result)
                {
                    ApplicationConfig ac = new ApplicationConfig();
                    ac._id = item._id;
                    ac.ID = item.ID;
                    ac.Name = item.Name;
                    ac.Type = item.Type;
                    ac.Value = Convert.ChangeType(item.Value, Helper.TypeHelper.FindStringType(item.Type));
                    ac.IsActive = item.IsActive;
                    ac.ApplicationName = item.ApplicationName;
                    cacheList.Add(ac);
                }
                isBusy = false;
            }
        }

        private async Task SyncActiveItemsAsync()
        {
            if (!isBusy)
            {
                isBusy = true;
                var result = await _configCollection.Find(config => config.ApplicationName == _applicationName && config.IsActive == true).ToListAsync();
                cacheList = new List<ApplicationConfig>();
                foreach (var item in result)
                {
                    ApplicationConfig ac = new ApplicationConfig();
                    ac._id = item._id;
                    ac.ID = item.ID;
                    ac.Name = item.Name;
                    ac.Type = item.Type;
                    ac.Value = Convert.ChangeType(item.Value, Helper.TypeHelper.FindStringType(item.Type));
                    ac.IsActive = item.IsActive;
                    ac.ApplicationName = item.ApplicationName;
                    cacheList.Add(ac);
                }
                isBusy = false;
            }
        }
        public string GetConfigurationListJSON()
        {
            return JsonConvert.SerializeObject(cacheList);
        }
        public T GetValue<T>(string key)
        {
            try
            {
                object v;
                if (cacheList != null)
                {
                    var q = cacheList.Where(x => x.Name == key);
                    v = q.Count() > 0 ? q.FirstOrDefault().Value : null;

                }
                else
                    v = null;

                return (T)Convert.ChangeType(v, typeof(T));
            }
            catch (Exception ex)
            {
                throw new Exception("Format Çevrilemedi!", ex.InnerException);
            }
        }
    }
}
# Art.ApplicationConfigReader

Kütüphane aşağıdaki kod ile initialize edilir ve storage olarak MongoDB ile çalışır.

**var client = new ConfigurationReader(serviceName, connectionString, refreshInterval);**

- serviceName =Kullanılacak servisin adını tanımlar. Ex: "SERVICE-A"
- connectionString =Storage için verilecek olan bağlantı metinini tanımlar. Uygulama connection string içerisinde veritabanı bilgisinide göndermenizi beklemektedir. Ex: mongodb://localhost:27017/config
- refreshInterval=Sistemin değişiklikleri kaç milisaniyede bir kontrol edeceğini belirler. Ex: 3000
```
var client = new ConfigurationReader("SERVICE-A", "mongodb://localhost:27017/config", 3000);
```

Kütüphane initialize edildiğinde config veritabanı içerisine ApplicationConfig koleksiyonu bulunmuyorsa yaratacaktır ve koleksiyonu otomatik olarak örnek veriler ile dolduracaktır. Bu sebeple test işlemlerini yaparken öncelikle **ConfigurationReaderApp** konsol uygulamasını çalıştırın.

**Methodlar**

- **client.GetConfigurationListJSON()** Methodu Aktif konfigürasyon kayıtlarını JSON string olarak dönmektedir.
```
client.GetConfigurationListJSON();
```
- **client.GetValue< T >(string key)** Methodu T ile belirtilen tipteki key değerini Storage üzerinde arayarak eşleşen değeri dönmektedir.
```
client.GetValue<string>("SiteName");
```
	
**Kontrol Panel**
- **Art.ConfigurationReader.UI** Storage üzerinde Ekleme, Silme, Güncelleme, Listeleme ve Filtreleme işlemlerinin yapılabildiği ASP.NET Core MVC uygulamasıdır. Storage connection stringi parametrik değildir. Home Controller altında aşağıdaki methodun aktif Storage'e göre güncellenmesi gerekmektedir.

```
public IMongoDatabase GetMongoDatabase()
{
	var mongoClient = new MongoClient("mongodb://localhost:27017");
	return mongoClient.GetDatabase("config");
}
```

# Art.ApplicationConfigReader

Kütüphane aşağıdaki kod ile initialize edilir.

**var client = new ConfigurationReader(serviceName, connectionString, refreshInterval);**

- serviceName =Kullanılacak servisin adını tanımlar. Ex: "SERVICE-A"
- connectionString =Storage için verilecek olan bağlantı metinini tanımlar. Ex: mongodb://localhost:27017/config
- refreshInterval=Sistemin değişiklikleri kaç milisaniyede bir kontrol edeceğini belirler. Ex: 3000

Kütüphane initialize edildiğinde config veritabanı içerisine ApplicationConfig koleksiyonu bulunmuyorsa yaratacaktır ve koleksiyonu otomatik olarak örnek veriler ile dolduracaktır. Bu sebeple test işlemlerini yaparken öncelikle **ConfigurationReaderApp** konsol uygulamasını çalıştırın.

**Methodlar**

- **client.GetConfigurationListJSON()** Methodu Aktif konfigürasyon kayıtlarını JSON string olarak dönmektedir.

- **client.GetValue<T>(string key)** Methodu T ile belirtilen tipteki key değerini Storage üzerinde arayarak eşleşen değeri dönmektedir.

- **Art.ConfigurationReader.UI** Storage üzerinde Ekleme, Silme, Güncelleme, Listeleme ve Filtreleme işlemlerinin yapılabildiği ASP.NET Core MVC uygulamasıdır.

# Art.ApplicationConfigReader

Uygulama Kullanım

Kütüphane aşağıdaki kod ile initialize edilir.

var client = new ConfigurationReader(serviceName, connectionString, refreshInterval);
serviceName =Kullanılacak servisin adını tanımlar. Ex: "SERVICE-A"
connectionString =Storage için verilecek olan bağlantı metinini tanımlar. Ex: mongodb://localhost:27017/config
refreshInterval=Sistemin değişiklikleri kaç dakikada bir kontrol edeceğini belirler. Ex: 3000

Kütüphane initialize edildiğinde config veritabanı içerisine ApplicationConfig koleksiyonunu yaratacaktır ve koleksiyonu otomatik dolduracaktır. Bu sebeple test işlemlerini yaparken öncelikle ConfigurationReaderApp konsol uygulamasını çalıştırın.

client.GetConfigurationListJSON() Methodu String tipinde JSON verisi olarak aktif olan konfigürasyon verilerini dönmektedir.

client.GetValue<T>(string key) Methodu T ile belirtilen tipte key alanında gönderilen veriyi Storage üzerinde Name parametresine karşılık gelen değeri dönmektedir.

Art.ConfigurationReader.UI Storage üzerinde Ekleme, Silme, Güncelleme, Listeleme ve Filtreleme işlemlerinin yapılabildiği ASP.NET Core MVC uygulamasıdır.

using Art.ConfigurationReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConfigurationReaderApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new ConfigurationReader("SERVICE-A", "mongodb://localhost:27017/config", 3000);
            Thread.Sleep(5000);
            Console.WriteLine(client.GetConfigurationListJSON());
            Console.WriteLine(client.GetValue<int>("MaxItemCount"));
            Console.WriteLine(client.GetValue<string>("SiteName"));
            Console.ReadKey();
        }
    }
}

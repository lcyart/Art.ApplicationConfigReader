using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Art.ConfigurationReader.UI.Infrastructure.Repository
{
    interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Detail(int recordId);
        void Create(T record);
        DeleteResult Delete(int recordId);
        UpdateResult Update(T record);
    }
}

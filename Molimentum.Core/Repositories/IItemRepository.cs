using System;

namespace Molimentum.Repositories
{
    public interface IItemRepository<T> : IDisposable
    {
        PagedList<T> List(int page, int pageSize);
        PagedList<T> ListPublished(int page, int pageSize);

        T Create();
        T Get(string id);
        void Save(T item);
        void Delete(T item);
        
        void SubmitChanges();
    }
}
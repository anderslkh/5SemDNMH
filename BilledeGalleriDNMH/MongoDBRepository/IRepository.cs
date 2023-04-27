using Models;

namespace MongoDBRepository
{
    public interface IRepository<T> where T : Entity
    {
        public Task<T> Create(T t);

        public Task<T> Read(T t);
        public Task<bool> Delete(Guid id);
    }
}

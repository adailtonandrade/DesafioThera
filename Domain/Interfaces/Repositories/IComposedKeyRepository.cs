namespace Domain.Interfaces.Repositories
{
    public interface IComposedKeyRepository<T> : IGenericRepository<T> where T : class
    {
        T GetByComposedKey(int param1, int param2);
        void Delete(T obj);
    }
}
using System.Collections.Generic;

namespace Domain.Interfaces.Services
{
    public interface IComposedKeyGenericService<T> where T : class
    {
        T GetByComposedKey(int param1, int param2);
        List<string> Delete(T obj);
    }
}

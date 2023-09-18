namespace Application.Interfaces
{
    public interface IComposedKeyGenericAppService<T> where T : class
    {
        T GetByComposedKey(int param1, int param2);
    }
}

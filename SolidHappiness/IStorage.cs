namespace SolidHappiness
{
    public interface IStorage
    {
         void Set(string key, object value);

         object Get(string key);

         bool Exists(string key);
    }
}
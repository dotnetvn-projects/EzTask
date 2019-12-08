namespace EzTask.Interface
{
    public interface ILanguageLocalization
    {
        void SetLocalization(string languageSetting);
        string GetLocalization(string key, object resourceType);
        string GetLocalization(string key, string resourceType);
        bool IsAvailable { get; }
    }
}

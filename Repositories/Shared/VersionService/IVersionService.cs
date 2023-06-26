namespace Repositories.Shared.VersionService
{
    public interface IVersionService
    {
        string GetVersionNumber();
        bool GetIsUpdateForcible();
        string GetLatestApiVersion();
    }
}
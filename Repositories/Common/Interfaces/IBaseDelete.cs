namespace Repositories.Interfaces
{
    public interface IBaseDelete
    {
        Task<bool> Delete(long id);
    }

}

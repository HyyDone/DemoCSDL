using UserDACS.Models;

namespace UserDACS.Repositories
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> ShowAsync();
    }
}

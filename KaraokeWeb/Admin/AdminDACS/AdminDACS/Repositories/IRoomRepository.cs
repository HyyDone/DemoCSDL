using AdminDACS.Models;

namespace AdminDACS.Repositories
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> ShowAsync();
        Task<Room> GetByIdAsync(string roomId);          
        Task AddAsync(Room room);                           
        Task UpdateAsync(Room room);                        
        Task DeleteAsync(string roomId);
    }
}

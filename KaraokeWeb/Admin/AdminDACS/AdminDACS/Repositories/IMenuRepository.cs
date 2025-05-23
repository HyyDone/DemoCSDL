using AdminDACS.Models;

namespace AdminDACS.Repositories
{
    public interface IMenuRepository
    {
        Task<IEnumerable<Menu>> ShowAsync();
        Task<IEnumerable<Menu>> GetAllAsync();
        Task<Menu> GetByIdAsync(string roomId);
        Task AddAsync(Menu menu);
        Task UpdateAsync(Menu menu);
        Task DeleteAsync(string menuId);
    }
}

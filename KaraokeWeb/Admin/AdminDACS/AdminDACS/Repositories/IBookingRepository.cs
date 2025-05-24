using AdminDACS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserDACS.Repositories
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetAllAsync();
        Task<Booking?> GetByIdAsync(string id);
        Task AddAsync(Booking entity);
        Task UpdateAsync(Booking entity);
        Task DeleteAsync(string id);
    }
}

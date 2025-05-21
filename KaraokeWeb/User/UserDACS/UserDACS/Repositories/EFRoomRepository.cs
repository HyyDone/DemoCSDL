using UserDACS.Models;
using Microsoft.EntityFrameworkCore;

namespace UserDACS.Repositories
{
    public class EFRoomRepository : IRoomRepository
    {
        private readonly ApplicationDBContext _context;

        public EFRoomRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Room>> ShowAsync()
        {
            return await _context.Rooms.ToListAsync();
        }
    }
}

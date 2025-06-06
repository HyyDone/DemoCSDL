using AdminDACS.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminDACS.Repositories
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
            return await _context.Rooms
                                 .Include(r => r.LoaiPhong)  
                                 .ToListAsync();
        }

        public async Task<Room> GetByIdAsync(string roomId)
        {
            return await _context.Rooms
                                 .Include(r => r.LoaiPhong) 
                                 .FirstOrDefaultAsync(r => r.MaPhong == roomId);
        }

        public async Task AddAsync(Room room)
        {
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
            }
        }
    }
}

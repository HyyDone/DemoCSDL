using AdminDACS.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserDACS.Repositories
{
    public class EFBookingRepository : IBookingRepository
    {
        private readonly ApplicationDBContext _context;

        public EFBookingRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.bookings
                                 .Include(b => b.Room) // nếu cần load phòng kèm theo
                                 .ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(string id)
        {
            return await _context.bookings
                                 .Include(b => b.Room)
                                 .FirstOrDefaultAsync(b => b.MaBooking == id);
        }

        public async Task AddAsync(Booking entity)
        {
            await _context.bookings.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Booking entity)
        {
            _context.bookings.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var booking = await _context.bookings.FindAsync(id);
            if (booking != null)
            {
                _context.bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
        }
    }
}

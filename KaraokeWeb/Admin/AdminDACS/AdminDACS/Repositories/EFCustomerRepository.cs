using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdminDACS.Models;
using UserDACS.Repositories;

namespace AdminDACS.Repositories
{
    public class EFCustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDBContext _context;

        public EFCustomerRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Customer?> GetByPhoneAsync(string phone)
        {
            return await _context.customers
                .FirstOrDefaultAsync(c => c.Phone == phone);
        }

        public async Task AddAsync(Customer customer)
        {
            await _context.customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Customer customer)
        {
            _context.customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string MaKH)
        {
            var booking = await _context.bookings.FindAsync(MaKH);
            if (booking != null)
            {
                _context.bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
        }
    }
}

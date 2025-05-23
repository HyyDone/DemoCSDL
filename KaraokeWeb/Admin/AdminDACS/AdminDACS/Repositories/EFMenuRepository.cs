using AdminDACS.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminDACS.Repositories
{
    public class EFMenuRepository : IMenuRepository
    {
        private readonly ApplicationDBContext _context;

        public EFMenuRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Menu>> ShowAsync()
        {
            return await _context.menus.ToListAsync();
        }

        public async Task<IEnumerable<Menu>> GetAllAsync()
        {
            return await _context.menus.ToListAsync();
        }

        public async Task<Menu> GetByIdAsync(string menuId)
        {
            return await _context.menus.FindAsync(menuId);
        }

        public async Task AddAsync(Menu menu)
        {
            await _context.menus.AddAsync(menu);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Menu menu)
        {
            _context.menus.Update(menu);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string menuId)
        {
            var menu = await _context.menus.FindAsync(menuId);
            if (menu != null)
            {
                _context.menus.Remove(menu);
                await _context.SaveChangesAsync();
            }
        }
    }
}

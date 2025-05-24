using AdminDACS.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminDACS.Repositories
{
    public class EFWarehouseRepository : IWarehouseRepository
    {
        private readonly ApplicationDBContext _context;

        public EFWarehouseRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Warehouse>> ShowAsync()
        {
            return await _context.Warehouses.ToListAsync();
        }

        public async Task<IEnumerable<Warehouse>> GetAllAsync()
        {
            return await _context.Warehouses.ToListAsync();
        }
    }
}

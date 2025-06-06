using AdminDACS.Models;

namespace AdminDACS.Repositories
{
    public interface IWarehouseRepository
    {
        Task<IEnumerable<Warehouse>> ShowAsync();
        Task<IEnumerable<Warehouse>> GetAllAsync();
    }
}
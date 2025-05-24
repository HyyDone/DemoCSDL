using AdminDACS.Models;

namespace AdminDACS.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByPhoneAsync(string phone);
        Task AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(string MaKH);
    }
}

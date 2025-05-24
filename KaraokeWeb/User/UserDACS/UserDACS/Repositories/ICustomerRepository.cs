using UserDACS.Models;

namespace UserDACS.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByPhoneAsync(string phone);
        Task AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
    }
}

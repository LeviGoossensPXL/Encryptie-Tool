using WebApplication1.Entities;

namespace WebApplication1.Repositories.Interfaces
{
    public interface IExampleRepository
    {
        // just a example repository
        Task<IEnumerable<Example>> GetAll();
        Task<Example?> GetById(int? id);
        Task Add(Example example);
        Task Update(Example example);
        Task Delete(Example example);
    }
}

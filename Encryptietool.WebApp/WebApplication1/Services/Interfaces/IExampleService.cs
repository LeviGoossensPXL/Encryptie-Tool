using WebApplication1.Entities;

namespace WebApplication1.Services.Interfaces
{
    public interface IExampleService
    {
        // just a example service
        Task<Example> DoSomethingWithExample(Example example);
    }
}

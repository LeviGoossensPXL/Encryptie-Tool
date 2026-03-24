namespace WebApplication1.Services.Interfaces;

public interface IFileService
{
    public Task Save(IFormFile formFile);
}
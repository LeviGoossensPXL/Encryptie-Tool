using WebApplication1.Models.Results;

namespace WebApplication1.Services.Interfaces;

public interface IFileService
{
    public Task<FileSaveResult> Save(IFormFile formFile);
}
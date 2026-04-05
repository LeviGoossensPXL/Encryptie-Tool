using WebApplication1.Models.Results;

namespace WebApplication1.Services.Interfaces;

public interface IFileService
{
    public Task<FileSaveResult> Save(IFormFile formFile);
    public FileDeleteResult Delete(FileInfo fileInfo);
    public FileDownloadResult Download(string fileName);
}
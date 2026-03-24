using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _hostingEnvironment;

    public FileService(IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    public async Task Save(IFormFile formFile)
    {
        string uploads = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot\\uploadedFiles");
        if (formFile.Length > 0)
        {
            string filePath = Path.Combine(uploads, formFile.FileName);
            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await formFile.CopyToAsync(fileStream);
        }
    }
}
using WebApplication1.Models.Results;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services;

public class FileService : IFileService
{
    private readonly string _uploadsFolder;

    public FileService(IWebHostEnvironment hostingEnvironment)
    {
        _uploadsFolder = Path.Combine(hostingEnvironment.ContentRootPath, "wwwroot\\uploadedFiles");
    }

    public async Task<FileSaveResult> Save(IFormFile formFile)
    {
        var result = new FileSaveResult();
        if (formFile.Length <= 0)
        {
            result.Failed("filesize is zero");
            return result;
        }
        
        string filePath = Path.Combine(_uploadsFolder, formFile.FileName);
        await using var fileStream = new FileStream(filePath, FileMode.Create);
        await formFile.CopyToAsync(fileStream);
        await fileStream.FlushAsync();
        // fileStream.Close();
        result.FileInfo = new FileInfo(filePath);
        return result;
    }

    public FileDeleteResult Delete(FileInfo fileInfo)
    {
        var result = new FileDeleteResult();
        fileInfo.Delete();
        return result;
    }
}
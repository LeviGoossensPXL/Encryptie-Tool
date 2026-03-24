using WebApplication1.Models.Results;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services;

public class FileService : IFileService
{
    private readonly string _filesFolder;

    public FileService(IWebHostEnvironment hostingEnvironment)
    {
        _filesFolder = Path.Combine(hostingEnvironment.ContentRootPath, "wwwroot", "Files");
        Directory.CreateDirectory(_filesFolder);
    }

    public async Task<FileSaveResult> Save(IFormFile formFile)
    {
        var result = new FileSaveResult();
        if (formFile.Length <= 0)
        {
            result.Failed("filesize is zero");
            return result;
        }
        
        string filePath = Path.Combine(_filesFolder, formFile.FileName);
        await using var fileStream = new FileStream(filePath, FileMode.Create);
        await formFile.CopyToAsync(fileStream);
        await fileStream.FlushAsync();
        result.FileInfo = new FileInfo(filePath);
        return result;
    }

    public FileDeleteResult Delete(FileInfo fileInfo)
    {
        var result = new FileDeleteResult();
        fileInfo.Delete();
        return result;
    }

    public FileDownloadResult Download(string fileName)
    {
        var result = new FileDownloadResult();
        var fileInfo = new FileInfo(Path.Combine(_filesFolder, fileName));
        if (!File.Exists(fileInfo.FullName))
        {
            result.Failed("file doesn't exist");
            return result;
        }
        result.FileInfo = new FileInfo(fileInfo.FullName);
        return result;
    }
}
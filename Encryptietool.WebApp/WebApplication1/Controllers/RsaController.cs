using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers;

public class RsaController : Controller
{
    private readonly IRsaEncryptionService _rsaEncryptionService;

    public RsaController(IRsaEncryptionService rsaEncryptionService)
    {
        _rsaEncryptionService = rsaEncryptionService;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        return View(new RsaViewModel());
    }
    
    [HttpPost]
    public IActionResult Encryption(RsaViewModel rsaViewModel)
    {
        var rsa = RSA.Create();
        rsa.ImportFromPem(rsaViewModel.RsaPublicKey);
        
        var l1 = Convert.FromBase64String(rsaViewModel.InputAesKey);
        var result = _rsaEncryptionService.EncryptKey(l1, rsa);
        rsaViewModel.OutputAesKey = Convert.ToBase64String(result);
        
        return View("Index", rsaViewModel);
    }
    
    [HttpPost]
    public IActionResult Decryption(RsaViewModel rsaViewModel)
    {
        var rsa = RSA.Create();
        rsa.ImportFromPem(rsaViewModel.RsaPrivateKey);
        
        var l1 = Convert.FromBase64String(rsaViewModel.InputAesKey);
        var result = _rsaEncryptionService.DecryptKey(l1, rsa);
        rsaViewModel.OutputAesKey = Convert.ToBase64String(result);
        
        return View("Index", rsaViewModel);
    }

    [HttpGet]
    public IActionResult DownloadOutput(string content)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(content);
        return File(bytes, "application/octet-stream", "output.txt");
    }
}
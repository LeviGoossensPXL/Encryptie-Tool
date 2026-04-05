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
        if (!ModelState.IsValid) return View("Index", rsaViewModel);
        
        var result = _rsaEncryptionService.EncryptKey(rsaViewModel.InputAesKey, rsaViewModel.RsaPublicKey);
        rsaViewModel.OutputAesKey = result.CipherText;
        
        return View("Index", rsaViewModel);
    }
    
    [HttpPost]
    public IActionResult Decryption(RsaViewModel rsaViewModel)
    {
        if (!ModelState.IsValid) return View("Index", rsaViewModel);
        
        var result = _rsaEncryptionService.DecryptKey(rsaViewModel.InputAesKey, rsaViewModel.RsaPrivateKey);
        rsaViewModel.OutputAesKey = result.PlainText;
        
        return View("Index", rsaViewModel);
    }

    [HttpGet]
    public IActionResult DownloadOutput(string content)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(content);
        return File(bytes, "application/octet-stream", "output.txt");
    }
}
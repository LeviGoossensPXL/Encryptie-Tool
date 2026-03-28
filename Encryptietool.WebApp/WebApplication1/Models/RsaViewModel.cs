using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class RsaViewModel
{
    [Required]
    public string InputAesKey { get; set; }
        
    public string? RsaPublicKey { get; set; }
    public string? RsaPrivateKey { get; set; }
    
    public string? OutputAesKey { get; set; }
}
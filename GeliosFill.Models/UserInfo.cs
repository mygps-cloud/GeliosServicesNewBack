using System.ComponentModel.DataAnnotations;

namespace GeliosFill.Models;

public class UserInfo
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
}
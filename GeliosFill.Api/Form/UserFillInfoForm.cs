using System.ComponentModel.DataAnnotations;

namespace GeliosFill.Api.Form;

public class UserFillInfoForm
{
    [Required]
    public string CarName { get; set; }
    
    [Required]
    public string CardId { get; set; }
    
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
}
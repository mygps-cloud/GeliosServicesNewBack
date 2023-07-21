using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GeliosFill.Models;

public class UserFillInfo
{
    [Key]
    public int Id { get; set; }
    
    public string CarName { get; set; }
    
    public string CardId { get; set; }
    
    public int CarId { get; set; }
 
    public string Username { get; set; }

    [JsonIgnore]
    public List<FuelFillHistory> FuelFillHistories { get; set; } = new List<FuelFillHistory>();

}

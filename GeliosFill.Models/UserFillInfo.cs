using System.ComponentModel.DataAnnotations;

namespace GeliosFill.Models;

public class UserFillInfo
{
    [Key]
    public int Id { get; set; }
    
    public string CarName { get; set; }
    
    public string CardId { get; set; }
    
    public int CarId { get; set; }
 
    public string Username { get; set; }

    public IList<FuelFillHistory> FuelFillHistories { get; set; } = new List<FuelFillHistory>();

}

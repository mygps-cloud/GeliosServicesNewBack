using System.ComponentModel.DataAnnotations;

namespace GeliosFill.Models;

public class FuelFillHistory
{
    [Key]
    public int Id { get; set; }

    public double Distance { get; set; }

    public DateTime DateOfFill { get; set; }

    public int UserFillInfoId { get; set; }
    public UserFillInfo UserFillInfo { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeliosFill.Models;

public class FuelFillHistory
{
    [Key]
    public int Id { get; set; }

    public double Distance { get; set; }

    public DateTime DateOfFill { get; set; }

    public string? FillPlace { get; set; }
    [Column(TypeName = "decimal(10,8)")]
    public decimal Latt { get; set; }
    [Column(TypeName = "decimal(10,8)")]
    public decimal Long { get; set; }
    public decimal Liters { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }

    public int UserFillInfoId { get; set; }
    public UserFillInfo UserFillInfo { get; set; }
}
namespace GeliosFill.Models;

public class Fill
{
    public string AGSID { get; set; }
    public string CardID { get; set; }
    public decimal? Liters { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? Sum { get; set; }
    public DateTime FillDateTime { get; set; }
}
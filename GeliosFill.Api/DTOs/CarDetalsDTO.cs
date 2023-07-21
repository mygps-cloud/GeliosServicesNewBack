namespace GeliosFill.Api.DTOs;

public record CarDetalsDTO(double Distance,DateTime DateOfFill,string FillPlace,decimal Latt,decimal Long,decimal Liters,decimal UnitPrice,decimal TotalPrice);
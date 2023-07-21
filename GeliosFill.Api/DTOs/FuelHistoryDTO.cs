using GeliosFill.Models;

namespace GeliosFill.Api.DTOs;

public record FuelHistoryDTO(string carname,string CardId,List<CarDetalsDTO> CarDetalsDtos);
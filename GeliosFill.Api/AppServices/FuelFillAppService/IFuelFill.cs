using GeliosFill.Api.DTOs;
using GeliosFill.Models;

namespace GeliosFill.Api.AppServices.FuelFillAppService;

public interface IFuelFill
{
    Task CalculateFuelFillHistory(UserInfo userInfo);
    List<FuelHistoryDTO> GetFuelHistory(UserInfo userInfo);
}
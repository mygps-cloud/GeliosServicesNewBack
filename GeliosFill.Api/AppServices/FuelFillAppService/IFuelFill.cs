using GeliosFill.Models;

namespace GeliosFill.Api.AppServices.FuelFillAppService;

public interface IFuelFill
{
    Task CalculateFuelFillHistory(UserInfo userInfo);
    object GetFuelHistory(UserInfo userInfo);
}
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using GeliosFill.Api.DTOs;
using GeliosFill.Models;

namespace GeliosFill.Api.ViewModels;

public static class UserFillViewModel
{
    public static readonly Func<UserFillInfo, FuelHistoryDTO> Create = Projection.Compile();

    private static Expression<Func<UserFillInfo, FuelHistoryDTO>> Projection =>
        userFillInfo =>
            new FuelHistoryDTO(
                userFillInfo.CarName,
                userFillInfo.CardId,
                userFillInfo.FuelFillHistories
                    .Select(fuelFillHistory => new CarDetalsDTO(
                        fuelFillHistory.Distance,
                        fuelFillHistory.DateOfFill,
                        fuelFillHistory.FillPlace,
                        fuelFillHistory.Latt,
                        fuelFillHistory.Long,
                        fuelFillHistory.Liters,
                        fuelFillHistory.UnitPrice,
                        fuelFillHistory.TotalPrice))
                    .ToList());
    /*userFillInfo.FuelFillHistories.Select(x => new
   {
       x.Distance,
       x.DateOfFill,
       x.FillPlace,
       x.Latt,
       x.Long,
       x.Liters,
       x.UnitPrice,
       x.TotalPrice
   }).ToList());*/

}
/*new FuelHistoryDTO(userFillInfo.CarName, 
           userFillInfo.CardId,
           userFillInfo.FuelFillHistories.Select(n=>n.Distance),
           userFillInfo.FuelFillHistories.Select(n=>n.DateOfFill),
           userFillInfo.FuelFillHistories.Select(n=>n.FillPlace),
           userFillInfo.FuelFillHistories.Select(n=>n.Latt),
           userFillInfo.FuelFillHistories.Select(n=>n.Long),
           userFillInfo.FuelFillHistories.Select(n=>n.Liters),
           userFillInfo.FuelFillHistories.Select(n=>n.UnitPrice),
           userFillInfo.FuelFillHistories.Select(n=>n.TotalPrice).ToList());*/


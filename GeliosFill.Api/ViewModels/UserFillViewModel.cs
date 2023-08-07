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

}


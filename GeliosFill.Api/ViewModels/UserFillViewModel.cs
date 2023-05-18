using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using GeliosFill.Models;

namespace GeliosFill.Api.ViewModels;

public static class UserFillViewModel
{
    public static readonly Func<UserFillInfo, object> Create = Projection.Compile();

    private static Expression<Func<UserFillInfo, object>> Projection =>
        userFillInfo => new
        {
            userFillInfo.CarName,
            userFillInfo.CardId,
            UserFuelHistories = userFillInfo.FuelFillHistories.Select(x => new
            {
                x.Distance,
                x.DateOfFill,
                x.FillPlace,
                x.Latt,
                x.Long,
                x.Liters,
                x.UnitPrice,
                x.TotalPrice
            }).ToList()
        };

}


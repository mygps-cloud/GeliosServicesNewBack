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
            UserFuelHistories = userFillInfo.FuelFillHistories.Select(x=>new
            {
                x.Distance,
                x.DateOfFill
            }).ToList()
        };

}
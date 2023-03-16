using System.Text.RegularExpressions;
using GeliosFill.Api.AppServices.UserAppService;
using GeliosFill.Api.ViewModels;
using GeliosFill.Data;
using GeliosFill.Models;

namespace GeliosFill.Api.AppServices.FineAppService;

public class Fine : IFine
{
    private readonly SmsSenderDbContext _ctx;
    private readonly IUser _iUser;

    private static readonly Regex RegexForGeliosCarNumber = new
    (@"(?<carNumber>[a-zA-Z0-9\-_]+)",
        RegexOptions.Compiled);

    public Fine(SmsSenderDbContext ctx, IUser iUser)
    {
        _ctx = ctx;
        _iUser = iUser;
    }

    public async Task<object> GetFines(UserInfo userInfo)
    {
        var carsInfo = await _iUser.GetCarInfoFromGelios(_iUser.BuildUrl(userInfo));

        if (carsInfo == null || carsInfo.Count < 1)
            return new object();

        var carNumbers = carsInfo
            .Select(carInfo => RegexForGeliosCarNumber.Match(carInfo.name))
            .Where(match => match.Success)
            .Select(match => match.Groups["carNumber"].Value.Trim().Replace("-", "").Replace("_", ""))
            .ToList();

        var fines = _ctx.ReceivedSms
            .Where(sms => sms.Parsed && carNumbers.Contains(
                sms.CarNumber!)).AsEnumerable()
            .Select(UserFineViewModel.Create)
            .ToList();

        return fines;

    }
}
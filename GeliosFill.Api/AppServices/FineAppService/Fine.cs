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
            .Where(sms => sms.Parsed && carNumbers.Contains(sms.CarNumber!))
            .AsEnumerable()
            .Select(UserFineViewModel.Create)
            .OrderBy(s => Convert.ToInt32(s.FineStatus) == Convert.ToInt32(FineStatus.Paid))
            .ToList();


        /*fines.ForEach(fine => fine.FineStatus = 0);

        /*foreach (var fine in fines)
        {#1#
        Parallel.ForEach(fines, fine =>
        {
            var fineUrl = BuildQuery(fine.CarNumber, fine.ReceiptNumber);
            var fineJsonResult = GetFineStatusAsync(fineUrl).Result;

            if (fineJsonResult.Contains("გადახდილია"))
                fine.FineStatus = FineStatus.Paid;
            else if (fineJsonResult.Contains("გადაუხდელია"))
                fine.FineStatus = FineStatus.Unpaid;
            else
                fine.FineStatus = FineStatus.NotFound;
        });
            
        /*};#1#*/
        return fines;
    }
    
    /*private string BuildQuery(string carNumber, string receiptNumber)
        => $"https://jarima.ge/back/result_n.php?input_v_1_val={carNumber}&input_v_2_val={receiptNumber}&index_select_val=car_id_penalty_n";*/
    
    /*private async Task<string> GetFineStatusAsync(string url)
    {
        var client = new HttpClient();
        var response = await client.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();
        
        client.Dispose();
        client = new HttpClient();
        client.BaseAddress = new Uri(url );
        
        return json.Trim();
    }*/

}
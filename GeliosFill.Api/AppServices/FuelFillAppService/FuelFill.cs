using System.Globalization;
using System.Runtime.InteropServices;
using GeliosFill.Api.ViewModels;
using GeliosFill.Data;
using GeliosFill.Models;
using GoogleMap.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GeliosFill.Api.AppServices.FuelFillAppService;

public class FuelFill : IFuelFill
{
    private readonly MyGpsDbContext _ctx;
    private readonly AppDbContext _context;
    private readonly DateTime _unixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);


    public FuelFill(MyGpsDbContext ctx, AppDbContext context)
    {
        _ctx = ctx;
        _context = context;
    }

    public async Task CalculateFuelFillHistory(UserInfo userInfo)
    {
        var userFillInfos = GetUserFillInfos(userInfo.Username);
        foreach (var info in userFillInfos)
        {
            if (info.CarId == 0) continue;
            var lastDateOfFill = GetLastDateOfFillForUser(userInfo, info.CarId);
            var fills = GetFills(info.CardId, lastDateOfFill);

            foreach (var fill in fills)
            {
                // AddFuelHistoryIntoDatabase(fill, info, userInfo);
                var ags = GetAgs(fill.AGSID);
                if (ags == null) continue;
                string result=await GetNamedLocation(ags.latt, ags.longt);
                var distance = await GetDistance(ags, fill, userInfo, info);

                
                var fuelFillHistory = new FuelFillHistory
                {
                    Distance = distance,
                    DateOfFill = fill.FillDateTime,
                    FillPlace = result,
                    Latt = ags.latt,
                    Long = ags.longt,
                    Liters = fill.Liters ?? 0M,
                    UnitPrice = fill.UnitPrice ?? 0M,
                    TotalPrice = fill.Sum ?? 0M,
                    UserFillInfoId = info.Id,
                };
               
                await AddFuelHistoryIntoDatabase(fuelFillHistory);
            }
        }
    }

    private DateTime? GetLastDateOfFillForUser(UserInfo userInfo, int carId)
    {
        var query = _context.FuelFillHistories
            .Include(x => x.UserFillInfo)
            .Where(x => x.UserFillInfo.Username.Equals(userInfo.Username) && x.UserFillInfo.CarId.Equals(carId));

        if (!query.Any())
            return null;
        return query.Max(x => x.DateOfFill);
    }

    public object GetFuelHistory(UserInfo userInfo)
        =>
            _context.UserFillInfos
                .Where(x => x.Username.Equals(userInfo.Username))
                .Include(y => y.FuelFillHistories)
                .Select(UserFillViewModel.Create)
                .ToList();
    

    private async Task<double> GetDistance(Ags ags, Fill fill, UserInfo userInfo,
        UserFillInfo info)
    {
        var carLocation = await GetCarLocation(fill, userInfo, info);

        return CalculateDistance(ags, carLocation);
    }

    private async Task<CarLocation> GetCarLocation(Fill fill, UserInfo userInfo, UserFillInfo info)
    {
        var timestamp = CalculateTimeStamp(fill);

        var url = BuildUrl(userInfo, info, timestamp);

        var data = await GetCarLocationOnExactTime(url, fill);

        return data;
    }

    private static async Task<CarLocation> GetCarLocationOnExactTime(string url, Fill fill)
    {
        var data = await GetReportFromGelios(url);
        if (data.Count < 1)
            return new CarLocation();

        var closestItem = GetClosestItemToProvidedTime(data, fill);
        var carLocation = new CarLocation()
        {
            lat = closestItem.lat,
            lon = closestItem.lon
        };
        return carLocation;
    }

    private static async Task<List<Models.Data>> GetReportFromGelios(string url)
    {
        // Send Request to Gelios Api
        using var client = new HttpClient();
        var response = await client.GetAsync(url);
        var jsonString = await response.Content.ReadAsStringAsync();
        var gpsData = JsonConvert.DeserializeObject<GPSData>(jsonString)!;

        return gpsData.tables[0].data;
    }

    private static Models.Data GetClosestItemToProvidedTime(IEnumerable<Models.Data> data, Fill fill)
        =>
            data.OrderBy(i =>
                (fill.FillDateTime - DateTime.ParseExact(i.time_start, "dd/MM/yyyy HH:mm:ss",
                    CultureInfo.InvariantCulture)).Duration()).First();


    private static string BuildUrl(UserInfo userInfo, UserFillInfo info, long timestamp)
        =>
            $"https://gadm.mygps.ge/sdk/?login={userInfo.Username}&pass={userInfo.Password}&svc=exec_report&params=" +
            $"{{\"id_unit\": {info.CarId},\"from\": " +
            $"{timestamp - 6 * 60}, \"to\": {timestamp + 6 * 60}," +
            $"\"report_type\": 7}}";

    private long CalculateTimeStamp(Fill fill)
        =>
            (long) (fill.FillDateTime.ToUniversalTime() - _unixEpoch).TotalSeconds;


    private async Task AddFuelHistoryIntoDatabase(FuelFillHistory fuelFillHistory)
    {
        _context.FuelFillHistories.Add(fuelFillHistory);
        await _context.SaveChangesAsync();
    }

    private IEnumerable<UserFillInfo> GetUserFillInfos(string username)
        =>
            _context.UserFillInfos.Where(x => x.Username.Equals(username))
                .ToList();

    private IEnumerable<Fill> GetFills(string cardId, DateTime? lastDateOfFill = null)
    {
        lastDateOfFill ??= new DateTime(DateTime.Now.Year, 1, 1);
        return _ctx.Fills.Where(x => x.CardID.Equals(cardId) && x.FillDateTime > lastDateOfFill.Value).ToList();
    }
    private Ags? GetAgs(string agsId)
        =>
            _ctx.AGS.FirstOrDefault(x => x.Name.Contains(agsId));


    private static double CalculateDistance(Ags ags, CarLocation carLocation)
    {
        // The math module contains
        // a function named ConvertToRadians
        // which converts from degrees
        // to radians.
        double lat1 = ConvertToRadians(Decimal.ToDouble(ags.latt));
        double lat2 = ConvertToRadians(carLocation.lat);
        double lon1 = ConvertToRadians(Decimal.ToDouble(ags.longt));
        double lon2 = ConvertToRadians(carLocation.lon);

        // Haversine formula
        double dlon = lon2 - lon1;
        double dlat = lat2 - lat1;
        double a = Math.Pow(Math.Sin(dlat / 2), 2) +
                   Math.Cos(lat1) * Math.Cos(lat2) *
                   Math.Pow(Math.Sin(dlon / 2), 2);

        double c = 2 * Math.Asin(Math.Sqrt(a));

        // Radius of earth in
        // kilometers. Use 3956
        // for miles
        double r = 6371;

        // calculate the result
        return c * r;
    }

    // Angle in 10th of a degree
    private static double ConvertToRadians(double angleIn10ThofaDegree) => (angleIn10ThofaDegree * Math.PI) / 180;

    private static async Task<string> GetNamedLocation(decimal latitude, decimal longitude)
    {
        const string api_key = "AIzaSyBiNzOT0oypzu-Izc2mBwWrV3j7CepjODU";
        string apiUrl = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={latitude},{longitude}&key={api_key}";
        string result = "";
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                UnifyingObject? data = JsonConvert.DeserializeObject<UnifyingObject>(jsonResponse);

                var dt = data.results[1].address_components;
              
                for (int i = 0; i <= 3; i++)
                {
                    try
                    {
                        if (i == 0)
                            continue;
                        result += dt[i].long_name + ((char)32);
                    }
                    catch (Exception e)
                    {
                        return "Unknown";
                    }
                }
            }
            else
            {
                return "Unknown";
            }
        }
        return result;
    }
}
using System.Text.Json;
using GeliosFill.Api.Form;
using GeliosFill.Data;
using GeliosFill.Models;

namespace GeliosFill.Api.AppServices.UserAppService;

public class User : IUser
{
    private readonly AppDbContext _ctx;

    public User(AppDbContext ctx)
    {
        _ctx = ctx;
    }


    public async Task<string> AddUserFillInfo(UserFillInfoForm userFillInfoForm)
    {
        var userFillInfo = GetUserFillInfo(userFillInfoForm);
        if (userFillInfo)
            return "Exists";
        
        var url = BuildUrl(userFillInfoForm);
        var carInfos = await GetCarInfoFromGelios(url);

        if (carInfos == null)
            return "Error";
        var carId = GetCarId(carInfos, userFillInfoForm);

        await AddUserFillInfoIntoDatabase(userFillInfoForm, carId);
        return "Ok";
    }

    private bool GetUserFillInfo(UserFillInfoForm userFillInfoForm)
    =>
        _ctx.UserFillInfos
            .Any(x => x.CarName.Equals(userFillInfoForm.CarName));

    private static int GetCarId(IEnumerable<CarInfo> carInfos, UserFillInfoForm userFillInfoForm)
        => carInfos
            .Where(x => x.name.Contains(userFillInfoForm.CarName, StringComparison.InvariantCultureIgnoreCase))
            .Select(y => y.id)
            .FirstOrDefault();

    private static async Task<List<CarInfo>?> GetCarInfoFromGelios(string url)
    {
        // Send Request to Gelios Api
        using var client = new HttpClient();
        var response = await client.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();
        var carInfos = JsonSerializer.Deserialize<List<CarInfo>>(json);

        return carInfos;
    }

    private static string BuildUrl(UserFillInfoForm userFillInfoForm)
        =>
            $"https://gadm.mygps.ge/sdk/?login={userFillInfoForm.Username}&pass={userFillInfoForm.Password}&svc=get_units";

    private async Task AddUserFillInfoIntoDatabase(UserFillInfoForm userFillInfoForm, int carId)
    {
        _ctx.UserFillInfos.Add(new UserFillInfo()
        {
            CarName = userFillInfoForm.CarName,
            CarId = carId,
            CardId = userFillInfoForm.CardId,
            Username = userFillInfoForm.Username
        });
        await _ctx.SaveChangesAsync();
    }
}
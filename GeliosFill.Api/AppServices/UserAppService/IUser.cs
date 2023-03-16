using GeliosFill.Api.Form;
using GeliosFill.Models;

namespace GeliosFill.Api.AppServices.UserAppService;

public interface IUser
{
    Task<string> AddUserFillInfo(UserFillInfoForm userFillInfoForm);

    Task<List<CarInfo>?> GetCarInfoFromGelios(string url);
    string BuildUrl(UserInfo userInfo);
}
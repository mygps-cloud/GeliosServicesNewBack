using GeliosFill.Api.Form;

namespace GeliosFill.Api.AppServices.UserAppService;

public interface IUser
{
    Task<string> AddUserFillInfo(UserFillInfoForm userFillInfoForm);
}
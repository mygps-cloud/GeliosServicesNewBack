using GeliosFill.Models;

namespace GeliosFill.Api.AppServices.FineAppService;

public interface IFine
{
    Task<object> GetFines(UserInfo userInfo);
}
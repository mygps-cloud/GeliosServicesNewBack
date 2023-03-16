using GeliosFill.Api.AppServices.UserAppService;
using GeliosFill.Api.Form;
using Microsoft.AspNetCore.Mvc;

namespace GeliosFill.Api.Controllers;

[ApiController]
[Route("api/excel")]
public class ExcelController: ControllerBase
{
    private readonly IUser _iUser;

    public ExcelController(IUser iUser)
    {
        _iUser = iUser;
    }

    [HttpPost]
    public async Task<IActionResult> Read(List<UserFillInfoForm> userFillInfoForms)
    {
        foreach (var userFillInfoForm in userFillInfoForms)
        {
            await _iUser.AddUserFillInfo(userFillInfoForm);
        }
        
        return Ok("Cars successfully imported from excel!");
    }
    
}
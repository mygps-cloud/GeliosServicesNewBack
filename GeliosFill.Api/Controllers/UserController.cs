using GeliosFill.Api.AppServices.UserAppService;
using GeliosFill.Api.Form;
using Microsoft.AspNetCore.Mvc;

namespace GeliosFill.Api.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUser _iUser;

    public UserController(IUser iUser)
    {
        _iUser = iUser;
    }


    [HttpPost]
    public async Task<IActionResult> Create(UserFillInfoForm userFillInfoForm)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        var response = await _iUser.AddUserFillInfo(userFillInfoForm);
        return response switch
        {
            "Exists" => BadRequest("Car Already Exists"),
            "Error" => BadRequest("Incorrect Info"),
            _ => Ok("Car Successfully Added")
        };
    }
}
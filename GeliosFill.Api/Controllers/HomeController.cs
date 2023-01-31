using GeliosFill.Api.AppServices.FuelFillAppService;
using GeliosFill.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeliosFill.Api.Controllers;

[ApiController]
[Route("api/home")]
public class HomeController : ControllerBase
{
    private readonly IFuelFill _iFuelFill;

    public HomeController(IFuelFill fuelFill)
    {
        _iFuelFill = fuelFill;
    }

    [HttpPost]
    public async Task<IActionResult> GetForUser([FromBody] UserInfo userInfo)
    {
        if (!ModelState.IsValid)
            return BadRequest("Please provide username and password");

        await _iFuelFill.CalculateFuelFillHistory(userInfo);

        var result = _iFuelFill.GetFuelHistory(userInfo);
        return Ok(result);
    }

}
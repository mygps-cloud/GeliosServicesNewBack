using GeliosFill.Api.AppServices.FineAppService;
using GeliosFill.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeliosFill.Api.Controllers;

[ApiController]
[Route("api/sms")]
public class SmsController : ControllerBase
{
    private readonly IFine _iFine;

    public SmsController(IFine iFine)
    {
        _iFine = iFine;
    }

    [HttpPost]
    public async Task<IActionResult> GetForUser([FromBody] UserInfo userInfo)
    {
        if (!ModelState.IsValid)
            return BadRequest("Please provide username and password");

        var result = await _iFine.GetFines(userInfo);

        return Ok(result);
    }
}
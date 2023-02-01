using Microsoft.AspNetCore.Mvc;

namespace GeliosFill.Api.Controllers;

[ApiController]
[Route("api/excel")]
public class ExcelController: ControllerBase
{

    [HttpPost]
    public IActionResult Read()
    {
        return Ok();
    }
    
}
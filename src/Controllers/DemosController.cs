using System;
using APIApiDemo.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace APIApiDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class DemosController : ControllerBase
{
    [HttpPost("{id:guid}/to-response")]
    public IActionResult CustomerWithoutConfig(
        [FromRoute] Guid id,
        [FromBody] ProductRequest request
    ) => Ok(request.ToResponse(id));
}

using System;
using APIApiDemo.DTOs;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace APIApiDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class ExtensionsController : ControllerBase
{
    private readonly ILogger<ExtensionsController> _logger;

    public ExtensionsController(ILogger<ExtensionsController> logger)
        => _logger = logger;

    [HttpPost("customer-without-config")]
    public IActionResult CustomerWithoutConfig(CustomerRequest request)
    {
        var response = request.Adapt<CustomerReponse>();

        return Ok(response);
    }

    [HttpPost("{id:guid}/customer-with-config")]
    public IActionResult CustomerWithConfig(
        [FromRoute] Guid id,
        [FromBody] CustomerRequest request
    )
    {
        var config = new TypeAdapterConfig();

        config.NewConfig<(Guid Id, CustomerRequest Request), CustomerReponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.FullName, src => $"{src.Request.FirstName} {src.Request.LastName}")
            .Map(dest => dest, src => src.Request);

        var response = (id, request).Adapt<CustomerReponse>(config);

        return Ok(response);
    }

    [HttpPost("{id:guid}/customer-global-config")]
    public IActionResult CustomerGlobalConfig(
        [FromRoute] Guid id,
        [FromBody] CustomerRequest request
    )
    {
        var response = (id, request).Adapt<CustomerReponse>();

        return Ok(response);
    }

    [HttpPost("{id:guid}/customer-from-global-config")]
    public IActionResult CustomerFromGlobalConfig(
        [FromRoute] Guid id,
        [FromBody] CustomerRequest request
    )
    {
        var config = new TypeAdapterConfig();

        config.ForType<(Guid Id, CustomerRequest Request), CustomerReponse>()
            .Map(dest => dest.FullName, src => $"From name: {src.Request.FirstName} {src.Request.LastName}");

        var response = (id, request).Adapt<CustomerReponse>(config);

        return Ok(response);
    }

    [HttpPost("customer-with-events")]
    public IActionResult CustomerEvents(CustomerRequest request)
    {
        var config = new TypeAdapterConfig();

        config.NewConfig<CustomerRequest, CustomerReponse>()
            .BeforeMapping(dest => _logger.LogInformation($"Before {dest}"))
            .AfterMapping(dest => _logger.LogInformation($"After {dest}"));

        var response = request.Adapt<CustomerReponse>(config);

        return Ok(response);
    }

    [HttpPost("customer-with-condition")]
    public IActionResult CustomerWithCondition(CustomerRequest request)
    {
        var config = new TypeAdapterConfig();

        config.NewConfig<CustomerRequest, CustomerReponse>()
            .Map(
                dest => dest.FullName,
                src => $"{src.FirstName} {src.LastName}",
                src => src.FirstName.StartsWith("n", StringComparison.InvariantCultureIgnoreCase)
            );

        var response = request.Adapt<CustomerReponse>(config);

        return Ok(response);
    }
}

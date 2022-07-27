using System;
using APIApiDemo.DTOs;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace APIApiDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class AdaptController : ControllerBase
{
    private readonly ILogger<AdaptController> _logger;

    public AdaptController(ILogger<AdaptController> logger)
        => _logger = logger;



    [HttpPost("clone")]
    public IActionResult Clone(ProductRequest request)
    {
        ProductRequest response = new();

        request.Adapt(response);

        return Ok(new
        {
            SourceHashCode = request.GetHashCode(),
            DestinationHashCode = response.GetHashCode(),
            IsEquals = request == response,
            New = response
        });
    }

    [HttpPost("copy-to-different-type")]
    public IActionResult CopyToDifferentType(CustomerRequest request)
    {
        var response = request.Adapt<CustomerReponse>();

        return Ok(response);
    }

    [HttpPost("from-record-to-class")]
    public IActionResult FromRecordToClass(OrderDto request)
    {
        var response = request.Adapt<Order>();

        return Ok(response);
    }

    [HttpPost("{id:guid}/using-mapping-configurations")]
    public IActionResult UsingMappingConfigurations(
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

    [HttpPost("{id:guid}/using-mapping-global-configurations")]
    public IActionResult UsingMappingGlobalConfigurations(
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

    [HttpPost("using-callbacks")]
    public IActionResult UsingCallbacks(CustomerRequest request)
    {
        var config = new TypeAdapterConfig();

        config.NewConfig<CustomerRequest, CustomerReponse>()
            .BeforeMapping(dest => _logger.LogInformation($"Before {dest}"))
            .AfterMapping(dest => _logger.LogInformation($"After {dest}"));

        var response = request.Adapt<CustomerReponse>(config);

        return Ok(response);
    }

    [HttpPost("using-conditions")]
    public IActionResult UsingConditions(CustomerRequest request)
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

    [HttpPost("{id:guid}/to-response")]
    public IActionResult ToResponse(
        [FromRoute] Guid id,
        [FromBody] ProductRequest request
    ) => Ok(request.ToResponse(id));

    [HttpPost("parameterless-constructor")]
    public IActionResult ParameterlessConstructor(
        [FromBody] PostRequest request
    ) => Ok(request.Adapt<PostValueObject>());

    [HttpPost("inheritance-without-default-constructor")]
    public IActionResult InheritanceWithoutDefaultConstructor(
        [FromBody] PostRequest request
    )
    {
        var config = new TypeAdapterConfig();

        config.NewConfig<PostRequest, Article>()
            .MapToConstructor(true);

        return Ok(request.Adapt<Article>(/*config*/));
    }
}

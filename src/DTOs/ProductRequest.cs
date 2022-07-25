using System;
using Mapster;

namespace APIApiDemo.DTOs;

public record ProductRequest
{
    public string Name { get; set; }
    public uint Quantiy { get; set; }

    public ProductResponse ToResponse(Guid id)
        => (id, this).Adapt<ProductResponse>();
}

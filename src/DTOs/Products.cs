using System;
using Mapster;

namespace APIApiDemo.DTOs;

public class ProductRequest
{
    public string Name { get; set; }
    public uint Quantiy { get; set; }

    public ProductResponse ToResponse(Guid id)
        => (id, this).Adapt<ProductResponse>();
}


public class ProductResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public uint Quantiy { get; set; }
}

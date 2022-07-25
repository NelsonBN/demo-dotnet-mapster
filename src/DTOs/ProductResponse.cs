using System;

namespace APIApiDemo.DTOs;

public record ProductResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public uint Quantiy { get; set; }
}

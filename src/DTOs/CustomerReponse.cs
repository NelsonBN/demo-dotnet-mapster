using System;

namespace APIApiDemo.DTOs;

public record CustomerReponse
{
    public Guid Id { get; init; }

    public string FullName { get; init; }

    public string AddressLine1 { get; init; }

    public uint Age { get; init; }
}

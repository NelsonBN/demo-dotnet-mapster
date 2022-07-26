using System;

namespace APIApiDemo.DTOs;

public record CustomerRequest
{
    public string FirstName { get; init; }
    public string LastName { get; init; }

    public string AddressLine1 { get; init; }

    public uint Age { get; init; }
}

public record CustomerReponse
{
    public Guid Id { get; init; }

    public string FullName { get; init; }

    public string AddressLine1 { get; init; }

    public uint Age { get; init; }
}

namespace APIApiDemo.DTOs;

public record CustomerRequest
{
    public string FirstName { get; init; }
    public string LastName { get; init; }

    public string AddressLine1 { get; init; }

    public uint Age { get; init; }
}

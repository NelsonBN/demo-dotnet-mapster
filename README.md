# demo-dotnet-mapster



## Nuget
[Mapster](https://www.nuget.org/packages/Mapster/)


## Examples

### Models
```csharp
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
```



### Basic exanple with `Adapt` extension

```csharp
var response = request.Adapt<CustomerReponse>();
```


### `Adapt` extension with configuration

```csharp
var config = new TypeAdapterConfig();

config.NewConfig<(Guid Id, CustomerRequest Request), CustomerReponse>()
    .Map(dest => dest.Id, src => src.Id)
    .Map(dest => dest.FullName, src => $"{src.Request.FirstName} {src.Request.LastName}")
    .Map(dest => dest, src => src.Request);

var response = (id, request).Adapt<CustomerReponse>(config);
```


### `Adapt` extension with global configuration


```csharp
TypeAdapterConfig<(Guid Id, CustomerRequest Request), CustomerReponse>.NewConfig()
    .Map(dest => dest.Id, src => src.Id)
    .Map(dest => dest.FullName, src => $"Your name is '{src.Request.FirstName} {src.Request.LastName}'")
    .Map(dest => dest.AddressLine1, src => $"Your address is '{src.Request.AddressLine1}'")
    .Map(dest => dest, src => src.Request);
```

```csharp
var response = (id, request).Adapt<CustomerReponse>();
```


### `Adapt` extension with events


```csharp
config.NewConfig<CustomerRequest, CustomerReponse>()
    .BeforeMapping(dest => _logger.LogInformation($"Before {dest}"))
    .AfterMapping(dest => _logger.LogInformation($"After {dest}"));

var response = request.Adapt<CustomerReponse>(config);
```


### `Adapt` extension with condition


```csharp
var config = new TypeAdapterConfig();

config.NewConfig<CustomerRequest, CustomerReponse>()
    .Map(
        dest => dest.FullName,
        src => $"{src.FirstName} {src.LastName}",
        src => src.FirstName.StartsWith("n", StringComparison.InvariantCultureIgnoreCase)
    );

var response = request.Adapt<CustomerReponse>(config);
```
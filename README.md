# demo-dotnet-mapster


## Nuget
[Mapster](https://www.nuget.org/packages/Mapster/)

## Examples

### `Adapt` extension

**Models**
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



public record OrderDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}


public record PostRequest
{
    public string Author { get; set; }
    public string Text { get; set; }
    public bool Like { get; set; }
}

public record PostValueObject(string Author, string Text, bool Like);



public abstract record ArticleBase
{
    public DateTime OccurredAt { get; init; }

    protected ArticleBase()
        => OccurredAt = DateTime.UtcNow;
}

public record Article(string Author, string Text, bool Like) : ArticleBase;
```


#### Clone to a new object with the same type

```csharp
ProductRequest response = new();
request.Adapt(response);
```


#### Copy to an object with a different type

```csharp
CustomerRequest request ...

var response = request.Adapt<CustomerReponse>();
```


#### From a record to a class

```csharp
OrderDto request ...

var response = request.Adapt<Order>();
```


#### Using mapping configurations

```csharp
var config = new TypeAdapterConfig();

config.NewConfig<(Guid Id, CustomerRequest Request), CustomerReponse>()
    .Map(dest => dest.Id, src => src.Id)
    .Map(dest => dest.FullName, src => $"{src.Request.FirstName} {src.Request.LastName}")
    .Map(dest => dest, src => src.Request);

var response = (id, request).Adapt<CustomerReponse>(config);
```


#### Using mapping global configurations

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


#### Using callbacks

```csharp
config.NewConfig<CustomerRequest, CustomerReponse>()
    .BeforeMapping(dest => _logger.LogInformation($"Before {dest}"))
    .AfterMapping(dest => _logger.LogInformation($"After {dest}"));

var response = request.Adapt<CustomerReponse>(config);
```


#### Using conditions

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


#### Example using a method in source the create a new destination

```csharp
var config = new TypeAdapterConfig();

config.NewConfig<CustomerRequest, CustomerReponse>()
    .Map(
        dest => dest.FullName,
        src => $"{src.FirstName} {src.LastName}",
        src => src.FirstName.StartsWith("n", StringComparison.InvariantCultureIgnoreCase)
    );

var response = request.Adapt<CustomerReponse>(config);
response = request.Adapt<CustomerReponse>(config);
```


#### Parameterless constructor

```csharp
PostRequest request = new();
var request.Adapt<PostValueObject>();
```


#### Inheritance without default constructor

```csharp
PostRequest request = new();

var config = new TypeAdapterConfig();
config.NewConfig<PostRequest, Article>()
    .MapToConstructor(true);

var request.Adapt<Article>(config);
```


#### Global configuration for destination type

```csharp
TypeAdapterConfig.GlobalSettings
    .ForDestinationType<ArticleBase>()
            .MapToConstructor(true);
```
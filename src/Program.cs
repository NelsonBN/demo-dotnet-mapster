using System;
using APIApiDemo.DTOs;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;


TypeAdapterConfig<(Guid Id, CustomerRequest Request), CustomerReponse>.NewConfig()
    .Map(dest => dest.Id, src => src.Id)
    .Map(dest => dest.FullName, src => $"Your name is '{src.Request.FirstName} {src.Request.LastName}'")
    .Map(dest => dest.AddressLine1, src => $"Your address is '{src.Request.AddressLine1}'")
    .Map(dest => dest, src => src.Request);


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();



app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

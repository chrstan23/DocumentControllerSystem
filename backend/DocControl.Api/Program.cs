using DocControl.Api.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IRequestorService, RequestorService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IProcedureService, ProcedureService>();

const long maxRequestBytes = 210L * 1024 *1024; //max MB for files
builder.Services.Configure<FormOptions>(o => o.MultipartBodyLengthLimit = maxRequestBytes); //Procedure Services
builder.WebHost.ConfigureKestrel( o => o.Limits.MaxRequestBodySize = maxRequestBytes);  //Procedure Services
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod());
}); //Connection for frontend address and port


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors("Frontend");

app.Run();

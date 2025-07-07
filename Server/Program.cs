using Bl.Api;
using Bl.Services;
using Dal.Models;
using Dal.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpsPolicy;   // HttpsRedirectionOptions
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. DbContext
builder.Services.AddDbContext<dbClass>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("YedidimDb")));

// 2. HttpClientFactory (ל-GoogleMapsService)
builder.Services.AddHttpClient();

// 3. DAL
builder.Services.AddScoped<CallDal>();
builder.Services.AddScoped<ClientDal>();
builder.Services.AddScoped<VolunteerDal>();

// 4. BL
builder.Services.AddScoped<ICallBl, CallServiceBl>();
builder.Services.AddScoped<IClientBl, ClientServiceBl>();
builder.Services.AddScoped<IVolunteerBl, VolunteerServiceBl>();

// 5. Google Maps
builder.Services.AddScoped<IGoogleMapsService, GoogleMapsService>();

// 6. MVC + Swagger
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// Dev-extras
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 7. HTTPS redirection
app.UseHttpsRedirection();

app.UseCors();

app.MapControllers();
app.Run();

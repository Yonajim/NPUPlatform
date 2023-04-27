using Microsoft.EntityFrameworkCore;
using NpuCreationService.Data;
using NpuCreationService.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<NpuDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NpuDatabase")));

builder.Services.AddSingleton<IFileStorageService>(x =>
    new LocalFileStorageService(configuration.GetValue<string>("FileStorage:BasePath")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
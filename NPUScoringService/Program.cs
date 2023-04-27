using Microsoft.EntityFrameworkCore;
using NpuScoreService.Data;
using NpuScoreService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<NpuCreationClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["NpuCreationServiceUrl"]);
});

builder.Services.AddDbContext<NpuScoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NpuScoreDatabase")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

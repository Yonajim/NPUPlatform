using Ocelot.DependencyInjection;
using Ocelot.Middleware;

// Create a WebApplication builder with the provided arguments
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("ocelot.json");

// Add services to the container.
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddSwaggerForOcelot(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Build the WebApplication
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwaggerForOcelotUI(options => options.PathToSwaggerGenerator = "/swagger/docs");
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gateway");
        options.RoutePrefix = "swagger";
    });
}

app.UseOcelot().Wait();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
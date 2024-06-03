var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApiDocument();
var app = builder.Build();
app.UseDeveloperExceptionPage();
app.UseRouting();
app.UseOpenApi();
app.UseSwaggerUi();
app.MapControllers();
app.Run();

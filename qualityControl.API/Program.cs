using qualityControl.API.Data;
using qualityControl.SHARED.Interfaces;
using qualityControl.API.Repositories;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();
builder.Services.AddScoped<IProductionRepo, ProductionRepo>();
builder.Services.AddScoped<IWorkOrderRepo, WorkOrderRepo>();
builder.Services.AddScoped<IItemRepo, ItemRepo>();
builder.Services.AddScoped<IQualityControlRepo, QualityControlRepo>();
builder.Services.AddScoped<IQualityControlResultRepo, QualityControlResultRepo>();

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

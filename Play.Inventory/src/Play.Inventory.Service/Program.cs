using Play.Common;
using Play.Common.MassTransit;
using Play.Common.Repository;
using Play.Inventory.Service.Client;
using Play.Inventory.Service.Entities;
using Polly;
using Polly.Timeout;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Play.Inventory.Service", Version = "v1" });
});

builder.AddMongo().AddMongoRepository<InventoryItem>("inventoryItems").AddMongoRepository<CatalogItem>("catalogItem");
builder.AddMassTransitWithRabbitMq();

Random jitterer = new();
builder.Services.AddHttpClient<CatalogClient>()
.AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>().WaitAndRetryAsync(5, retryCnt => TimeSpan.FromSeconds(Math.Pow(2, retryCnt)) + TimeSpan.FromSeconds(jitterer.Next(0, 1000))))
.AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>().CircuitBreakerAsync(3, TimeSpan.FromSeconds(15)))
.AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

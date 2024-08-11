using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catalog.Service.Repository;
using Play.Common.Entities;
using Play.Common.Settings;
using MassTransit;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using static MassTransit.Logging.OperationName;

namespace Play.Common.MassTransit
{
  public static class Extensions
  {
    public static WebApplicationBuilder AddMassTransitWithRabbitMq(this WebApplicationBuilder builder)
    {
      builder.Services.AddMassTransit(x =>
      {
        x.AddConsumers(Assembly.GetEntryAssembly());

        x.UsingRabbitMq((ctx, cfg) =>
      {
        var rabbitMQSettings = builder.Configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
        var serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
        cfg.Host(rabbitMQSettings.Host);
        cfg.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));
      });
      });
      return builder;
    }

  }
}
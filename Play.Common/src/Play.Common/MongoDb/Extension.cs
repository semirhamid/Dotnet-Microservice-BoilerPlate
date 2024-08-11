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

namespace Play.Common.Repository
{
  public static class Extensions
  {
    public static WebApplicationBuilder AddMongo(this WebApplicationBuilder builder)
    {
      builder.Services.Configure<MongoDbSettings>(
          builder.Configuration.GetSection("Database"));
      BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
      BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
      builder.Services.AddSingleton(serviceProvider =>
      {
        var settings = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
        var mongoClient = new MongoClient(settings.ConnectionString);
        return mongoClient.GetDatabase(settings.DatabaseName);
      });
      return builder;
    }
    public static WebApplicationBuilder AddMongoRepository<T>(this WebApplicationBuilder builder, string collectionName) where T : IEntity
    {
      builder.Services.AddSingleton<IRepository<T>>(serviceProvider =>
        {
          var mongoDatabase = serviceProvider.GetRequiredService<IMongoDatabase>();
          return new MongoRepository<T>(mongoDatabase, collectionName);
        });
      return builder;
    }
  }
}
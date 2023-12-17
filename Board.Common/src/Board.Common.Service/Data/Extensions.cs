
using Board.Common.Interfaces;
using Board.Common.Models;
using Board.Common.Settings;
using Board.Common.Settings.Mongo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Board.Common.Mongo
{
    public static class Extensions
    {

        private static MongoSettings? _mongoSettings;
        private static ServiceSettings? _serviceSettings;

        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            services.AddSingleton(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                _serviceSettings = configuration!.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>()!;
                _mongoSettings = configuration!.GetSection(nameof(MongoSettings)).Get<MongoSettings>()!;

                var mongoClient = new MongoClient(_mongoSettings!.ConnectionString);
                return mongoClient.GetDatabase(_serviceSettings!.ServiceName);
            });

            return services;
        }

        public static IServiceCollection AddPersistence<T>(this IServiceCollection services, string collectionName) where T : BaseEntity
        {
            services.AddSingleton<IGenericRepository<T>>(serviceProvider =>
            {
                var database = serviceProvider.GetService<IMongoDatabase>();
                var collection = database!.GetCollection<T>(collectionName);
                return new GenericRepository<T>(database, collection);
            });
            return services;

        }
    }
}
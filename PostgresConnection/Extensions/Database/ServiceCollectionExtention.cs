using Microsoft.EntityFrameworkCore;
using PostgresConnection.Context;

namespace PostgresConnection.Extensions.Database;

public static class ServiceCollectionExtention
{
    private const string ConnectionString = "PgDb";

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseNpgsql(
                config.GetConnectionString(ConnectionString),
                opt => opt.CommandTimeout(60)
            );
        });

        return services;
    }
}
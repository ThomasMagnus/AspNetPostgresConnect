using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Reflection;

namespace PostgresConnection.Context;

public class DatabaseContext : DbContext
{
    public DatabaseContext() 
    {
    }

    public DatabaseContext(DbContextOptions options)
        : base(options)
    {
    }

    public void Migrate()
    {
        Database.Migrate();
    }

    public IDbContextTransaction Transaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        return base.Database.BeginTransaction(isolationLevel);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(GetType())!);

        base.OnModelCreating(builder);
    }
}

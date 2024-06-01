using BenchmarkDotNet.Attributes;
using Bogus;
using PostgresConnection.Context;
using PostgresConnection.Entities;
using System.Collections;

namespace PostgresConnection.Benchmarks;

[MemoryDiagnoser]
public class BulkInsertBenchmarks
{
    private readonly Lazy<DatabaseContext> _context = new Lazy<DatabaseContext>();
    private static readonly Faker Faker = new Faker();


    [Params(100)]
    public int Size { get; set; }

    [Benchmark]
    public async Task EfAddOneAndSave()
    {
        foreach (User user in Generated())
        {
            _context.Value.Add(user);

            await _context.Value.SaveChangesAsync();
        }
    }

    [Benchmark]
    public async Task EfAddOneByOne()
    {
        foreach (User user in Generated())
        {
            _context.Value.Add(user);
        }

        await _context.Value.SaveChangesAsync();
    }

    [Benchmark]
    public async Task EfAddRange()
    {
        await _context.Value.AddRangeAsync(Generated());

        await _context.Value.SaveChangesAsync();
    }

    private User[] Generated()
    {
        return Enumerable
            .Range(0, Size)
            .Select(_ => new User
            {
                Id = Guid.NewGuid(),
                FirstName = Faker.Name.FirstName(),
                LastName = Faker.Name.LastName(),
                Email = Faker.Internet.Email(),
                PhoneNumber = Faker.Phone.PhoneNumber(),
            })
            .ToArray();
    }
}

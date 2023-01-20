using Npgsql;
using FluentAssertions;

namespace tests;

public class Tests
{
    private const string _testDataBaseName = "music_library_test";
    private static readonly string? _serverConnString = Environment.GetEnvironmentVariable("PostgreSQLConnectionString");
    readonly string _connString = _serverConnString + "Database=" + _testDataBaseName;
    
    [SetUp]
    public void Setup()
    {
        if (_serverConnString is null)
        {
            throw new ArgumentException(
                "Connection string not found. Store connection string in environment variable PostgreSQLConnectionString.");
        }
    }

    [Test]
    public async Task testDB_CorrectlySeeded_ShouldContainDoolittleAlbum()
    {

        await using var conn = new NpgsqlConnection(_connString);
        await conn.OpenAsync();
        
        // Query for a known value
        await using var cmd = new NpgsqlCommand("SELECT title FROM albums WHERE id = 1", conn);
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var result = reader.GetString(0);
            result.Should().Be("Doolittle", "that's how I seeded the testDB");
        }
    }
}
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
        //TODO: Extract connecting to setup
    }
    //TODO: Add a teardown to close connection

    [Test]
    public async Task testDB_CorrectlySeeded_ShouldContainDoolittleAsAlbum()
    {

        //Open a connection
        await using var conn = new NpgsqlConnection(_connString);
        await conn.OpenAsync();
        
        //Execute SQL insert statements from a file 
        var sqlString = await File.ReadAllTextAsync(@"..\..\..\seeds\music_library.sql", System.Text.Encoding.UTF8);
        var seedCommand = new NpgsqlCommand(sqlString, conn);
        seedCommand.ExecuteNonQuery();
        
        //Check that a know value is present
        await using var cmd = new NpgsqlCommand("SELECT title FROM albums WHERE id = 1", conn);
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var result = reader.GetString(0);
            result.Should().Be("Doolittle", "that's how I seeded the testDB");
        }
    }
    
    [Test]
    public async Task testDB_CorrectlySeeded_ShouldContainPixiesAsArtist()
    {

        await using var conn = new NpgsqlConnection(_connString);
        await conn.OpenAsync();
        
        // Query for a known value
        await using var cmd = new NpgsqlCommand("SELECT name FROM artists WHERE id = 1", conn);
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var result = reader.GetString(0);
            result.Should().Be("Pixies", "that's how I seeded the testDB");
        }
        //Nonsense commit to check a github process
    }
}
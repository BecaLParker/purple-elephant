using Npgsql;

namespace tests;

public class Tests
{
    [SetUp]
    public async Task Setup()
    {
        const string testDataBaseName = "music_library_test";
        string? serverConnString = Environment.GetEnvironmentVariable("PostgreSQLConnectionString");
        if (serverConnString is null)
        {
            throw new ArgumentException(
                "Connection string not found. Store connection string in environment variable PostgreSQLConnectionString.");
        }

        string connString = serverConnString + "Database=" + testDataBaseName;
        await using var conn = new NpgsqlConnection(connString);
        await conn.OpenAsync();
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}
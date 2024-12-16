using Application;
using System.Text.Json;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var servers = new List<(string, int)>
        {
            ("127.0.0.1", 5000),
            ("127.0.0.1", 5001),
            ("127.0.0.1", 5002),
            ("127.0.0.1", 5003),
            ("127.0.0.1", 5004),
            ("127.0.0.1", 5005),
            ("127.0.0.1", 5006),
            ("127.0.0.1", 5007),
            ("127.0.0.1", 5008),
            ("127.0.0.1", 5009)
        };

        var client = new SqlClient(servers);

        var queries = Enumerable.Repeat(
            "SELECT TOP (1000) [Id], [Number], [Description], [Decision], [LabId] FROM [DbLabs].[dbo].[Tasks]",
            100
        ).ToList();

        var tasks = queries.Select(query => client.ExecuteQueryAsync(query));
        await Task.WhenAll(tasks);

        Console.WriteLine("Все запросы выполнены.");
    }

}
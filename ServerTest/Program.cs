using TcpSqlServer;

internal class Program
{
    static async Task Main(string[] args)
    {
        var servers = new List<int> { 5000, 5001, 5002, 5003, 5004, 5005, 5006, 5007, 5008, 5009 };

        Console.WriteLine("Запуск серверов...");

        foreach (var port in servers)
        {
            var server = new SqlServer("Server=DESKTOP-CGA60LE;Database=DbLabs;Trusted_Connection=True; TrustServerCertificate=True");
            Task.Run(() => server.StartAsync(port));
        }

        Console.WriteLine("Серверы запущены.");
        Console.ReadLine();
    }
}
using TcpSqlServer;

public class Program
{

    static async Task Main(string[] args)
    {
        var serverTasks = new List<Task>
    {
        StartSqlServerOnPortAsync(5000),
        StartSqlServerOnPortAsync(5001),
        StartSqlServerOnPortAsync(5002)
    };

        await Task.WhenAll(serverTasks);
    }

    static async Task StartSqlServerOnPortAsync(int port)
    {
        var sqlServer = new SqlServer("Server=DESKTOP-CGA60LE;Database=DbLabs;Trusted_Connection=True; TrustServerCertificate=True");
        await sqlServer.StartAsync(port);
    }

}

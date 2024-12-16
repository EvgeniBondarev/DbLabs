using System.Net.Sockets;
using System.Text.Json;
using System.Text;

namespace Application
{
    public class SqlClient
    {
        private readonly List<(string ServerAddress, int Port)> _servers;
        private int _currentServerIndex = -1; // Индекс текущего сервера

        public SqlClient(List<(string serverAddress, int port)> servers)
        {
            _servers = servers;
        }

        private (string ServerAddress, int Port) GetNextServer()
        {
            // Круговой обход серверов
            _currentServerIndex = (_currentServerIndex + 1) % _servers.Count;
            return _servers[_currentServerIndex];
        }

        public async Task<List<Dictionary<string, object>>> ExecuteQueryAsync(string sql)
        {
            var (serverAddress, port) = GetNextServer();

            using var client = new TcpClient();
            await client.ConnectAsync(serverAddress, port);

            using var stream = client.GetStream();
            using var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
            using var reader = new StreamReader(stream, Encoding.UTF8);

            string cleanSql = sql.Replace("\n", " ").Replace("\r", "").Trim();
            await writer.WriteLineAsync(cleanSql);

            string response = await reader.ReadLineAsync();
            var result = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(response);
            Console.WriteLine(result);
            return result;
        }
    }
}

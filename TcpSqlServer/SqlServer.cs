using Dapper;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace TcpSqlServer
{
    public class SqlServer
    {
        private readonly string _connectionString;

        public SqlServer(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task StartAsync(int port)
        {
            var listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine($"SQL Server listening on port {port}");

            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                _ = Task.Run(() => HandleClientAsync(client, port)); // Асинхронный запуск обработки клиента
            }
        }



        public async Task HandleClientAsync(TcpClient client, int port)
        {
            var stopwatch = new Stopwatch();

            try
            {
                using var stream = client.GetStream();
                using var reader = new StreamReader(stream, Encoding.UTF8);
                using var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

                string sql;
                while ((sql = await reader.ReadLineAsync()) != null) // Обработка всех запросов до закрытия клиента
                {
                    stopwatch.Restart();
                    Console.WriteLine($"[Server Port: {port}] Received SQL query: {sql}");

                    var results = await Task.Run(() => ExecuteQuery(sql)); // Асинхронное выполнение запроса

                    stopwatch.Stop();
                    Console.WriteLine($"[Server Port: {port}] Executed query in {stopwatch.ElapsedMilliseconds} ms");

                    string response = JsonSerializer.Serialize(results);
                    await writer.WriteLineAsync(response); // Отправка результата клиенту
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                stopwatch.Stop();
                client.Close();
                Console.WriteLine($"[Server Port: {port}] Connection closed.");
            }
        }


        public List<Dictionary<string, object>> ExecuteQuery(string sql)
        {
            var sqlCommands = sql.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries)
                                 .Select(command => command.Trim())
                                 .Where(command => !string.IsNullOrEmpty(command))
                                 .ToList();

            var results = new List<Dictionary<string, object>>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using var transaction = connection.BeginTransaction();

                try
                {
                    foreach (var command in sqlCommands)
                    {
                        var result = connection.Query(command, transaction: transaction);
                        results.AddRange(result.Select(row => (IDictionary<string, object>)row)
                                               .Select(dict => dict.ToDictionary(kv => kv.Key, kv => kv.Value)));
                    }

                    transaction.Rollback();
                }
                catch (SqlException ex)
                {
                    
                }
            }

            return results;
        }

    }
}

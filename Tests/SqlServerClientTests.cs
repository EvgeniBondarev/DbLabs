using Application;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TcpSqlServer;
using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace Tests
{
    public class SqlServerTests
    {
        private readonly string _connectionString = "Server=DESKTOP-CGA60LE;Database=DbLabs;Trusted_Connection=True;TrustServerCertificate=True;";

        [Fact]
        public void ExecuteQuery_ValidQuery_ReturnsExpectedResults()
        {
            // Arrange
            string sql = "SELECT TOP 1 * FROM Labs";
            var expectedResults = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Id", 14},
                    {"Name", "Лабораторная работа 1"}
                }
            };

            var sqlServer = new SqlServer(_connectionString);

            // Act
            List<Dictionary<string, object>> results = sqlServer.ExecuteQuery(sql);

            // Assert
            Assert.NotNull(results);
            Assert.Single(results);

            var result = results.First();
            Assert.Equal(14, result["Id"]);
            Assert.Equal("Лабораторная работа 1", result["Name"]);
        }

        [Fact]
        public void ExecuteQuery_EmptyTable_ReturnsNoResults()
        {
            // Arrange
            string sql = "SELECT TOP 1 * FROM EmptyTable";
            var sqlServer = new SqlServer(_connectionString);

            // Act
            List<Dictionary<string, object>> results = sqlServer.ExecuteQuery(sql);

            // Assert
            Assert.NotNull(results);
            Assert.Empty(results);
        }

        [Fact]
        public void ExecuteQuery_SelectTasksTable_ReturnsExpectedResults()
        {
            // Arrange
            string sql = "SELECT TOP 1 [Id], [Number], [Description], [Decision], [LabId] FROM Tasks";
            var sqlServer = new SqlServer(_connectionString);

            // Act
            var results = sqlServer.ExecuteQuery(sql);

            // Assert
            Assert.NotNull(results);
            Assert.Single(results);

            var result = results.First();
            Assert.Equal(28, result["Id"]);
            Assert.Equal(1, result["Number"]);
            Assert.Contains("создать табличную функцию", result["Description"].ToString());
        }

        [Fact]
        public void ExecuteQuery_FilterById_ReturnsSingleRecord()
        {
            // Arrange
            string sql = "SELECT * FROM Labs WHERE Id = 15";
            var sqlServer = new SqlServer(_connectionString);

            // Act
            var results = sqlServer.ExecuteQuery(sql);

            // Assert
            Assert.NotNull(results);
            Assert.Single(results);

            var result = results.First();
            Assert.Equal(15, result["Id"]);
            Assert.Equal("Лабораторная работа 2", result["Name"]);
        }

        [Fact]
        public void ExecuteQuery_JoinLabsAndTasks_ReturnsResults()
        {
            // Arrange
            string sql = @"
                        SELECT t.Id, t.Number, t.LabId, l.Name 
                        FROM Tasks t
                        JOIN Labs l ON t.LabId = l.Id";
            var sqlServer = new SqlServer(_connectionString);

            // Act
            var results = sqlServer.ExecuteQuery(sql);

            // Assert
            Assert.NotNull(results);
            Assert.NotEmpty(results);

            var result = results.First();
            Assert.Equal(28, result["Id"]);
            Assert.Equal(14, result["LabId"]);
            Assert.Equal("Лабораторная работа 1", result["Name"]);
        }

        [Fact]
        public void ExecuteQuery_CountLabs_ReturnsRecordCount()
        {
            // Arrange
            string sql = "SELECT COUNT(*) AS TotalLabs FROM Labs";
            var sqlServer = new SqlServer(_connectionString);

            // Act
            var results = sqlServer.ExecuteQuery(sql);

            // Assert
            Assert.NotNull(results);
            Assert.Single(results);

            var result = results.First();
            Assert.Equal(3, result["TotalLabs"]); // Всего 3 записи в таблице Labs
        }

        [Fact]
        public void ExecuteQuery_OrderByName_ReturnsSortedResults()
        {
            // Arrange
            string sql = "SELECT TOP 1 * FROM Labs ORDER BY Name DESC";
            var sqlServer = new SqlServer(_connectionString);

            // Act
            var results = sqlServer.ExecuteQuery(sql);

            // Assert
            Assert.NotNull(results);
            Assert.Single(results);

            var result = results.First();
            Assert.Equal("Лабораторная работа 3", result["Name"]);
        }

    }
}

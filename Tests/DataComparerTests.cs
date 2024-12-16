using Application.Common.SQL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class DataComparerTests
    {
        private readonly DataComparer _dataComparer;

        public DataComparerTests()
        {
            _dataComparer = new DataComparer();
        }

        [Fact]
        public void CompareResults_ShouldReturnTrue_WhenResultsAreIdentical()
        {
            // Arrange
            var result1 = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "Id", 1 },
                    { "Name", "John" },
                    { "Age", 30 }
                },
                new Dictionary<string, object>
                {
                    { "Id", 2 },
                    { "Name", "Alice" },
                    { "Age", 25 }
                }
            };

            var result2 = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "Id", 1 },
                    { "Name", "John" },
                    { "Age", 30 }
                },
                new Dictionary<string, object>
                {
                    { "Id", 2 },
                    { "Name", "Alice" },
                    { "Age", 25 }
                }
            };

            // Act
            var areEqual = _dataComparer.CompareResults(result1, result2);

            // Assert
            Assert.True(areEqual);
        }

        [Fact]
        public void CompareResults_ShouldReturnFalse_WhenRecordCountsDiffer()
        {
            // Arrange
            var result1 = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "Id", 1 },
                    { "Name", "John" }
                }
            };

            var result2 = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "Id", 1 },
                    { "Name", "John" }
                },
                new Dictionary<string, object>
                {
                    { "Id", 2 },
                    { "Name", "Alice" }
                }
            };

            // Act
            var areEqual = _dataComparer.CompareResults(result1, result2);

            // Assert
            Assert.False(areEqual);
        }

        [Fact]
        public void CompareResults_ShouldReturnFalse_WhenColumnNamesDiffer()
        {
            // Arrange
            var result1 = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "Id", 1 },
                    { "Name", "John" }
                }
            };

            var result2 = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "Id", 1 },
                    { "FullName", "John" } // Column name differs
                }
            };

            // Act
            var areEqual = _dataComparer.CompareResults(result1, result2);

            // Assert
            Assert.False(areEqual);
        }

        [Fact]
        public void CompareResults_ShouldReturnFalse_WhenDataDiffers()
        {
            // Arrange
            var result1 = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "Id", 1 },
                    { "Name", "John" },
                    { "Age", 30 }
                }
            };

            var result2 = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "Id", 1 },
                    { "Name", "John" },
                    { "Age", 35 } // Data differs (Age)
                }
            };

            // Act
            var areEqual = _dataComparer.CompareResults(result1, result2);

            // Assert
            Assert.False(areEqual);
        }

        [Fact]
        public void CompareResults_ShouldReturnTrue_WhenNumericValuesAreEqual()
        {
            // Arrange
            var result1 = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "Id", 1 },
                    { "Amount", 100.50m }
                }
            };

            var result2 = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "Id", 1 },
                    { "Amount", 100.50m }
                }
            };

            // Act
            var areEqual = _dataComparer.CompareResults(result1, result2);

            // Assert
            Assert.True(areEqual);
        }

        [Fact]
        public void CompareResults_ShouldReturnFalse_WhenNumericValuesDiffer()
        {
            // Arrange
            var result1 = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "Id", 1 },
                    { "Amount", 100.50m }
                }
            };

            var result2 = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "Id", 1 },
                    { "Amount", 105.50m } // Numeric value differs
                }
            };

            // Act
            var areEqual = _dataComparer.CompareResults(result1, result2);

            // Assert
            Assert.False(areEqual);
        }

        [Fact]
        public void CompareResults_ShouldReturnTrue_WhenNullableValuesAreEqual()
        {
            // Arrange
            var result1 = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "Id", 1 },
                    { "Name", "John" },
                    { "MiddleName", null }
                }
            };

            var result2 = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "Id", 1 },
                    { "Name", "John" },
                    { "MiddleName", null } // Nullable value is the same (both null)
                }
            };

            // Act
            var areEqual = _dataComparer.CompareResults(result1, result2);

            // Assert
            Assert.True(areEqual);
        }
    }
}

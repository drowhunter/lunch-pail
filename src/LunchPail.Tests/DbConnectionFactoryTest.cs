using Moq;
using System;
using System.Data;
using Xunit;

namespace LunchPail.Tests
{
  public class DbConnectionFactoryTest
  {
    protected readonly Mock<IDbConnection> connection;
    protected readonly DbConnectionFactory connectionFactory;

    public DbConnectionFactoryTest()
    {
      connection = new Mock<IDbConnection>();
      connectionFactory = new DbConnectionFactory(ConnectionFactoryFn);
    }

    public IDbConnection ConnectionFactoryFn(string connectionName)
    {
      var c = connection.Object;
      c.Open();

      return c;
    }

    public class CreateOpenConnection : DbConnectionFactoryTest
    {
      [Fact]
      public void Should_create_open_connection()
      {
        //Arrange
        connection
          .SetupGet(c => c.State)
          .Returns(ConnectionState.Open);

        //Act
        var conn = connectionFactory.CreateOpenConnection("DefaultConnection");

        //Assert
        Assert.IsAssignableFrom<IDbConnection>(conn);
        Assert.Equal(ConnectionState.Open, conn.State);
      }
    }
  }
}
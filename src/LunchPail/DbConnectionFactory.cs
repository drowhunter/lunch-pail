﻿using System;
using System.Data;

namespace LunchPail
{
public class DbConnectionFactory : IDbConnectionFactory
{
  private readonly Func<string,IDbConnection> connectionFactoryFn;

  /// <summary>
  /// Responsible for instantiating new IDbConnection's
  /// </summary>
  /// <param name="connectionFactory">Should return open IDbConnection instance</param>
  public DbConnectionFactory(Func<string,IDbConnection> connectionFactory)
  {
    this.connectionFactoryFn = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
  }

  public IDbConnection CreateOpenConnection(string connectionName)
  {
    return connectionFactoryFn(connectionName);
  }
}
}
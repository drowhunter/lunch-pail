using System;
using System.Data;

namespace LunchPail
{
	public abstract class DbContext : IDbContext
  {
    private readonly IDbConnectionFactory connectionFactory;
		protected IDbConnection connection;
    private IDbTransaction transaction;
    private IUnitOfWork unitOfWork;

    public DbContext(IDbConnectionFactory connectionFactory)
    {
      this.connectionFactory = connectionFactory;
    }

    public IDbContextState State { get; private set; } = IDbContextState.Closed;

		public abstract IDbConnection Connection { get; }

    public IDbTransaction Transaction =>
      transaction ?? (transaction = Connection.BeginTransaction());

    public IUnitOfWork UnitOfWork =>
      unitOfWork ?? (unitOfWork = new UnitOfWork(Transaction));

    public void Commit()
    {
      try
      {
        UnitOfWork.Commit();
        State = IDbContextState.Comitted;
      }
      catch
      {
        Rollback();
        throw;
      }
      finally
      {
        Reset();
      }
    }

    public void Rollback()
    {
      try
      {
        UnitOfWork.Rollback();
        State = IDbContextState.RolledBack;
      }
      finally
      {
        Reset();
      }
    }

    protected IDbConnection OpenConnection(string connectionName)
    {
      State = IDbContextState.Open;
      return connectionFactory.CreateOpenConnection(connectionName);
    }

    private void Reset()
    {
      Connection?.Close();
      Connection?.Dispose();
      Transaction?.Dispose();

      connection = null;
      transaction = null;
      unitOfWork = null;
    }
  }
}
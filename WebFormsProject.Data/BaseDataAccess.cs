using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace WebFormsProject.Data
{
    /// <summary>
    /// Represents a set of methods to get data from the db and convert into app entities
    /// </summary>
    public abstract class BaseDataAccess
    {
        private ConnectionStringSettings _connectionStringSetting;
        private DbProviderFactory _factory;

        /// <summary>
        /// Gets the forums connection string
        /// </summary>
        protected virtual ConnectionStringSettings ConnectionStringSetting
        {
            get
            {
                if (_connectionStringSetting == null)
                {
                    var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"];
                    connection = EnsureProvider(connection);
                    _connectionStringSetting = connection;
                }
                return _connectionStringSetting;
            }
        }

        /// <summary>
        /// Ensures that the connection strings contains a provider
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        protected ConnectionStringSettings EnsureProvider(ConnectionStringSettings connection)
        {
            if (String.IsNullOrEmpty(connection.ProviderName))
            {
                //Fallback to default provider: sql server
                connection = new ConnectionStringSettings(connection.Name, connection.ConnectionString, "System.Data.SqlClient");
            }
            return connection;
        }

        /// <summary>
        /// The database provider factory to create the connections and commands to access the db.
        /// </summary>
        public virtual DbProviderFactory Factory
        {
            get
            {
                if (_factory == null)
                {
                    _factory = DbProviderFactories.GetFactory(ConnectionStringSetting.ProviderName);
                }
                return _factory;
            }
            set
            {
                _factory = value;
            }
        }

        /// <summary>
        /// Gets a new command for query executing.
        /// </summary>
        /// <param name="procedureName"></param>
        /// <returns></returns>
        protected SqlCommand GetCommand(string query)
        {
            var command = this.Factory.CreateCommand() as SqlCommand;
            command.Connection = GetConnection();
            command.CommandText = query;

            return command;
        }

        /// <summary>
        /// Gets a new connection.
        /// </summary>
        /// <returns></returns>
        public virtual SqlConnection GetConnection()
        {
            var connection = Factory.CreateConnection() as SqlConnection;
            connection.ConnectionString = ConnectionStringSetting.ConnectionString;
            return connection;
        }

        /// <summary>
        /// Gets a datatable filled with the first result of executing the command.
        /// </summary>
        protected DataRow GetFirstRow(DbCommand command)
        {
            DataRow dr = null;
            var dt = GetTable(command);
            if (dt.Rows.Count > 0)
            {
                dr = dt.Rows[0];
            }
            return dr;
        }

        /// <summary>
        /// Gets a datatable filled with the results of executing the command.
        /// </summary>
        protected DataTable GetTable(DbCommand command)
        {
            var dt = new DataTable();
            var da = this.Factory.CreateDataAdapter();
            da.SelectCommand = command;
            da.Fill(dt);

            return dt;
        }

        /// <summary>
        /// Disposes the reader.
        /// </summary>
        /// <param name="reader"></param>
        protected void SafeDispose(DbDataReader reader)
        {
            if (reader != null)
            {
                reader.Dispose();
            }
        }

        /// <summary>
        /// Safely opens the connection, executes and closes the connection
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        protected int SafeExecuteNonQuery(DbCommand command)
        {
            return command.SafeExecuteNonQuery();
        }

        /// <summary>
        /// Safely opens the connection, executes and closes the connection
        /// </summary>
        /// <param name="command"></param>
        /// <returns>The last inserted element id.</returns>
        protected int SafeExecuteScalar(DbCommand command)
        {
            return command.SafeExecuteScalar();
        }
    }
}

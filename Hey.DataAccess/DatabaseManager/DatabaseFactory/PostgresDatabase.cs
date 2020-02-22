using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;
using Hey.Common;

namespace Hey.DataAccess.DatabaseManager.DatabaseFactory
{
    public class PostgresDatabase : IDatabase
    {
        private static PostgresDatabase pgInstance;
        public static string CONNECTION_STRING = System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString;
        //public static string CONNECTION_STRING = Hey.Common.Utils.Functions.GetValueFromWebConfig("connectionstring");

        //public static string CONNECTION_STRING = @"userid=postgres; password=postgres; database=Hey-Ermelo; server=localhost; port=5432; Timeout=60; encoding=unicode;";
        public PostgresDatabase()
        {
        }

        public string ConnectionString
        {
            get { return CONNECTION_STRING; }
        }

        public static PostgresDatabase GetPGDatabase()
        {
            if (pgInstance == null)
            {
                pgInstance = new PostgresDatabase();
            }
            return pgInstance;
        }

        #region IDatabase Members

        public DataTable GetDataTable(string query)
        {
            using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
            {
                using (NpgsqlCommand command = new NpgsqlCommand(query, sqlConnection))
                {
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        try
                        {
                            sqlConnection.Open();
                            adapter.Fill(dt);
                        }
                        catch (NpgsqlException ex)
                        {
                            Hey.Common.Utils.LogWriter.Log(ex.Message + ".  Query:" + ex.ErrorSql);
                            throw new Exception(ex.Message);
                        }
                        return dt;
                    }
                }
            }
        }

        public DataTable GetDataTable(NpgsqlCommand command)
        {
            using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
            {
                command.Connection = sqlConnection;
                using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                {
                    DataTable dt = new DataTable();
                    try
                    {
                        sqlConnection.Open();
                        adapter.Fill(dt);
                    }
                    catch (NpgsqlException ex)
                    {
                        Hey.Common.Utils.LogWriter.Log(ex.Message + ".  Query:" + ex.ErrorSql);
                        throw new Exception(ex.Message);
                    }
                    return dt;
                }
            }
        }

        public void ExecuteNonQuery(NpgsqlCommand command)
        {
            using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
            {
                try
                {
                    command.Connection = sqlConnection;
                    sqlConnection.Open();
                    command.ExecuteNonQuery();
                }
                catch (NpgsqlException ex)
                {
                    Hey.Common.Utils.LogWriter.Log(ex.Message + ".Update  Query:" + ex.ErrorSql);
                    throw new Exception(ex.Message);
                }
            }
        }

        public DataSet GetDataTables(NpgsqlCommand[] commands, string[] tableName)
        {
            using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
            {
                DataSet ds = new DataSet();
                for (int i = 0; i < commands.Length; i++)
                {
                    commands[i].Connection = sqlConnection;
                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(commands[i]))
                    {
                        try
                        {
                            sqlConnection.Open();
                            adapter.Fill(ds, tableName[i]);
                        }
                        catch (NpgsqlException ex)
                        {
                            Hey.Common.Utils.LogWriter.Log(ex.Message + ".  Query:" + ex.ErrorSql);
                            throw new Exception(ex.Message);
                        }
                    }
                }
                return ds;
            }
        }
        
        public int ExecuteQuery(string query)
        {
            int result = 0;
            using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
            {
                using (NpgsqlCommand command = new NpgsqlCommand(query, sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        result = command.ExecuteNonQuery();
                    }
                    catch (NpgsqlException ex)
                    {
                        Hey.Common.Utils.LogWriter.Log(ex.Message + ".  Query:" + ex.ErrorSql);
                        throw new Exception(ex.Message);
                    }
                    return result;
                }
            }
        }

        public int ExecuteQuery(NpgsqlCommand command)
        {
            int result = 0;
            using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
            {
                command.Connection = sqlConnection;
                try
                {
                    sqlConnection.Open();
                    result = command.ExecuteNonQuery();
                }
                catch (NpgsqlException ex)
                {
                    string values = "";
                    foreach (NpgsqlParameter param in command.Parameters)
                    {
                        values += param.Value + ",";
                    }
                    Hey.Common.Utils.LogWriter.Log(ex.Message + ". Query:" + ex.ErrorSql);
                    throw new Exception(ex.Code);
                }
                return result;
            }
        }

        #endregion

    }

    public class PGConnection
    {
        private static NpgsqlConnection instance;
        public static NpgsqlConnection GetConnectionInstance(string con)
        {
            if (instance == null)
            {
                instance = new NpgsqlConnection(con);
            }
            return instance;
        }
    }
}

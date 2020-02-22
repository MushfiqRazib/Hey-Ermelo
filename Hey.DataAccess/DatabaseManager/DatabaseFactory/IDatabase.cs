using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;

namespace Hey.DataAccess.DatabaseManager.DatabaseFactory
{
    public interface IDatabase
    {       
        DataTable GetDataTable(string query);
        DataTable GetDataTable(NpgsqlCommand command);
        int ExecuteQuery(string query);
        int ExecuteQuery(NpgsqlCommand command);
        void ExecuteNonQuery(NpgsqlCommand command);        
    }
}

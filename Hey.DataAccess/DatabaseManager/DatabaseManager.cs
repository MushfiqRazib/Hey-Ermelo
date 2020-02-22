using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hey.DataAccess.DatabaseManager.DatabaseFactory;
using System.Data;
namespace Hey.DataAccess.DatabaseManager
{
    public class DatabaseManager
    {
        public string ACTIVE_DATABASE = System.Configuration.ConfigurationManager.AppSettings["activeDB"];
        public DatabaseManager()
        {
        }

        public DataTable GetMenuItems()
        {
            DataTable dt = new DataTable();
            IDatabaseFactory dbFactory = new DatabaseFactory.DatabaseFactory();
            IDatabase dbObject = dbFactory.CreateDatabaseInstance(ACTIVE_DATABASE);            
            dt = dbObject.GetDataTable(SQLStatements.SQLClass.GET_GROUPS);
            return dt;
        }

    }
}

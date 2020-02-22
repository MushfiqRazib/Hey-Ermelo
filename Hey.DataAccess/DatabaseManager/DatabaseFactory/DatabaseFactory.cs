using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hey.DataAccess.DatabaseManager.DatabaseFactory
{
    public class DatabaseFactory : IDatabaseFactory
    {
        #region IDatabaseFactory Members

        public IDatabase CreateDatabaseInstance(string activeDb)
        {
            try
            {
                switch (activeDb)
                {
                    case "postgres":
                        return PostgresDatabase.GetPGDatabase();
                    //case "oracle":
                    //    return new OracleDatabase();
                    //case "mssql":
                    //    return new MSSqlDatabase();
                }
                return null;
            }
            catch { throw new NotImplementedException(); }
        }

        #endregion
    }
}


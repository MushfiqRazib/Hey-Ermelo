using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hey.DataAccess.DatabaseManager.DatabaseFactory
{
    public class OracleDatabase : IDatabase
    {

        #region IDatabase Members

        public void GetConnection(string connection)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable GetDataTable(string query)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

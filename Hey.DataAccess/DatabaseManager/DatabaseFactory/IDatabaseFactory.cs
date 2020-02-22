using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Hey.DataAccess.DatabaseManager.DatabaseFactory
{
    public interface IDatabaseFactory
    {
        IDatabase CreateDatabaseInstance(string activeDb);
    }
}

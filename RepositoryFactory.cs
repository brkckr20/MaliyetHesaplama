using Microsoft.Data.Sqlite;
using System.Data;

namespace MaliyeHesaplama
{
    public static class RepositoryFactory
    {
        public static MiniOrm Create()
        {
            var config = DbConfig.Load();
            IDbConnection conn;
            if (config.DbType == "SQLite")
            {
                conn = new SqliteConnection(config.ConnectionString);
            }
            else if (config.DbType == "MSSQL")
            {
                conn = new SqliteConnection(config.ConnectionString);
            }
            else
                throw new Exception("Desteklenmeyen veritabanı tipi: " + config.DbType);
            return new MiniOrm();
        }
    }
}

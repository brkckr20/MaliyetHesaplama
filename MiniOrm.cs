using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Windows;
using Microsoft.Data.SqlClient;
using MaliyeHesaplama.helpers;

public class MiniOrm
{
    private readonly IDbConnection _connection;
    private readonly string _dbType;

    public MiniOrm()
    {
        var config = DbConfig.Load();
        _dbType = config.DbType;
        if (_dbType == "MSSQL")
            _connection = new SqlConnection(config.ConnectionString);
        else if (_dbType == "SQLite")
            _connection = new SqliteConnection(config.ConnectionString);
        else
            throw new NotSupportedException($"{_dbType} desteklenmiyor.");
        _connection.Open();
    }

    public int Save(string tableName, Dictionary<string, object> data, string idColumn = "Id")
    {
        if (!data.ContainsKey(idColumn))
            throw new ArgumentException($"{idColumn} dictionary içinde olmalı.");

        var id = Convert.ToInt32(data[idColumn]);

        if (id == 0)
        {
            var insertColumns = data.Keys.Where(k => k != idColumn).ToList();
            var insertValues = insertColumns.Select(k => $"@{k}");
            string sql = $"INSERT INTO {tableName} ({string.Join(",", insertColumns)}) VALUES ({string.Join(",", insertValues)});";

            if (_dbType == "MSSQL")
                sql += " SELECT CAST(SCOPE_IDENTITY() as int);";
            else if (_dbType == "SQLite")
                sql += " SELECT last_insert_rowid();";

            return _connection.Query<int>(sql, data).Single();
        }
        else
        {
            var updateColumns = data.Keys.Where(k => k != idColumn).Select(k => $"{k}=@{k}");
            var sql = $"UPDATE {tableName} SET {string.Join(",", updateColumns)} WHERE {idColumn}=@{idColumn}; SELECT @{idColumn};";
            return _connection.Query<int>(sql, data).Single();
        }
    }

    public int Delete(string tableName, int id, bool isConfirmed, string Condition = "Id")
    {
        if (id == 0)
        {
            Bildirim.Uyari2("Kayıt silme işlemini gerçekleştirebilmek için lütfen bir kayıt seçiniz!!");
            return 0;
        }
        var sql = $"DELETE FROM {tableName} WHERE {Condition}=@Id;";
        if (isConfirmed)
        {
            if (Bildirim.SilmeOnayi2())
            {
                return _connection.Execute(sql, new { Id = id });
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return _connection.Execute(sql, new { Id = id });
        }
    }

    public T GetById<T>(string tableName, int id, string fieldName = "Id")
    {
        var sql = $"SELECT * FROM {tableName} WHERE {fieldName}={id};";
        return _connection.Query<T>(sql).FirstOrDefault();
    }

    public IEnumerable<T> GetAll<T>(string tableName)
    {
        var sql = $"SELECT * FROM {tableName};";
        return _connection.Query<T>(sql);
    }
    public T GetBeforeRecord<T>(string tableName, int Id)
    {
        var sql = $"SELECT TOP 1 * FROM {tableName} where Id < {Id} order by Id desc";
        return _connection.Query<T>(sql).FirstOrDefault();
    }
    public T GetNextRecord<T>(string tableName, int Id)
    {
        var sql = $"SELECT TOP 1 * FROM {tableName} where Id > {Id} order by Id asc";
        return _connection.Query<T>(sql).FirstOrDefault();
    }
    public string GetLastCompanyCode()
    {
        var sql = "Select Top 1 CompanyCode from Company order by Id Desc";
        return _connection.QueryFirstOrDefault<string>(sql);
    }
    public string GetEURCurrency()
    {
        var kur = GetById<dynamic>("ProductionManagementParams", 1).BazAlinacakKur;
        var sql = $"select top 1 {kur} from Currency order by TARIH desc";
        return _connection.QueryFirstOrDefault<string>(sql);
    }
    public string GetInventoryCodeByCombinedCode(string code)
    {
        var sql = "SELECT InventoryCode FROM Inventory WHERE CombinedCode = @Code";
        return _connection.QueryFirstOrDefault<string>(sql, new { Code = code });
    }
    public string GetRecordNo(string tableName, string fieldName, string typeName, int typeNo)
    {
        var sql = $"select top 1 {fieldName} from {tableName} where {typeName} = {typeNo} order by {fieldName} desc";
        var result = _connection.QueryFirstOrDefault<string>(sql);
        if (result != null)
        {
            if (int.TryParse(result, out int currentValue))
            {
                currentValue++;
                return currentValue.ToString("D8");
            }
        }
        return "00000001";
    }
    public IEnumerable<T> GetCostList<T>()
    {
        var sql = @"select 
                    ISNULL(C.Id,'') [Id]
                    ,ISNULL(C.Date,'') [Date]
                    ,ISNULL(C.OrderNo,'') [OrderNo]
                    ,ISNULL(CO.Id,'') [CompanyId]
                    ,ISNULL(CO.CompanyCode,'') [CompanyCode]
                    ,ISNULL(CO.CompanyName,'') [CompanyName]
                    ,ISNULL(I.InventoryName,'') [InventoryName]
                    ,ISNULL(I.InventoryCode,'') [InventoryCode]
                    ,ISNULL(I.Id,'') [InventoryId]
					,ISNULL(CONVERT(varchar(max), C.ProductImage, 1), '') AS [ProductImage] 
                    from Cost C 
                    left join Company CO on C.CompanyId = CO.Id
                    left join Inventory I on I.Id = C.InventoryId
                    order by ISNULL(C.OrderNo,'')";
        return _connection.Query<T>(sql);
    }
    public byte[] GetImage(string tableName, string fieldName, int id)
    {
        var sql = $"SELECT {fieldName} FROM {tableName} WHERE Id = @Id";
        var imageBytes = _connection.QueryFirstOrDefault<byte[]>(sql, new { Id = id });
        return imageBytes;
    }
    public T GetReport<T>(string reportName)
    {
        var sql = $"SELECT * FROM Report WHERE ReportName={reportName};";
        return _connection.Query<T>(sql).FirstOrDefault();
    }
}

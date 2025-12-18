using Dapper;
using MaliyeHesaplama.helpers;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System.Data;

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
    public T GetBeforeRecord<T>(string tableName, int Id, string filter = "")
    {
        string where = $"Id < {Id}";
        if (!string.IsNullOrWhiteSpace(filter))
            where += $" AND {filter}";

        var sql = $"SELECT TOP 1 * FROM {tableName} WHERE {where} ORDER BY Id DESC";
        return _connection.Query<T>(sql).FirstOrDefault();
    }

    public T GetNextRecord<T>(string tableName, int Id, string filter = "")
    {
        string where = $"Id > {Id}";
        if (!string.IsNullOrWhiteSpace(filter))
            where += $" AND {filter}";

        var sql = $"SELECT TOP 1 * FROM {tableName} WHERE {where} ORDER BY Id ASC";
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
                    ISNULL(C.Id,0) [Id]
                    ,ISNULL(C.Date,'') [Date]
                    ,ISNULL(C.OrderNo,'') [OrderNo]
                    ,ISNULL(CO.Id,'') [CompanyId]
                    ,ISNULL(CO.CompanyCode,'') [CompanyCode]
                    ,ISNULL(CO.CompanyName,'') [CompanyName]
                    ,ISNULL(I.InventoryName,'') [InventoryName]
                    ,ISNULL(I.InventoryCode,'') [InventoryCode]
                    ,ISNULL(I.Id,'') [InventoryId]
					--,ISNULL(CONVERT(varchar(max), C.ProductImage, 1), '') AS [ProductImage] 
                    from Cost C 
                    left join Company CO on C.CompanyId = CO.Id
                    left join Inventory I on I.Id = C.InventoryId
                    order by ISNULL(C.OrderNo,'')";
        return _connection.Query<T>(sql);
    }

    public IEnumerable<T> GetMovementList<T>(int receiptType)
    {
        var sql = $@"
                    select 
                        ISNULL(R.Id,0) [Id],
	                    ISNULL(R.ReceiptNo,'') [ReceiptNo],
	                    ISNULL(R.ReceiptDate,'') [ReceiptDate],	
	                    ISNULL(C.Id,'') [CompanyId],
	                    ISNULL(C.CompanyName,'') [CompanyName],
	                    ISNULL(R.Authorized,'') [Authorized],
	                    ISNULL(R.DuaDate,'') [DuaDate],
	                    ISNULL(R.Maturity,'') [Maturity]
                    --	,
                    --*
                    from Receipt R
                    left join Company C with(nolock) on C.Id = R.CompanyId
                    where R.ReceiptType = {receiptType}
";
        return _connection.Query<T>(sql);
    }
    public IEnumerable<T> GetMovementList<T>(int DepoId, int receiptType)
    {
        var sql = $@"
                            select 
                            ISNULL(R.Id,0) [Id],
                            ISNULL(R.ReceiptNo,'') [ReceiptNo],
                            ISNULL(R.ReceiptDate,'') [ReceiptDate],	
                            ISNULL(C.Id,'') [CompanyId],
                            ISNULL(C.CompanyName,'') [CompanyName],
                            ISNULL(R.Authorized,'') [Authorized],
                            ISNULL(R.DuaDate,'') [DuaDate],
                            ISNULL(R.Maturity,'') [Maturity],
                            ISNULL(R.CustomerOrderNo,'') [CustomerOrderNo],
                            ISNULL(R.Explanation,'') [Explanation],
                            ISNULL(RI.Id,'') [ReceiptItemId],
                            ISNULL(RI.OperationType,'') [OperationType],
                            ISNULL(RI.InventoryId,'') [InventoryId],
                            ISNULL(I.InventoryCode,'') [InventoryCode],
                            ISNULL(I.InventoryName,'') [InventoryName],
                            ISNULL(RI.NetMeter,0) [NetMeter],
                            ISNULL(RI.CashPayment,0) [CashPayment],
                            ISNULL(RI.DeferredPayment,0) [DeferredPayment],
                            ISNULL(RI.Forex,'') [Forex],
                            ISNULL(RI.VariantId,'') [VariantId],
                            ISNULL(CO.Code,'') [VariantCode],
                            ISNULL(CO.Name,'') [Variant],
                            ISNULL(RI.RowExplanation,'') [RowExplanation]
                        from Receipt R
                        left join Company C with(nolock) on C.Id = R.CompanyId
                        left join ReceiptItem RI with(nolock) on RI.ReceiptId = R.Id
                        left join Inventory I with(nolock) on I.Id = RI.InventoryId
                        LEFT JOIN Color CO with(nolock) on CO.Id = RI.VariantId
                    where R.ReceiptType = {receiptType} and R.WareHouseId = {DepoId}
";
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
        var sql = $"SELECT * FROM Report WHERE ReportName='{reportName}';";
        return _connection.Query<T>(sql).FirstOrDefault();
    }
    public List<T> GetReportsToUserControl<T>(string formName)
    {
        var sql = $"SELECT * FROM Report WHERE FormName='{formName}';";
        return _connection.Query<T>(sql).ToList();
    }
    public List<dynamic> GetAfterOrBeforeRecord(string query, int id)
    {
        return _connection.Query(query, new { Id = id }).ToList();
    }
    public int? GetIdForAfterOrBeforeRecord(string KayitTipi, string TableName, int id, string TableName2, string TableName2Ref, int ReceiptType)
    {
        string sql = KayitTipi == "Önceki"
            ? $"SELECT MAX(t1.Id) FROM {TableName} t1 inner join {TableName2} t2 on t1.Id=t2.{TableName2Ref} WHERE t1.Id < @Id and ReceiptType = {ReceiptType}"
            : $"SELECT MIN(t1.Id) FROM {TableName} t1 inner join {TableName2} t2 on t1.Id=t2.{TableName2Ref} WHERE t1.Id > @Id and ReceiptType = {ReceiptType}";

        return _connection.QueryFirstOrDefault<int?>(sql, new { Id = id });
    }
    public void LoadCurrenciesFromDbToCombobox(System.Windows.Controls.ComboBox cmbDoviz)
    { // normal combobox için
        var data = GetById<dynamic>("ProductionManagementParams", 1);
        string list = data.DovizKurlari;
        cmbDoviz.Items.Clear();
        foreach (var item in list.Split(','))
        {
            cmbDoviz.Items.Add(item.Trim());
        }
    }
    public IEnumerable<T> GetColorList<T>(bool isParent)
    {
        string parent = "0";
        if (isParent)
        {
            parent = "1";
        }
        string sql;
        if (true)
        {
            sql = $@"Select 
                    ISNULL(C.Id,0) [Id],
                    ISNULL(C.Type,0) [Type],
                    case 
                    when ISNULL(C.Type,0) = 1 then 'Kumaş'
                    when ISNULL(C.Type,0) = 2 then 'İplik'
                    end as [TypeName],
                    ISNULL(C.Code,'') [Code],
                    ISNULL(C.Name,'') [Name],
                    ISNULL(C.Date,'') [Date],
                    ISNULL(C.CompanyId,0) [CompanyId],
                    ISNULL(C.ParentId,0) [ParentId],
                    ISNULL(C.RequestDate,'') [RequestDate],
                    ISNULL(C.ConfirmDate,'') [ConfirmDate],
                    ISNULL(C.PantoneNo,'') [PantoneNo],
                    ISNULL(C.Price,0) [Price],
                    ISNULL(C.Forex,'') [Forex],
                    ISNULL(C.IsUse,0) [IsUse],
                    ISNULL(C.Explanation,0) [Explanation] from Color C
                    left join Company CO on C.CompanyId = CO.Id
                    where C.IsParent = '{parent}'";
        }
        return _connection.Query<T>(sql);
    }
    public int IfExistRecord(string tableName, string columnName, string columnValue,string Type)
    {
        string checkQuery = $"SELECT COUNT(1) FROM {tableName} WHERE {columnName} = @Value and Type = '{Type}'" ;
        return _connection.ExecuteScalar<int>(checkQuery, new { Value = columnValue });
    }

}

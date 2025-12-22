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
    public IEnumerable<T> GetMovementList<T>(string contition)
    {
        var sql = $@"    select 
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
                            ISNULL(RI.NetWeight,0) [NetWeight],
                            ISNULL(RI.Piece,0) [Piece],
                            ISNULL(RI.CashPayment,0) [CashPayment],
                            ISNULL(RI.DeferredPayment,0) [DeferredPayment],
                            ISNULL(RI.Forex,'') [Forex],
                            ISNULL(RI.VariantId,'') [VariantId],
                            ISNULL(CO.Code,'') [VariantCode],
                            ISNULL(CO.Name,'') [Variant],
                            ISNULL(RI.RowExplanation,'') [RowExplanation],	
                            ISNULL(W.Id,'') [WareHouseId],
                            ISNULL(W.Code,'') [WareHouseCode],
                            ISNULL(W.Name,'') [WareHouseName],
                            ISNULL(RI.CustomerOrderNo,'') [CustomerOrderNo],
                            ISNULL(RI.OrderNo,'') [OrderNo]
                        from Receipt R
                        left join Company C with(nolock) on C.Id = R.CompanyId
                        left join ReceiptItem RI with(nolock) on RI.ReceiptId = R.Id
                        left join Inventory I with(nolock) on I.Id = RI.InventoryId
                        LEFT JOIN Color CO with(nolock) on CO.Id = RI.VariantId
                        left join WareHouse W on R.WareHouseId = W.Id
                        where {contition}";
        return _connection.Query<T>(sql);
    }
    public IEnumerable<T> GetMovementListWithQuantities<T>(string contition)
    {
        var sql = $@"	WITH UsedNetMeter AS
	(
		SELECT
			RI1.TrackingNumber,
			SUM(ISNULL(RI1.NetMeter, 0)) AS UsedMeter
		FROM ReceiptItem RI1
		GROUP BY RI1.TrackingNumber
	)
	SELECT 
		R.Id                                   AS Id,
		ISNULL(R.ReceiptNo,'')                AS ReceiptNo,
		R.ReceiptDate                         AS ReceiptDate,
		C.Id                                  AS CompanyId,
		ISNULL(C.CompanyName,'')              AS CompanyName,
		ISNULL(R.Authorized,'')               AS Authorized,
		R.DuaDate                             AS DuaDate,
		R.Maturity                            AS Maturity,
		ISNULL(R.CustomerOrderNo,'')          AS CustomerOrderNo,
		ISNULL(R.Explanation,'')              AS Explanation,

		RI.Id                                 AS ReceiptItemId,
		ISNULL(RI.OperationType,'')           AS OperationType,
		RI.InventoryId                        AS InventoryId,
		ISNULL(I.InventoryCode,'')            AS InventoryCode,
		ISNULL(I.InventoryName,'')            AS InventoryName,

		SUM(ISNULL(RI.NetMeter,0)) 
			- ISNULL(UNM.UsedMeter,0)          AS NetMeter,

		ISNULL(RI.NetWeight,0)                AS NetWeight,
		ISNULL(RI.Piece,0)                    AS Piece,
		ISNULL(RI.CashPayment,0)              AS CashPayment,
		ISNULL(RI.DeferredPayment,0)          AS DeferredPayment,
		ISNULL(RI.Forex,'')                   AS Forex,

		RI.VariantId                          AS VariantId,
		ISNULL(CO.Code,'')                    AS VariantCode,
		ISNULL(CO.Name,'')                    AS Variant,

		ISNULL(RI.RowExplanation,'')          AS RowExplanation,
		ISNULL(RI.TrackingNumber,'')          AS TrackingNumber

	FROM Receipt R
	LEFT JOIN Company C        WITH (NOLOCK) ON C.Id = R.CompanyId
	LEFT JOIN ReceiptItem RI   WITH (NOLOCK) ON RI.ReceiptId = R.Id
	LEFT JOIN Inventory I      WITH (NOLOCK) ON I.Id = RI.InventoryId
	LEFT JOIN Color CO         WITH (NOLOCK) ON CO.Id = RI.VariantId
	LEFT JOIN UsedNetMeter UNM                ON UNM.TrackingNumber = RI.Id

	WHERE {contition}

	GROUP BY
		R.Id,
		R.ReceiptNo,
		R.ReceiptDate,
		C.Id,
		C.CompanyName,
		R.Authorized,
		R.DuaDate,
		R.Maturity,
		R.CustomerOrderNo,
		R.Explanation,
		RI.Id,
		RI.OperationType,
		RI.InventoryId,
		I.InventoryCode,
		I.InventoryName,
		RI.NetWeight,
		RI.Piece,
		RI.CashPayment,
		RI.DeferredPayment,
		RI.Forex,
		RI.VariantId,
		CO.Code,
		CO.Name,
		RI.RowExplanation,
		RI.TrackingNumber,
		UNM.UsedMeter
        having 
		        SUM(ISNULL(RI.NetMeter,0)) 
			        - ISNULL(UNM.UsedMeter,0) <>0
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
    public int IfExistRecord(string tableName, string columnName, string columnValue, string Type)
    {
        string checkQuery = $"SELECT COUNT(1) FROM {tableName} WHERE {columnName} = @Value and Type = '{Type}'";
        return _connection.ExecuteScalar<int>(checkQuery, new { Value = columnValue });
    }


    /*-----------------------------------------------------------*/
    public int UpdateStockQuantity(int inventoryId, int wareHouseId,
        decimal deltaKg, decimal deltaMeter, int deltaPiece,
        int? variantId = null, string batchNo = null, string orderNo = null, IDbTransaction tx = null)
    {
        var p = new
        {
            InventoryId = inventoryId,
            WareHouseId = wareHouseId,
            VariantId = variantId,
            BatchNo = batchNo,
            OrderNo = orderNo,
            DeltaKg = deltaKg,
            DeltaMeter = deltaMeter,
            DeltaPiece = deltaPiece
        };

        var updateSql = @"
                        UPDATE Stock SET 
                            QuantityKg = ISNULL(QuantityKg,0) + @DeltaKg,
                            QuantityMeter = ISNULL(QuantityMeter,0) + @DeltaMeter,
                            QuantityPiece = ISNULL(QuantityPiece,0) + @DeltaPiece
                        WHERE InventoryId = @InventoryId AND WareHouseId = @WareHouseId
                          AND (VariantId = @VariantId OR (VariantId IS NULL AND @VariantId IS NULL))
                          AND (BatchNo = @BatchNo OR (BatchNo IS NULL AND @BatchNo IS NULL))
                          AND (OrderNo = @OrderNo OR (OrderNo IS NULL AND @OrderNo IS NULL));";

        int affected = tx == null ? _connection.Execute(updateSql, p) : _connection.Execute(updateSql, p, tx);

        if (affected == 0)
        {
            var insertSql = @"
                            INSERT INTO Stock (InventoryId, WareHouseId, VariantId, BatchNo, OrderNo, QuantityKg, QuantityMeter, QuantityPiece)
                            VALUES (@InventoryId, @WareHouseId, @VariantId, @BatchNo, @OrderNo, @DeltaKg, @DeltaMeter, @DeltaPiece);";
            if (_dbType == "MSSQL") insertSql += " SELECT CAST(SCOPE_IDENTITY() as int);";
            else if (_dbType == "SQLite") insertSql += " SELECT last_insert_rowid();";

            if (tx == null)
                return _connection.Query<int>(insertSql, p).Single();
            else
                return _connection.Query<int>(insertSql, p, tx).Single();
        }

        return affected;
    }
    public void InsertStockMovement(int? stockId, int receiptId, int receiptItemId, int inventoryId, int wareHouseId,
        int? variantId, string batchNo, string orderNo,
        decimal deltaKg, decimal deltaMeter, int deltaPiece,
        decimal beforeKg, decimal beforeMeter, int beforePiece,
        int? userId = null, IDbTransaction tx = null)
    {
        var mv = new
        {
            StockId = stockId,
            ReceiptId = receiptId,
            ReceiptItemId = receiptItemId,
            InventoryId = inventoryId,
            WareHouseId = wareHouseId,
            VariantId = variantId,
            BatchNo = batchNo,
            OrderNo = orderNo,
            DeltaKg = deltaKg,
            DeltaMeter = deltaMeter,
            DeltaPiece = deltaPiece,
            BeforeKg = beforeKg,
            AfterKg = beforeKg + deltaKg,
            BeforeMeter = beforeMeter,
            AfterMeter = beforeMeter + deltaMeter,
            BeforePiece = beforePiece,
            AfterPiece = beforePiece + deltaPiece,
            UserId = userId,
            ReceiptNo = "" // optional, eklenmek istenirse parametre alabilirS
        };

        var insertMv = @"
INSERT INTO StockMovement (StockId, ReceiptId, ReceiptItemId, InventoryId, WareHouseId, VariantId, BatchNo, OrderNo,
    DeltaKg, DeltaMeter, DeltaPiece, BeforeKg, AfterKg, BeforeMeter, AfterMeter, BeforePiece, AfterPiece, UserId, CreatedAt)
VALUES (@StockId, @ReceiptId, @ReceiptItemId, @InventoryId, @WareHouseId, @VariantId, @BatchNo, @OrderNo,
    @DeltaKg, @DeltaMeter, @DeltaPiece, @BeforeKg, @AfterKg, @BeforeMeter, @AfterMeter, @BeforePiece, @AfterPiece, @UserId, GETDATE());";

        if (tx == null)
            _connection.Execute(insertMv, mv);
        else
            _connection.Execute(insertMv, mv, tx);
    }
    public int SaveReceiptWithStock(Dictionary<string, object> receiptDict, List<Dictionary<string, object>> items, int wareHouseId, int? userId = null, string tblName1 = null, string tblName2 = null)
    {
        using var tran = _connection.BeginTransaction();
        try
        {
            // 1) Receipt save (insert/update) within transaction
            if (!receiptDict.ContainsKey("Id"))
                throw new ArgumentException("Fiş bir Id anahtarı içermelidir.");

            int receiptId = Convert.ToInt32(receiptDict["Id"]);
            if (receiptId == 0)
            {
                var insertCols = receiptDict.Keys.Where(k => k != "Id").ToList();
                var insertVals = insertCols.Select(k => "@" + k);
                var insertSql = $"INSERT INTO {tblName1} ({string.Join(",", insertCols)}) VALUES ({string.Join(",", insertVals)});";
                if (_dbType == "MSSQL") insertSql += " SELECT CAST(SCOPE_IDENTITY() as int);";
                else if (_dbType == "SQLite") insertSql += " SELECT last_insert_rowid();";

                receiptId = _connection.Query<int>(insertSql, receiptDict, tran).Single();
            }
            else
            {
                var updateCols = receiptDict.Keys.Where(k => k != "Id").Select(k => $"{k}=@{k}");
                var updateSql = $"UPDATE {tblName1} SET {string.Join(",", updateCols)} WHERE Id=@Id; SELECT @Id;";
                receiptId = _connection.Query<int>(updateSql, receiptDict, tran).Single();
            }
            foreach (var item in items)
            {
                if (!item.ContainsKey("Id"))
                    throw new ArgumentException("Kalemler bir Id anahtarı içermelidir.");

                int itemId = Convert.ToInt32(item["Id"]);
                dynamic prev = null;
                if (itemId != 0)
                {
                    prev = _connection.QueryFirstOrDefault<dynamic>($"SELECT Id, InventoryId, NetMeter, NetWeight, Piece, VariantId, BatchNo, OrderNo FROM {tblName2} WHERE Id = @Id", new { Id = itemId }, tran);
                }

                // ensure ReceiptId
                item["ReceiptId"] = receiptId;

                if (itemId == 0)
                {
                    var insertCols = item.Keys.Where(k => k != "Id").ToList();
                    var insertVals = insertCols.Select(k => "@" + k);
                    var insertSql = $"INSERT INTO {tblName2} ({string.Join(",", insertCols)}) VALUES ({string.Join(",", insertVals)});";
                    if (_dbType == "MSSQL") insertSql += " SELECT CAST(SCOPE_IDENTITY() as int);";
                    else if (_dbType == "SQLite") insertSql += " SELECT last_insert_rowid();";

                    int newItemId = _connection.Query<int>(insertSql, item, tran).Single();
                    item["Id"] = newItemId;
                    itemId = newItemId;
                }
                else
                {
                    var updateCols = item.Keys.Where(k => k != "Id").Select(k => $"{k}=@{k}");
                    var updateSql = $"UPDATE {tblName2} SET {string.Join(",", updateCols)} WHERE Id=@Id; SELECT @Id;";
                    _connection.Query<int>(updateSql, item, tran).Single();
                }

                // compute deltas
                decimal newMeter = item.ContainsKey("NetMeter") && item["NetMeter"] != null ? Convert.ToDecimal(item["NetMeter"]) : 0m;
                decimal newWeight = item.ContainsKey("NetWeight") && item["NetWeight"] != null ? Convert.ToDecimal(item["NetWeight"]) : 0m;
                int newPiece = item.ContainsKey("Piece") && item["Piece"] != null ? Convert.ToInt32(item["Piece"]) : 0;

                decimal oldMeter = 0m; decimal oldWeight = 0m; int oldPiece = 0;
                if (prev != null)
                {
                    oldMeter = prev.NetMeter != null ? Convert.ToDecimal(prev.NetMeter) : 0m;
                    oldWeight = prev.NetWeight != null ? Convert.ToDecimal(prev.NetWeight) : 0m;
                    oldPiece = prev.Piece != null ? Convert.ToInt32(prev.Piece) : 0;
                }

                var deltaMeter = newMeter - oldMeter;
                var deltaWeight = newWeight - oldWeight;
                var deltaPiece = newPiece - oldPiece;

                int invId = item.ContainsKey("InventoryId") && item["InventoryId"] != null ? Convert.ToInt32(item["InventoryId"]) : 0;
                int? variantId = item.ContainsKey("VariantId") && item["VariantId"] != null ? (int?)Convert.ToInt32(item["VariantId"]) : null;
                string batchNo = item.ContainsKey("BatchNo") ? (item["BatchNo"]?.ToString()) : null;
                string orderNo = item.ContainsKey("OrderNo") ? (item["OrderNo"]?.ToString()) : null;

                if (invId != 0 && (deltaMeter != 0m || deltaWeight != 0m || deltaPiece != 0))
                {
                    // before values read
                    var selSql = @"
                                SELECT ISNULL(QuantityKg,0) AS QuantityKg, ISNULL(QuantityMeter,0) AS QuantityMeter, ISNULL(QuantityPiece,0) AS QuantityPiece, Id
                                FROM Stock
                                WHERE InventoryId = @InventoryId AND WareHouseId = @WareHouseId
                                  AND (VariantId = @VariantId OR (VariantId IS NULL AND @VariantId IS NULL))
                                  AND (BatchNo = @BatchNo OR (BatchNo IS NULL AND @BatchNo IS NULL))
                                  AND (OrderNo = @OrderNo OR (OrderNo IS NULL AND @OrderNo IS NULL));";

                    var before = _connection.QueryFirstOrDefault<dynamic>(selSql, new { InventoryId = invId, WareHouseId = wareHouseId, VariantId = variantId, BatchNo = batchNo, OrderNo = orderNo }, tran);
                    decimal beforeKg = 0m; decimal beforeMeter = 0m; int beforePiece = 0; int? stockId = null;
                    if (before != null)
                    {
                        beforeKg = Convert.ToDecimal(before.QuantityKg);
                        beforeMeter = Convert.ToDecimal(before.QuantityMeter);
                        beforePiece = Convert.ToInt32(before.QuantityPiece);
                        stockId = Convert.ToInt32(before.Id);
                    }

                    // apply to stock (update or insert)
                    UpdateStockQuantity(invId, wareHouseId, deltaWeight, deltaMeter, deltaPiece, variantId, batchNo, orderNo, tran);

                    // insert movement log
                    InsertStockMovement(stockId, receiptId, itemId, invId, wareHouseId, variantId, batchNo, orderNo,
                        deltaWeight, deltaMeter, deltaPiece,
                        beforeKg, beforeMeter, beforePiece,
                        userId, tran);
                }
            }

            tran.Commit();
            return receiptId;
        }
        catch
        {
            tran.Rollback();
            throw;
        }
    }

}

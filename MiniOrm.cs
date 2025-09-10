﻿using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Windows;
using Microsoft.Data.SqlClient;

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

    public int Delete(string tableName, int id, string idColumn = "Id")
    {
        var sql = $"DELETE FROM {tableName} WHERE {idColumn}=@Id;";
        return _connection.Execute(sql, new { Id = id });
    }

    public T GetById<T>(string tableName, int id, string idColumn = "Id")
    {
        var sql = $"SELECT * FROM {tableName} WHERE {idColumn}=@Id;";
        return _connection.Query<T>(sql, new { Id = id }).FirstOrDefault();
    }

    public IEnumerable<T> GetAll<T>(string tableName)
    {
        var sql = $"SELECT * FROM {tableName};";
        return _connection.Query<T>(sql);
    }
}

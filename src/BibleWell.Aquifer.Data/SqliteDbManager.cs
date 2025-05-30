using BibleWell.Storage;
using Dapper;
using Microsoft.Data.Sqlite;

namespace BibleWell.Aquifer.Data;

public class SqliteDbManager
{
    private readonly string _connectionString;
    private readonly string _databasePath;
    private readonly IStorageService _storageService;

    public SqliteDbManager(IStorageService storageService)
    {
        _storageService = storageService;
        _databasePath = Path.Combine(_storageService.ApplicationDirectoryPath, Constants.AquiferDatabaseFilename);
        _connectionString = $"Data Source={_databasePath}";
        InitializeDatabase();
    }

    public SqliteConnection CreateConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }

    private void InitializeDatabase()
    {
        // Ensure the directory exists
        var directory = Path.GetDirectoryName(_databasePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // ensure DB is in WAL mode
        using var connection = new SqliteConnection(_connectionString);
        const string sql = "PRAGMA journal_mode = WAL;";
        connection.Execute(sql);

        // TODO ensure all tables have been created using repositories?
        // TODO add other tables here or swap this entire section for migrations
    }
}
namespace BibleWell.Aquifer.Data;

internal class ResourceContentRepository
{
    private const string tableName = "ResourceContents";
    private bool _hasBeenInitialized  = false;
    private readonly SqliteDbManager _dbManager;
    // private readonly ILogger _logger;

    public ResourceContentRepository(SqliteDbManager dbManager)
    {
        // _logger = logger;
        _dbManager = dbManager;
        _ = InitAsync();
    }

    private async Task InitAsync()
    {
        if (_hasBeenInitialized)
        {
            return;
        }

        await using var connection = _dbManager.CreateConnection();
        connection.Open();

        try
        {
            await using var command = connection.CreateCommand();
            command.CommandText = $@"
                CREATE TABLE IF NOT EXISTS {tableName} (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Content TEXT NOT NULL
                );";
           await command.ExecuteNonQueryAsync();

           await InsertAsync(new ResourceContent(0, "Genesis test RC", "Genesis Test RC Content"));
        }
        catch (Exception e)
        {
            // todo: log and/or throw
            Console.WriteLine(e);
            throw new NotImplementedException(e.Message);
        }
        
        _hasBeenInitialized = true;
    }
    
    public async Task<ResourceContent?> GetByIdAsync(int id)
    {
        await using var connection = _dbManager.CreateConnection();
        await using var command = connection.CreateCommand();
        command.CommandText = $@"SELECT Id, Name, Content FROM {tableName} WHERE Id = @Id;";
        command.Parameters.AddWithValue("@Id", id);

        await using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            return null;
        }

        var resourceId = reader.GetInt32(0);
        var name = reader.GetString(1);
        var content = reader.GetString(2);

        return new ResourceContent(resourceId, name, content);
    }
    
    public async Task<int> SaveAsync(ResourceContent resourceContent)
    {
        ArgumentNullException.ThrowIfNull(resourceContent);

        if (resourceContent.Id <= 0)
        {
            return await InsertAsync(resourceContent);
        }
        
        return await UpdateAsync(resourceContent);
    }

    private async Task<int> InsertAsync(ResourceContent resourceContent) 
    {
        await using var connection = _dbManager.CreateConnection();
        await using var command = connection.CreateCommand();

        command.CommandText = $@"INSERT INTO {tableName} (Name, Content) VALUES (@Name, @Content);";
        command.Parameters.AddWithValue("@Name", resourceContent.Name);
        command.Parameters.AddWithValue("@Content", resourceContent.Content);
        // todo: error handling
        return await command.ExecuteNonQueryAsync();
    }

    private async Task<int> UpdateAsync(ResourceContent resourceContent)
    {
        await using var connection = _dbManager.CreateConnection();
        await using var command = connection.CreateCommand();

        command.CommandText = $@"UPDATE {tableName} SET Content = @Content, Name = @Name WHERE Id = @Id;";
        command.Parameters.AddWithValue("@Id", resourceContent.Id);
        command.Parameters.AddWithValue("@Name", resourceContent.Name);
        command.Parameters.AddWithValue("@Content", resourceContent.Content);
        // todo: error handling 
        return await command.ExecuteNonQueryAsync();
    }

    
    public async Task<IEnumerable<ResourceContent>> GetAllAsync()
    {
        var resourceContents = new List<ResourceContent>();

        await using var connection = _dbManager.CreateConnection();
        await using var command = connection.CreateCommand();
        command.CommandText = $@"SELECT Id, Name, Content FROM {tableName};";

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var id = reader.GetInt32(0);
            var name = reader.GetString(1);
            var content = reader.GetString(2);
            resourceContents.Add(new ResourceContent(id, name, content));
        }

        return resourceContents;
    }
}
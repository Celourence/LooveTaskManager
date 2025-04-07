namespace LooveTaskManager.Infrastructure.Data;

public static class TaskQueries
{
    public const string Insert = @"
        INSERT INTO Tasks (Id, Title, Description, CreatedAt, DueDate, Status)
        VALUES (@Id, @Title, @Description, @CreatedAt, @DueDate, @Status)";

    public const string Update = @"
        UPDATE Tasks 
        SET Title = @Title, 
            Description = @Description, 
            DueDate = @DueDate, 
            Status = @Status
        WHERE Id = @Id";

    public const string Delete = "DELETE FROM Tasks WHERE Id = @Id";

    public const string ExistsByTitle = "SELECT COUNT(1) FROM Tasks WHERE Title = @Title";

    public const string GetById = "SELECT * FROM Tasks WHERE Id = @Id";

    public const string GetAll = "SELECT * FROM Tasks ORDER BY CreatedAt DESC";

    public const string GetByTitle = "SELECT * FROM Tasks WHERE Title = @Title";
} 
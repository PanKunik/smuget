namespace Domain.Exceptions;

public class ConflictException
    : SmugetException
{
    public string EntityName { get; init; }

    public ConflictException(
        string entityName,
        string key,
        string keyValue
    )
        : base($"Entity `{entityName}` with `{key} = {keyValue}` already exists.")
    {
        EntityName = entityName;
    }

    public ConflictException(
        string message,
        string entityName
    )
        : base(message)
    {
        EntityName = entityName;
    }
}
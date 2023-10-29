namespace Domain.Exceptions;

public class NotFoundException
    : SmugetException
{
    public string EntityName { get; init; }

    public NotFoundException(
        string entityName,
        string key,
        string keyValue
    ) : base($"Entity `{entityName}` with key `{key} = {keyValue}` couldn't be found.")
    {
        EntityName = entityName;
    }

    public NotFoundException(
        string message,
        string entityName
    ) : base(message)
    {
        EntityName = entityName;
    }
}
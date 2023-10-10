namespace Infrastructure.Persistance.Entities;

internal sealed class UserEntity
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string SecuredPassword { get; set; }
}
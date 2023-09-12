namespace Infrastructure.Exceptions;

public record Error(
    string Reason,
    string Code,
    string Instance
);
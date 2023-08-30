using System.Reflection;

namespace Domain.Abstractions;

public abstract record Enumeration<T>
    where T : Enumeration<T>
{
    public int Id { get; private set; }
    public string Name { get; private set; }

    protected Enumeration(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public override string ToString()
        => Name;

    public static T GetById(int id)
    {
        return GetAll<T>()
            .Single(s => s.Id == id);
    }

    public static T GetByName(string name)
    {
        return GetAll<T>()
            .Single(s => s.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
    }

    private static IEnumerable<T> GetAll<T>()
    {
        var fields = typeof(T).GetFields(
            BindingFlags.Public |
            BindingFlags.Static |
            BindingFlags.DeclaredOnly
        );

        return fields
            .Select(f => f.GetValue(null))
            .Cast<T>();
    }
}
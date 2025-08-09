namespace LightHouseDomain.Common;

public abstract class EnumerationBase(int id, string name)
{
    public int Id { get; } = id;
    public string Name { get; } = name;

    public override string ToString()
    {
        return Name;
    }
}

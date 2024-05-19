namespace Refithance.Generator;

public readonly struct RefithanceInfo(string name, string fullName)
{
    public string Name { get; } = name;
    public string FullName { get; } = fullName;
}

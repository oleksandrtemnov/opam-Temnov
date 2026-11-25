namespace Lab09;

public record Student(string Name, string Group, int Score)
{
    public override string ToString() => $"{Name} ({Group}): {Score}";
}
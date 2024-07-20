namespace YanehCheck.EpicGamesUtils.ConsoleApp.Commands;

public record Argument
{
    public Func<string, bool> Validator { private get; init; }

    public bool Validate(string value)
    {
        return Validator(value);
    }
}
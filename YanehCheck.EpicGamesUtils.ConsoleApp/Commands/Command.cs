using YanehCheck.EpicGamesUtils.ConsoleApp.Enums;

namespace YanehCheck.EpicGamesUtils.ConsoleApp.Commands;

public record Command
{
    public string Name { get; init; }
    public IEnumerable<string> Aliases { get; init; } = [];
    public IEnumerable<Argument> Arguments { get; init; } = [];
    public ClientState RequiredState { get; init; }
    public Func<List<string>?, Task> Action { private get; init; }

    public async Task Execute(List<string>? arguments) => await Action?.Invoke(arguments);

    public bool ValidateStructure(string command, out List<string>? outArguments)
    {
        var parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        // Name
        if (parts.Length == 0 ||
            (parts[0] != Name && !Aliases.Contains(parts[0])) ||
            parts.Length - 1 != Arguments.Count())
        {
            outArguments = null;
            return false;
        }

        // Arg valid
        var parsedArguments = new List<string>();
        for (var i = 0; i < Arguments.Count(); i++)
        {
            var valid = Arguments.ElementAt(i).Validate(parts[i + 1]);
            if (!valid)
            {
                outArguments = null;
                return false;
            }
            else
            {
                parsedArguments.Add(parts[i + 1]);
            }
        }
        outArguments = parsedArguments;
        return true;
    }

    public bool ValidateState(ClientState state) => state.HasFlag(RequiredState);

}
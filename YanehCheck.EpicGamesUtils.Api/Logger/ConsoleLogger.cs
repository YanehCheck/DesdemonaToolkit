namespace YanehCheck.EpicGamesUtils.Api.Logger;

public class ConsoleLogger : ILogger {
    public void Message(string message) {
        Console.Error.WriteLine("DEBUG: " + message);
    }

    public void ResponseContent(string message) {
        Console.Error.WriteLine("DEBUG ============");
        Console.Error.WriteLine(message);
        Console.Error.WriteLine("==================");
    }
}
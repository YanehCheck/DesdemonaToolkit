namespace YanehCheck.EpicGamesUtils.Api.Logger;

public interface ILogger {
    void Message(string message);
    void ResponseContent(string responseContent);
}
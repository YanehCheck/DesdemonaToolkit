namespace YanehCheck.EpicGamesUtils.Api;

public record ApiResult(bool Success, string? Message = null) {
    public bool Success { get; set; } = Success;
    public string? Message { get; set; } = Message;

    public static ApiResult Ok() => new(true);
    public static ApiResult Error(string message) => new(false, message);

    public static implicit operator bool(ApiResult result) => result.Success;
    public static implicit operator string?(ApiResult result) => result.Message;
}
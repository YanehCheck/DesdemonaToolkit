namespace YanehCheck.EpicGamesUtils.EgsApi.Api.Enums;

public enum FounderCodePlatform
{
    Epic,
    Xbox
}

public static class FortniteCodePlatformExtensions {
    public static string ToParameterString(this FounderCodePlatform profile) {
        return profile switch {
            FounderCodePlatform.Epic => "epic",
            FounderCodePlatform.Xbox => "xbox",
            _ => throw new ArgumentException("Invalid FounderCodePlatform value.")
        };
    }
}
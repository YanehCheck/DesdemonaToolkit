using YanehCheck.EpicGamesUtils.EgsApi.Api.Enums;

namespace YanehCheck.EpicGamesUtils.EgsApi.Api.Helpers;

public record AuthClient(string Name, string Id, string Secret) {
    public static AuthClient GetClient(AuthClientType authClientType) => authClientType switch {
        AuthClientType.FortnitePcGameClient =>
            new AuthClient("fortnitePCGameClient",
            "ec684b8c687f479fadea3cb2ad83f5c6",
            "e1f31c211f28413186262d37a13fc84d"),
        AuthClientType.FortniteIosGameClient =>
            new AuthClient("fortniteIOSGameClient",
            "3446cd72694c4a4485d81b77adbb2141",
            "9209d4a5e25a457fb9b07489d313b41a"),
        AuthClientType.FortniteSwitchGameClient =>
            new AuthClient("fortniteSwitchGameClient",
            "5229dcd3ac3845208b496649092f251b",
            "e3bd2d3e-bf8c-4857-9e7d-f3d947d220c7"),
        AuthClientType.FortniteNewSwitchGameClient =>
            new AuthClient("fortniteNewSwitchGameClient",
            "98f7e42c2e3a4f86a74eb43fbb41ed39",
            "0a2449a2-001a-451e-afec-3e812901c4d7"),
        AuthClientType.FortniteAndroidGameClient =>
            new AuthClient("fortniteAndroidGameClient",
            "3f69e56c7649492c8cc29f1af08a8a12",
            "b51ee9cb12234f50a69efa67ef53812e"),
        AuthClientType.FortniteXboxGameClient =>
            new AuthClient("fortniteXboxGameClient",
            "cfaa14c4bf8744e3a5ef9a5d6c34558d",
            ""),
        AuthClientType.FortniteXsxGameClient =>
            new AuthClient("fortniteXSXGameClient",
            "db84fa58b60e468ba64e3b17209b56e9",
            ""),
        AuthClientType.FortniteValkyrieGameClient =>
            new AuthClient("fortniteValkyrieGameClient",
            "3e13c5c57f594a578abe516eecb673fe",
            "530e316c337e409893c55ec44f22cd62"),
        AuthClientType.FortniteCnGameClient =>
            new AuthClient("fortniteCNGameClient",
            "efe3cbb938804c74b20e109d0efc1548",
            "6e31bdbae6a44f258474733db74f39ba"),
        AuthClientType.FortniteComClient =>
            new AuthClient("fortniteComClient",
            "cd2b7c19c9734a2ab98dc251868d7724",
            ""),
        AuthClientType.FortnitePs4EuGameClient =>
            new AuthClient("fortnitePS4EUGameClient",
            "79a931b375334570ac369234f5da05ec",
            ""),
        AuthClientType.FortnitePs4UsGameClient =>
            new AuthClient("fortnitePS4USGameClient",
            "d8566f2e7f5c48f89683173eb529fee1",
            "255c7109c8274241986616e3702678b5"),
        AuthClientType.FortnitePs5EuGameClient =>
            new AuthClient("fortnitePS5EUGameClient",
            "386cbbc78d57464181005c3f7edfad0d",
            ""),
        AuthClientType.FortnitePs5UsGameClient =>
            new AuthClient("fortnitePS5USGameClient",
            "03f2645147214e1ab368caa78c5fca81",
            ""),
        AuthClientType.FortnitePs5UsGameClientTest =>
            new AuthClient("fortnitePS5USGameClientTest",
            "3cf19c6ba05a4fa3997957491e15ba1c",
            ""),
        AuthClientType.FortnitePcQaGameClientTest =>
            new AuthClient("fortnitePCQAGameClientTest",
            "81ffd992c8a94ccaaaa6bd74c073ce6a",
            ""),
        AuthClientType.FortniteEsportsComClient =>
            new AuthClient("fortniteEsportsComClient",
            "0fa561666d6e41cb95b2a357a8b4a6f3",
            ""),
        _ => throw new InvalidOperationException("Unknown auth client type.")
    };
}
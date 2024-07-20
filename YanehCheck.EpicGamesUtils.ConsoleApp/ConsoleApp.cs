using System.Diagnostics;
using Microsoft.Extensions.Options;
using YanehCheck.EpicGamesUtils.Api;
using YanehCheck.EpicGamesUtils.Api.Auth;
using YanehCheck.EpicGamesUtils.Api.Stw;
using YanehCheck.EpicGamesUtils.ConsoleApp.Commands;
using YanehCheck.EpicGamesUtils.ConsoleApp.Enums;
using YanehCheck.EpicGamesUtils.ConsoleApp.Items;
using YanehCheck.EpicGamesUtils.ConsoleApp.Options;
using YanehCheck.EpicGamesUtils.ConsoleApp.Resources;
using YanehCheck.EpicGamesUtils.FortniteGGScraper;
using YanehCheck.General.InterConsoleLibrary;
using YanehCheck.General.InterConsoleLibrary.Colors;

namespace YanehCheck.EpicGamesUtils.ConsoleApp;

public class ConsoleApp(IEpicGamesClient client, IFortniteGgScrapper scrapper, 
    IUriItemFetcher uriItemFetcher, IItemManager itemManager, IOptions<ItemOptions> config) : IConsoleApp {

    private ClientState _state = ClientState.Started;
    private readonly HashSet<Command> Commands = [];

    public async Task Run() {
        SetupConsole();
        SetupCommands();

        InterConsole.WriteRawLine(Strings.Title);
        InterConsole.WriteLine(Strings.Introduction);

        while(true) {
            string userCommand = InterConsole.ReadLine()!;
            Command command = FindCommand(userCommand, out var parsedArguments);
            await command.Execute(parsedArguments);
        }
    }

    private void SetupConsole() {
        if(Environment.OSVersion.Platform == PlatformID.Win32NT) {
            Console.SetBufferSize(Console.BufferHeight > 250 ? Console.BufferHeight + 1 : 250,
                                Console.BufferWidth > 100 ? Console.BufferWidth + 1 : 100);
        }
        InterConsole.ChoiceCursor = $"{AnsiColors.BrightGreen}=> {AnsiColors.Reset}";
        InterConsole.InputPrefix = $"{AnsiColors.BrightGreen}< {AnsiColors.Reset}";
        InterConsole.OutputPrefix = $"{AnsiColors.BrightYellow}> {AnsiColors.Reset}";
        InterConsole.LongOutputPrefix = $"{AnsiColors.BrightYellow}| {AnsiColors.Reset}";
        InterConsole.ChoiceSpacing = 5;
    }

    private void SetupCommands() {
        // UTILS
        Commands.Add(new Command {
            Name = "exit",
            RequiredState = ClientState.Started,
            Action = async (_) => Environment.Exit(0)
        });
        Commands.Add(new Command {
            Name = "clear",
            RequiredState = ClientState.Started,
            Action = async (_) => InterConsole.Clear()
        });
        Commands.Add(new Command {
            Name = "help",
            RequiredState = ClientState.Started,
            Action = async (_) => InterConsole.WriteLongLine(Strings.HelpCommand)
        });

        // NO REQUIREMENTS
        Commands.Add(new Command() {
            Name = "fetch-item-info",
            RequiredState = ClientState.Started,
            Action = async (arg) => await HandleFetchItemInfo(arg),
            Arguments = [
                new Argument {
                    Validator = (arg) => arg.ToLower() is "fgg" or "gh"
                }
            ]
        });
        Commands.Add(new Command() {
            Name = "fetch-item-info",
            Aliases = ["fetch"],
            RequiredState = ClientState.Started,
            Action = async (arg) => await HandleFetchItemInfo(arg),
            Arguments = [
                new Argument {
                    Validator = (arg) => arg.ToLower() is "fgg"
                },
                new Argument {
                    Validator = (arg) => {
                        var result = int.TryParse(arg, out var value);
                        return result && value is > 0 and < 15000;
                    }
                }
            ]
        });

        Commands.Add(new Command {
            Name = "auth",
            RequiredState = ClientState.Started,
            Action = async (_) => await HandleAuth(null)
        });
        Commands.Add(new Command {
            Name = "auth",
            RequiredState = ClientState.Started,
            Action = async (arg) => await HandleAuth(arg),
            Arguments = [
                new Argument {
                    Validator = (s) => s.Length == 32
                }
            ]
        });

        // AUTHENTICATED
        Commands.Add(new Command {
            Name = "founder-codes",
            RequiredState = ClientState.Authenticated,
            Action = async (arg) => await HandleFounderCode(null)
        });
        Commands.Add(new Command {
            Name = "founder-codes",
            Aliases = ["founder"],
            RequiredState = ClientState.Authenticated,
            Action = async (arg) => await HandleFounderCode(arg),
            Arguments = [
                new Argument {
                    Validator = (s) => s.ToLower() is "epic" or "xbox"
                }
            ]
        });
        Commands.Add(new Command {
            Name = "inventory",
            Aliases = ["inv"],
            RequiredState = ClientState.Authenticated,
            Action = async (arg) => await HandleInventoryRequest(arg),
            Arguments = [
                new Argument() {
                    Validator = (s) => s.ToLower() is "raw" or "list" or "json" or "web"
                }
            ]
        });

        // INTERNAL
        Commands.Add(new Command {
            Name = "invalid_command_for_ease_of_me",
            RequiredState = ClientState.Started,
            Action = async (_) => InterConsole.WriteLine(Strings.InvalidCommandError)
        });
        Commands.Add(new Command {
            Name = "invalid_state_for_ease_of_me",
            RequiredState = ClientState.Started,
            Action = async (_) => InterConsole.WriteLine(Strings.NotAuthenticatedError)
        });
    }

    private Command FindCommand(string userCommand, out List<string>? outArguments) {
        List<string>? args = null;
        Command? com = null;
        foreach(var c in Commands) {
            if(c.ValidateStructure(userCommand, out args)) {
                com = c;
                break;
            }
        }
        if(com == null) {
            outArguments = null;
            return Commands.Single(c => c.Name == "invalid_command_for_ease_of_me");
        }
        if(!com.ValidateState(_state)) {
            outArguments = null;
            return Commands.Single(c => c.Name == "invalid_state_for_ease_of_me");
        }
        outArguments = args;
        return com;
    }

    private async Task HandleInventoryRequest(List<string> args) {
        var format = EnumExtensions.FromStringToItemOutputFormat(args[0]);
    }

    private async Task HandleFetchItemInfo(List<string> args) {
        var target = EnumExtensions.FromStringToFetchItemTarget(args[0]);

        if (target == FetchItemTarget.FortniteGg) {
            var range = Convert.ToInt32(args.Count > 1 ? args[1] : config.Value.DefaultFortniteGgMaxId);
            var items = await scrapper.ScrapIdRangeParallelAsync(0, range);
            if (items.IsEmpty) {
                InterConsole.WriteLine(Strings.FetchCommandFail);
                return;
            }
            await itemManager.Save(items);
        }
        else {
            var json = await uriItemFetcher.FetchAsJson();
            if (json == null) {
                InterConsole.WriteLine(Strings.FetchCommandFail);
                return;
            }
            await itemManager.Save(json);
        }
        InterConsole.WriteLine(Strings.FetchCommandSuccess);
    }

    private async Task HandleAuth(List<string>? args) {
        // Check if auth code was an argument
        string authCode;
        if(args == null || !args.Any()) {
            var browser = Process.Start("explorer", "\"https://epicgames.com/id/api/redirect?clientId=ec684b8c687f479fadea3cb2ad83f5c6&responseType=code\"");
            InterConsole.WriteLine(Strings.AuthCommandPrompt);
            authCode = InterConsole.ReadLine()!;
            browser.Kill();
        }
        else {
            authCode = args[0];
        }

        var result = await client.AuthenticateAsAccount(AuthClientType.FortnitePcGameClient, authCode);

        if(result) {
            _state = ClientState.Authenticated;
        }
        InterConsole.WriteLine(result ? Strings.AuthCommandSuccess : Strings.AuthCommandError);
        if(!result) {
            InterConsole.WriteLine(result!);
        }
    }

    private async Task HandleFounderCode(List<string> args) {
        FounderCodePlatform platform;
        if(args == null || !args.Any()) {
            platform = InterConsole.PromptMultipleChoice<FounderCodePlatform>("Pick what platform to check.",
                ["xbox", "epic"],
                new Dictionary<string, FounderCodePlatform> {
                    {"epic", FounderCodePlatform.Epic},
                    {"xbox", FounderCodePlatform.Xbox}
                });
        }
        else {
            Enum.TryParse(args[0], true, out platform);
        }
        var result = await client.GetFounderCodes(platform);
        InterConsole.WriteLine(result ? Strings.FounderCodeCommandSuccess : Strings.GeneralError);
        InterConsole.WriteLongLine(string.IsNullOrEmpty(result) ? "NO CODES :(" : result.Message);
    }
}
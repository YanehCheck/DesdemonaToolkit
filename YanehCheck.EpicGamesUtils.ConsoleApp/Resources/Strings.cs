using YanehCheck.General.InterConsoleLibrary.Colors;

namespace YanehCheck.EpicGamesUtils.ConsoleApp.Resources;

internal static class Strings
{
    public static string Title => $"""
                                  {AnsiColors.Green}    __________  ____  _______   __________________{AnsiColors.Yellow}   __  ______________   _____
                                  {AnsiColors.Green}   / ____/ __ \/ __ \/_  __/ | / /  _/_  __/ ____/{AnsiColors.Yellow}  / / / /_  __/  _/ /  / ___/
                                  {AnsiColors.Green}  / /_  / / / / /_/ / / / /  |/ // /  / / / __/   {AnsiColors.Yellow} / / / / / /  / // /   \__ \ 
                                  {AnsiColors.Green} / __/ / /_/ / _, _/ / / / /|  // /  / / / /___   {AnsiColors.Yellow}/ /_/ / / / _/ // /______/ / 
                                  {AnsiColors.Green}/_/    \____/_/ |_| /_/ /_/ |_/___/ /_/ /_____/   {AnsiColors.Yellow}\____/ /_/ /___/_____/____/  
                                  {AnsiColors.Reset}                                                                              
                                  """;

    public static string Introduction => "Small utility for the god forsaken epic games API! See help for a list of available commands.";

    public static string HelpCommand => $"""
                                         {AnsiColors.BrightYellow}help{AnsiColors.Reset} - Shows this message.
                                         {AnsiColors.BrightYellow}clear{AnsiColors.Reset} - Clears the terminal.
                                         {AnsiColors.BrightYellow}exit{AnsiColors.Reset} - Terminates the program. Ctrl+C also works.
                                         {AnsiColors.BrightYellow}fetch-item-info {AnsiColors.BrightGreen}(fgg/gh){AnsiColors.Reset} - Fetches fresh item information.
                                          ├ {AnsiColors.BrightGreen}fgg (0-13500){AnsiColors.Reset} = Latest information from fortnite.gg. You can specify the item ID range (currently 13500 for all items)
                                          └ {AnsiColors.BrightGreen}gh{AnsiColors.Reset} = Very fast, stable, but infrequently updated information from official github.
                                         {AnsiColors.BrightYellow}auth{AnsiColors.Reset} - Authenticate using this command. Make sure you are logged into Epic Games on your default browser. Browser window will automatically open and all you need to do is copy the "authorizationCode" and paste it into the terminal.
                                         {AnsiColors.BrightYellow}founder-codes {AnsiColors.BrightGreen}(epic/xbox){AnsiColors.Reset} - Retrieves STW founder codes for the specified platform. Requires authentication.
                                         """;

    // Generic
    public static string GeneralError => $"{AnsiColors.Red}Following error occured:{AnsiColors.Reset}";
    public static string NotAuthenticatedError => $"{AnsiColors.Red}Authenticate first! See help.{AnsiColors.Reset}";
    public static string InvalidCommandError => $"{AnsiColors.Red}Invalid command. See help for a list of available commands{AnsiColors.Reset}";

    // Command specific
    public static string AuthCommandPrompt => "Please input the authorization code (without quotes):";
    public static string AuthCommandSuccess => $"{AnsiColors.BrightGreen}Successfully authenticated!{AnsiColors.Reset}";
    public static string AuthCommandError => $"{AnsiColors.Red}Following error occured while authenticating (maybe try to be faster?):{AnsiColors.Reset}";

    public static string FetchCommandFail =>
        $"{AnsiColors.Red}Could not fetch item information from specified source.{AnsiColors.Reset}";
    public static string FetchCommandSuccess =>
        $"{AnsiColors.BrightGreen}Successfully fetched item information.{AnsiColors.Reset}";

    public static string FounderCodeCommandSuccess => $"{AnsiColors.BrightGreen}Successfully retrieved! See the list below for retrieved codes:{AnsiColors.Reset}";
}
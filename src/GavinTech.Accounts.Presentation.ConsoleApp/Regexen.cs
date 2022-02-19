namespace GavinTech.Accounts.Presentation.ConsoleApp;

internal static class Regexen
{
    internal const string Day = "[0-9]{4}-[0-9]{2}-[0-9]{2}";

    internal const string Amount = @"\$?-?[0-9]+(?:\.[0-9]{2})?";

    internal const string Recurrence = "[0-9]+[md]";
}

using System;

namespace HelseId.JwkGenerator;

internal static class Logger
{
    public static void Info(string message)
    {
        LogWithColoredPrefix("Information: ", ConsoleColor.Blue, message);
    }

    public static void Warning(string message)
    {
        LogWithColoredPrefix("Warning: ", ConsoleColor.Yellow, message);
    }

    public static void Error(string message)
    {
        LogWithColoredPrefix("Error: ", ConsoleColor.Red, message);
    }

    public static void Success(string message)
    {
        LogWithColoredPrefix("Success: ", ConsoleColor.Green, message);
    }

    private static void LogWithColoredPrefix(string prefix, ConsoleColor color, string message)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(prefix);
        Console.ForegroundColor = originalColor;
        Console.WriteLine(message);
    }
}
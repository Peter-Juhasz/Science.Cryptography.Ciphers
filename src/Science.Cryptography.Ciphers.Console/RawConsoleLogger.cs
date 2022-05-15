using Microsoft.Extensions.Logging;
using System;

namespace Science.Cryptography.Ciphers.Console;

internal sealed class RawConsoleLogger : ILogger
{
	public IDisposable BeginScope<TState>(TState state) => NullDisposable.Instance;

	public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Information;

	private readonly object syncLock = new();

	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
	{
		lock (syncLock)
		{
			System.Console.ForegroundColor = logLevel switch
			{
				LogLevel.Error => ConsoleColor.Red,
				LogLevel.Warning => ConsoleColor.Yellow,
				< LogLevel.Information => ConsoleColor.DarkGray,
				_ => ConsoleColor.Gray,
			};
			if (logLevel == LogLevel.Information)
			{
				System.Console.ResetColor();
			}
			System.Console.WriteLine(formatter(state, exception));
			if (exception != null)
			{
				System.Console.WriteLine(exception);
			}
		}
	}

	private sealed class NullDisposable : IDisposable
	{
		public static readonly IDisposable Instance = new NullDisposable();

		public void Dispose()
		{
			System.Console.ResetColor();
		}
	}
}

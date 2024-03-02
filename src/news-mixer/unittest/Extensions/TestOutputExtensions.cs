using Microsoft.Extensions.Logging;
using System.Text;
using Xunit.Abstractions;

namespace NewsMixer.UnitTests.Extensions
{
    internal static class TestOutputExtensions
    {
        internal static ILogger ToLogger(this ITestOutputHelper output)
        {
            return XUnitLogger.CreateLogger(output);
        }

        internal class XUnitLogger : ILogger
        {
            private readonly ITestOutputHelper _testOutputHelper;
            private readonly string _categoryName;
            private readonly LoggerExternalScopeProvider _scopeProvider;

            public static ILogger CreateLogger(ITestOutputHelper testOutputHelper) => new XUnitLogger(testOutputHelper, new LoggerExternalScopeProvider(), "");
            public static ILogger<T> CreateLogger<T>(ITestOutputHelper testOutputHelper) => new XUnitLogger<T>(testOutputHelper, new LoggerExternalScopeProvider());

            public XUnitLogger(ITestOutputHelper testOutputHelper, LoggerExternalScopeProvider scopeProvider, string categoryName)
            {
                _testOutputHelper = testOutputHelper;
                _scopeProvider = scopeProvider;
                _categoryName = categoryName;
            }

            public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

#pragma warning disable CS8633 // Nullability in constraints for type parameter doesn't match the constraints for type parameter in implicitly implemented interface method'.
            public IDisposable BeginScope<TState>(TState state) => _scopeProvider.Push(state);
#pragma warning restore CS8633 // Nullability in constraints for type parameter doesn't match the constraints for type parameter in implicitly implemented interface method'.

#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
            {
                var sb = new StringBuilder();
                sb.Append(GetLogLevelString(logLevel))
                  .Append(" [").Append(_categoryName).Append("] ")
                  .Append(formatter(state, exception));

                if (exception != null)
                {
                    sb.Append('\n').Append(exception);
                }

                // Append scopes
                _scopeProvider.ForEachScope((scope, state) =>
                {
                    state.Append("\n => ");
                    state.Append(scope);
                }, sb);

                _testOutputHelper.WriteLine(sb.ToString());
            }

            private static string GetLogLevelString(LogLevel logLevel)
            {
                return logLevel switch
                {
                    LogLevel.Trace => "trce",
                    LogLevel.Debug => "dbug",
                    LogLevel.Information => "info",
                    LogLevel.Warning => "warn",
                    LogLevel.Error => "fail",
                    LogLevel.Critical => "crit",
                    _ => throw new ArgumentOutOfRangeException(nameof(logLevel))
                };
            }
        }

#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
#pragma warning disable CS8633 // Nullability in constraints for type parameter doesn't match the constraints for type parameter in implicitly implemented interface method'.
        internal sealed class XUnitLogger<T> : XUnitLogger, ILogger<T>
#pragma warning restore CS8633 // Nullability in constraints for type parameter doesn't match the constraints for type parameter in implicitly implemented interface method'.
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        {
            public XUnitLogger(ITestOutputHelper testOutputHelper, LoggerExternalScopeProvider scopeProvider)
#pragma warning disable CS8604 // Possible null reference argument.
                : base(testOutputHelper, scopeProvider, typeof(T).FullName)
#pragma warning restore CS8604 // Possible null reference argument.
            {
            }
        }
    }
}

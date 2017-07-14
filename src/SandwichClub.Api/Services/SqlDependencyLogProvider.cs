using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace SandwichClub.Api.Services
{
    public class SqlDependencyLogProvider : ILoggerProvider
    {
        private readonly TelemetryClient _telemetryClient;

        public SqlDependencyLogProvider(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
        }

        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            if (categoryName == "Microsoft.EntityFrameworkCore.Database.Command")
                return new SqlDependencyLogger(_telemetryClient);
            return NullLogger.Instance;
        }
    }

    public class SqlDependencyLogger : ILogger
    {
        private readonly TelemetryClient _telemetryClient;

        public SqlDependencyLogger(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (eventId.Id != RelationalEventId.CommandExecuted.Id)
                return;

            var data = state as IEnumerable<KeyValuePair<string, object>>;
            if (data == null)
                return;

            var dataList = data.ToDictionary(p => p.Key, p => p.Value);
            var elapsedMs = Convert.ToInt32(dataList["elapsed"]);
            var commandText = dataList["commandText"].ToString();

            // Ignore empty sql, or Pragma commands
            if (string.IsNullOrEmpty(commandText) || commandText.StartsWith("PRAGMA "))
                return;

            var dependencyTelemetry = new DependencyTelemetry
            {
                Type = "SQL",
                Name = commandText,
                Timestamp = DateTimeOffset.Now,
                Duration = TimeSpan.FromMilliseconds(elapsedMs),
                Success = exception == null,
                ResultCode = exception != null ? "0" : "?",
            };

            _telemetryClient.TrackDependency(dependencyTelemetry);
        }

        public IDisposable BeginScope<TState>(TState state) => null;
    }
    }

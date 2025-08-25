namespace Utils;

using Microsoft.Extensions.Logging;
using Serilog;

 public static class Logger
    {
        public static Microsoft.Extensions.Logging.ILogger LoggerApp { get; private set; }

        static Logger()
        {
            var logPath = Environment.GetEnvironmentVariable("LOG_PATH") ?? "/app/data/logs";
            Directory.CreateDirectory(logPath);

            // Configuración de Serilog
            var serilogLogger = new LoggerConfiguration()
                .WriteTo.File(Path.Combine(logPath, "app.log"), rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Integrar con Microsoft.Extensions.Logging
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog(serilogLogger);
            });

            LoggerApp = loggerFactory.CreateLogger("AppLogger");

        }
    }
namespace AtbShop.Helpers
{
    public static class AppLoggerFile
    {
        public static void UseLoggerFile(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var path = Path.Combine(System.Environment.CurrentDirectory, "Logs");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var fileLog = Path.Combine(path, "log-{Date}.txt");
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                loggerFactory.AddFile(fileLog);
            }
        }
    }
}

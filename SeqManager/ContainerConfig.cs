using Abstractions;
using Autofac;
using Services;

namespace SeqManager;

public static class ContainerConfig
{
    public static IContainer ConfigureContainer()
    {
        var builder = new ContainerBuilder();

        // Dependency Injection Configuration
        builder.RegisterType<AppSettingsService>().As<IAppSettingsService>();
        builder.RegisterType<LogFileImportServiceService>().As<ILogFileImportService>();

        return builder.Build();
    }
}
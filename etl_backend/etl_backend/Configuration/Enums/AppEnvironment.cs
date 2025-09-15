namespace etl_backend.Configuration.Enums;

public enum AppEnvironment
{
    Development,
    Test,
    Production,
    Unknown
}

public static class AppEnvironmentExtensions
{
    public static AppEnvironment ToAppEnvironment(this string? environmentName) =>
        environmentName?.ToLowerInvariant() switch
        {
            "development" => AppEnvironment.Development,
            "test"        => AppEnvironment.Test,
            "production"  => AppEnvironment.Production,
            _             => AppEnvironment.Unknown
        };
    
}
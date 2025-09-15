namespace Domain.ValueObjects.PluginConfig;

public record FilterConfig(
    string Condition  
) : PluginConfig;
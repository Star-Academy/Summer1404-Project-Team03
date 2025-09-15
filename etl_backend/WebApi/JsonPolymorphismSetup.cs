using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Domain.Entities;
using Domain.ValueObjects.PluginConfig;

namespace WebApi;

public static class JsonPolymorphismSetup
{
    public static void ConfigurePluginConfigPolymorphism(JsonTypeInfo typeInfo)
    {
        if (typeInfo.Type != typeof(PluginConfig)) return;

        var options = new JsonPolymorphismOptions
        {
            TypeDiscriminatorPropertyName = "$type",
            IgnoreUnrecognizedTypeDiscriminators = false,
            UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization
        };

        options.DerivedTypes.Add(new JsonDerivedType(typeof(FilterConfig), "FilterConfig"));
        // options.DerivedTypes.Add(new JsonDerivedType(typeof(AggregateConfig), "AggregateConfig"));
        // Add more as needed:
        // options.DerivedTypes.Add(new JsonDerivedType(typeof(FileConfig), "FileConfig"));

        typeInfo.PolymorphismOptions = options;
    }
}
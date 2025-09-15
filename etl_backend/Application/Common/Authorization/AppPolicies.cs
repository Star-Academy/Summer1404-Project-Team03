namespace Application.Common.Authorization;

public static class AppPolicies
{
    public const string RequireSysAdmin = nameof(RequireSysAdmin);
    public const string RequireDataAdmin = nameof(RequireDataAdmin);
    public const string RequireAnalyst = nameof(RequireAnalyst);

    public const string RequireAtLeastAnalyst = nameof(RequireAtLeastAnalyst);
    public const string RequireAtLeastDataAdmin = nameof(RequireAtLeastDataAdmin);
}
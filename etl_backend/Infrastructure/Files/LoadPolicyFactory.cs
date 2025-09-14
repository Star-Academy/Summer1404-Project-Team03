using Application.Abstractions;
using Application.Common.Services.Abstractions;
using Application.Enums;
using Application.Services.Abstractions;
using Infrastructure.Files.PostgresTableServices.HelperServices;

namespace Infrastructure.Files;

public sealed class LoadPolicyFactory : ILoadPolicyFactory { public ILoadPolicy Create(LoadMode m, bool d) => new LoadPolicy(m, d); }

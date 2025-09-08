using Application.Abstractions;
using Application.Files.Commands;
using Infrastructure.Files.PostgresTableServices.HelperServices;
// using ILoadPolicyFactory = Infrastructure.Files.Abstractions.ILoadPolicyFactory;

namespace Infrastructure.Files;

public sealed class LoadPolicyFactory : ILoadPolicyFactory { public ILoadPolicy Create(LoadMode m, bool d) => new LoadPolicy(m, d); }

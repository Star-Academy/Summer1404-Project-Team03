// // Services/RoleMapper.cs
// using System.Text.Json;
// using etl_backend.Services.Auth.Abstraction;
//
// public class RoleMapper : IRoleMapper       
// {
//     public IEnumerable<string> MapRolesFromClaim(string claimJson, string rolesKey = "roles")
//     {
//         if (string.IsNullOrEmpty(claimJson)) return Array.Empty<string>();
//
//         try
//         {
//             using var doc = JsonDocument.Parse(claimJson);
//             if (doc.RootElement.ValueKind == JsonValueKind.Object && doc.RootElement.TryGetProperty(rolesKey, out var rolesEl))
//             {
//                 if (rolesEl.ValueKind == JsonValueKind.Array)
//                 {
//                     foreach (var r in rolesEl.EnumerateArray())
//                         return r.GetString()!;
//                 }
//             }
//         }
//         catch
//         {
//             // ignore parse errors and return none
//         }
//     }
// }
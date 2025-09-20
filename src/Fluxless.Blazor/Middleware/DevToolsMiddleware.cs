using System.Text.Json;
using Fluxless.Blazor.Core;

namespace Fluxless.Blazor.Middleware;

/// <summary>
/// Stub for devtools integration.
/// </summary>
public static class DevToolsMiddleware
{
    public static void Attach<TState>(IStore<TState> store, string name = "")
    {
        store.StateChanged += (_, args) =>
        {
            var json = JsonSerializer.Serialize(args.State);
            Console.WriteLine($"[Fluxless.DevTools:{name}] {json}");
        };
    }
}

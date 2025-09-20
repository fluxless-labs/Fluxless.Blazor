using Fluxless.Blazor.Core;

namespace Fluxless.Blazor.Middleware;

public static class LoggingMiddleware
{
    public static void Attach<TState>(IStore<TState> store, string name = "")
    {
        store.StateChanged += (_, args) =>
        {
            Console.WriteLine($"[Fluxless:{name}] State changed (Reason={args.Reason}) → {args.State}");
        };
    }
}

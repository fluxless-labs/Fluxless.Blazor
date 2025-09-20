namespace Fluxless.Blazor.Core;

/// <summary>
/// A strongly typed reactive store.
/// </summary>
public interface IStore<TState> : IDisposable
{
    /// <summary>
    /// Current state of the store.
    /// </summary>
    TState State { get; }

    /// <summary>
    /// Updates the state immutably.
    /// </summary>
    void SetState(Func<TState, TState> update, string? reason = null);

    /// <summary>
    /// Subscribe to full state changes.
    /// </summary>
    void Subscribe(Action<TState> listener);

    /// <summary>
    /// Subscribe to a selected part of state.
    /// </summary>
    void Subscribe<TSelected>(
        Func<TState, TSelected> selector,
        Action<TSelected> listener,
        int? throttleMs = null,
        int? debounceMs = null);

    /// <summary>
    /// Event fired when state changes.
    /// </summary>
    event EventHandler<StateChangedEventArgs<TState>>? StateChanged;
}

namespace Fluxless.Blazor.Core;

/// <summary>
/// Provides event data when a store's state changes.
/// Includes the new state and an optional reason for the change.
/// </summary>
/// <typeparam name="TState">The type of the store state.</typeparam>
public class StateChangedEventArgs<TState> : EventArgs
{
    /// <summary>
    /// The new state after the change.
    /// </summary>
    public TState State { get; }

    /// <summary>
    /// An optional string describing why the state changed.
    /// </summary>
    public string? Reason { get; }

    public StateChangedEventArgs(TState state, string? reason)
    {
        State = state;
        Reason = reason;
    }
}

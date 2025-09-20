namespace Fluxless.Blazor.Core;

/// <summary>
/// Base store implementation.
/// </summary>
public class Store<TState>(TState initial) : IStore<TState>
{
    private TState _state = initial;
    private readonly List<Action<TState>> _listeners = new();
    private readonly List<SelectorSubscription<TState>> _selectorListeners = new();

    public event EventHandler<StateChangedEventArgs<TState>>? StateChanged;

    public TState State => _state;

    public void SetState(Func<TState, TState> update, string? reason = null)
    {
        var newState = update(_state);
        if (EqualityComparer<TState>.Default.Equals(newState, _state))
            return;

        _state = newState;

        foreach (var listener in _listeners)
            listener(_state);

        foreach (var sub in _selectorListeners)
            sub.TryNotify(_state);

        StateChanged?.Invoke(this, new StateChangedEventArgs<TState>(_state, reason));
    }

    public void Subscribe(Action<TState> listener) => _listeners.Add(listener);

    public void Subscribe<TSelected>(
        Func<TState, TSelected> selector,
        Action<TSelected> listener,
        int? throttleMs = null,
        int? debounceMs = null)
    {
        var sub = new SelectorSubscription<TState>(s => selector(s)!, o => listener((TSelected)o!));

        if (throttleMs.HasValue)
            sub.UseThrottle(throttleMs.Value);
        if (debounceMs.HasValue)
            sub.UseDebounce(debounceMs.Value);

        _selectorListeners.Add(sub);
    }

    public void Batch(Action<Store<TState>> batchAction, string? reason = null)
    {
        var oldState = _state;
        batchAction(this);
        if (!EqualityComparer<TState>.Default.Equals(oldState, _state))
        {
            StateChanged?.Invoke(this, new StateChangedEventArgs<TState>(_state, reason ?? "Batch"));
        }
    }

    public void Dispose()
    {
        _listeners.Clear();
        _selectorListeners.Clear();
        StateChanged = null;
    }
}

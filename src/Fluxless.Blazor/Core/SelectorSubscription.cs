namespace Fluxless.Blazor.Core;

/// <summary>
/// Holds a selector subscription with optional throttling/debounce.
/// </summary>
public class SelectorSubscription<TState>
{
    public Func<TState, object?> Selector { get; }
    public Action<object?> Listener { get; }
    public object? Last { get; private set; }

    private int? _throttleMs;
    private int? _debounceMs;
    private DateTime _lastNotified = DateTime.MinValue;
    private CancellationTokenSource? _debounceCts;

    public SelectorSubscription(Func<TState, object?> selector, Action<object?> listener)
    {
        Selector = selector;
        Listener = listener;
        Last = default;
    }

    public void TryNotify(TState state)
    {
        var newVal = Selector(state);
        if (Equals(newVal, Last)) return;

        if (_throttleMs.HasValue &&
            (DateTime.UtcNow - _lastNotified).TotalMilliseconds < _throttleMs.Value)
            return;

        if (_debounceMs.HasValue)
        {
            _debounceCts?.Cancel();
            _debounceCts = new CancellationTokenSource();
            var token = _debounceCts.Token;
            Task.Delay(_debounceMs.Value, token).ContinueWith(t =>
            {
                if (!t.IsCanceled)
                {
                    Last = newVal;
                    Listener(newVal);
                    _lastNotified = DateTime.UtcNow;
                }
            });
            return;
        }

        Last = newVal;
        Listener(newVal);
        _lastNotified = DateTime.UtcNow;
    }

    public void UseThrottle(int ms) => _throttleMs = ms;
    public void UseDebounce(int ms) => _debounceMs = ms;
}

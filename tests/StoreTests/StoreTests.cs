using Fluxless.Blazor.Core;

namespace StoreTests;

public record TestState(int Value);

[TestFixture]
public class StoreTests
{
    [Test]
    public void SetState_ShouldUpdateValue()
    {
        var store = new Store<TestState>(new TestState(0));
        store.SetState(s => s with { Value = 42 });
        Assert.That(store.State.Value, Is.EqualTo(42));
    }

    [Test]
    public void Subscribe_ShouldNotifyOnChange()
    {
        var store = new Store<TestState>(new TestState(0));
        var observed = 0;
        store.Subscribe(s => s.Value, v => observed = v);
        store.SetState(s => s with { Value = 99 });
        Assert.That(observed, Is.EqualTo(99));
    }
}

// Lo que ven los demás (Player, UI, etc.)
public interface IReadOnlyReactive<T>
{
    T Value { get; }
    void Subscribe(System.Action<T> subscriber);
    void Unsubscribe(System.Action<T> subscriber);
}
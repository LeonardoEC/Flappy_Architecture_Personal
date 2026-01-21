using System;

public class Evento_Reactivo<T>
{
    private T _value;
    private Action<T> _onValueChanged;

    public T Value
    {
        get => _value;
        set
        {
            // Opcional: Solo disparar si el valor es DISTINTO
            if (Object.Equals(_value, value)) return;
            _value = value;
            _onValueChanged?.Invoke(_value);
        }
    }

    public void Subscribe(Action<T> subscriber)
    {
        _onValueChanged += subscriber;
        subscriber?.Invoke(_value);
    }

    public void Unsubscribe(Action<T> subscriber)
    {
        _onValueChanged -= subscriber;
    }
}
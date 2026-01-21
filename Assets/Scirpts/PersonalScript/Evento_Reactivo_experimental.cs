using System;

public class Evento_Reactivo_experimental<T>
{
    private event Action<T> _onValueChanged;
    private T _value;

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

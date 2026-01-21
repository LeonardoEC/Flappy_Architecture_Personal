using System;

public class ReadOnlyReactiveProperty<T>
{
    private T _value;
    private bool _isAssigned = false; // El cerrojo
    private Action<T> _onValueChanged;

    public T Value
    {
        get => _value;
        set
        {
            // Si ya fue asignado, ignoramos cualquier intento de cambio
            if (_isAssigned) return;

            _value = value;
            _isAssigned = true; // Cerramos el candado para siempre
            _onValueChanged?.Invoke(_value);
        }
    }

    public void Subscribe(Action<T> subscriber)
    {
        _onValueChanged += subscriber;
        // Si ya tenemos el valor definitivo, se lo entregamos al nuevo suscriptor
        if (_isAssigned) subscriber?.Invoke(_value);
    }
}

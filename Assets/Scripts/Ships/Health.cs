using System;
using UnityEngine;

[Serializable]
public class Health
{
    public Action<int> Changed;
    public Action Died;

    [SerializeField] private int _value;

    public int Value => _value;

    public Health(int value) => _value = value;

    public void Reduce()
    {
        //if(value < 0)
        //{
        //    throw new ArgumentException(nameof(value));
        //}
        _value--;

        if(_value == 0)
        {
            Died?.Invoke();
            Debug.Log("VALUE = 0");
        }
    }
}

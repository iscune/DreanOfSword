using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Energy {
    int _value;

    Slider _bar;
    public int Value
    {
        get { return _value; }
        set
        {
            _value = _value + value;

            _value = _value > 100 ? 100 : _value;
            _value = _value < 0 ? 0 : _value;
            _bar.value = _value; 
         }
    }

    public Energy(GameObject bar)
    {
        _bar = bar.GetComponent<Slider>();
        Value = 0;
        
    }
}

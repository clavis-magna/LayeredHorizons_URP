using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderComponent : MonoBehaviour
{
    public float value;
    public string sliderName;

    private float increment = 0.1f;

    void Update()
    {
        value = Mathf.Round(value * 100f) / 100f;

    }

    public void IncreaseValue()
    {
        value += increment;
        if (value > 1)
        {
            value = 1;
        }
    }

    public void DecreaseValue()
    {
        value -= increment;
        if (value < 0)
        {
            value = 0;
        }
    }

    public void DefineValue(float newValue)
    {
        value = newValue;
        if (value > 1)
        {
            value = 1;
        }
        if (value < 0)
        {
            value = 0;
        }
    }

}

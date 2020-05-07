using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public float MaxValue = 100;

    void Start() {
        slider.value = MaxValue;
    }

    public void LowerHealth(float damageTaken){
        slider.value -= damageTaken;
        SetBarValue(slider.value);
    }

    public void SetBarValue(float newValue){
        slider.value = newValue;
    }

    public void SetMaxBarValue(float newMaxValue){
        slider.maxValue = newMaxValue;
        slider.value = newMaxValue;
    }
}

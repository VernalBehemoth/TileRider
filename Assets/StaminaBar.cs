using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;

    public void SetBarValue(float newValue){
        if(slider != null)
        slider.value = newValue;
    }

    public void SetMaxBarValue(float newMaxValue){
        slider.maxValue = newMaxValue;
        slider.value = newMaxValue;
    }
}

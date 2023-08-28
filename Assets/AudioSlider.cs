using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AudioSlider : MonoBehaviour
{
    public Slider max, min;
    Slider baseSlider;

    void Awake()
    {
        baseSlider = GetComponent<Slider>();
    }

    public void OnMaxChange(float value){
        if (min.value > value){
            min.value = value;
        }
    }
    public void OnMinChange(float value){
        if (max.value < value){
            max.value = value;
        }
    }

    public void SetValue(float value){
        baseSlider.value = value;
    }
}

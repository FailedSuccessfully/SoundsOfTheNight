using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class AudioSlider : MonoBehaviour
{
    public Slider max, min;
    Slider baseSlider;
    TextMeshProUGUI rawLable, minLable, maxLable;
    public float rawValue = 0;
    void Awake()
    {
        baseSlider = GetComponent<Slider>();
        rawLable = GetComponentInChildren<TextMeshProUGUI>();
        minLable = min.GetComponentInChildren<TextMeshProUGUI>();
        maxLable = max.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        OnMaxChange(max.value);
        OnMinChange(min.value);
    }

    public void OnMaxChange(float value){
        if (min.value > value){
            min.value = value;
        }
        maxLable.text = $"{value * 100:n0}%";
    }
    public void OnMinChange(float value){
        if (max.value < value){
            max.value = value;
        }
        minLable.text = $"{value * 100:n0}%";
    }

    public void SetValue(float value){
        baseSlider.value = value;
    }

    public void SetRawValue(float value) {
        rawValue = value;
        rawLable.text = $"{value * 100:n0}%";
    }
}

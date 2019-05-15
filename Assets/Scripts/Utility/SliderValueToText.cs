using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueToText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Slider slider;
    

    public void UpdateTextWithValue()
    {
        text.text = slider.value.ToString();
    }
}

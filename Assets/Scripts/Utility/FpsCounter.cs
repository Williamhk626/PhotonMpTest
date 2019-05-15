using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class FpsCounter : MonoBehaviour
{
    public TextMeshProUGUI counter;
    public float UpdateRate = 1f;
    int frameCount;
    float dt;
    float fps;

    void Update()
    {
        frameCount++;
        dt += Time.unscaledDeltaTime;
        if (dt > 1.0 / UpdateRate)
        {
            fps = frameCount / dt;
            frameCount = 0;
            dt -= 1f / UpdateRate;
        }
        counter.text = $"{Math.Round(fps)} FPS";
    }
}

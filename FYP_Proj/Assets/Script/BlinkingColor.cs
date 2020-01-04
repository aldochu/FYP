using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingColor : MonoBehaviour
{
    // Start is called before the first frame update
    public Color colorStart;
    Color colorEnd = Color.white;

    float duration = 1.0f;
    Renderer rend;

    void Start()
    {
        colorStart.a = 0.5f;
        colorEnd.a = 0; //transparent

        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        float lerp = Mathf.PingPong(Time.time, duration) / duration;
        rend.material.color = Color.Lerp(colorStart, colorEnd, lerp);
    }
}

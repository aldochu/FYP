using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PriceText : MonoBehaviour
{
    private float paid;
    private bool displayed;
    public Text priceText;
    // Start is called before the first frame update
    void Start()
    {
        paid = 0.0f;
        displayed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!displayed)
        {
            priceText.text = "$" + paid;
            displayed = true;
        }
    }

    public void UpdateText(float newAmt)
    {
        paid = newAmt;
        displayed = false;
    }
}

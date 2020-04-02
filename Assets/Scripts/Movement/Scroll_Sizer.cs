using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll_Sizer : MonoBehaviour
{
    public float height;
    public Scrolling scrolling;

    void Update()
    {
        float oldHeight = height;
        if (oldHeight != height)
        {
            this.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -height / 2);
        }
        height = this.GetComponent<RectTransform>().rect.height;
        scrolling.UpdateLimits();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Content_Sizer : MonoBehaviour
{
    public float height;
    // Update is called once per frame
    void Update()
    {
        float oldHeight = height;
        if (oldHeight != height)
        {
            this.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -height / 2);
        }
        height = Scrolling.GetWorldRect(this.GetComponent<RectTransform>(), Vector2.one).height;
    }
}

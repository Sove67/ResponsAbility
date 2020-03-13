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
        height = GetWorldRect(this.GetComponent<RectTransform>(), Vector2.one).height;
    }

    static public Rect GetWorldRect(RectTransform rt, Vector2 scale) // Code from Ash-Blue's accepted answer at https://answers.unity.com/questions/1100493/convert-recttransformrect-to-rect-world.html
    {
        // Convert the rectangle to world corners and grab the top left
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        Vector3 topLeft = corners[0];

        // Rescale the size appropriately based on the current Canvas scale
        Vector2 scaledSize = new Vector2(scale.x * rt.rect.size.x, scale.y * rt.rect.size.y);

        return new Rect(topLeft, scaledSize);
    }
}

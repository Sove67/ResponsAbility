using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{
    // Variables
    public RectTransform mask;
    public RectTransform root;
    public RectTransform content;
    public float? oldContentHeight;
    public float? listLength;
    public float oldListLength;
    public Vector2 YLimit;
    public float YBuffer;

    public void Update()
    {
        if ((listLength != null && listLength != oldListLength) || (content != null && GetWorldRect(content, Vector2.one).height != oldContentHeight) || YLimit == null)
        {
            if (listLength != null)
            {
                YLimit.x = 0;
                YLimit.y = listLength ?? default(float); // Taken from https://stackoverflow.com/questions/5995317/how-to-convert-c-sharp-nullable-int-to-int/5995418
                oldListLength = listLength ?? default(float);
            }

            else if (content != null)
            {
                YLimit.x = -GetWorldRect(content, Vector2.one).height / 2;
                YLimit.y = (GetWorldRect(content, Vector2.one).height / 2) - (GetWorldRect(mask, Vector2.one).height) + YBuffer;
                oldContentHeight = GetWorldRect(content, Vector2.one).height;
            }

            else
            {
                Debug.LogError("No Limiting Object Was Attached To The Scrolling Script On " + this.name + ".");
                YLimit = Vector2.zero;
            }

            Reset();
        }
    }

    public void Swipe(float dist, Touch touch) // Filters input to correct root
    {
        float posMod = root.anchoredPosition.y + dist;
        if (mask.gameObject.activeSelf && GetWorldRect(mask, Vector2.one).Contains(touch.position))
        {
            if (posMod <= YLimit.x) // Lowest Limit
            { root.anchoredPosition = new Vector2(0, YLimit.x); }

            else if (posMod >= YLimit.y) // Highest Limit
            { 
                if (YLimit.x <= YLimit.y)
                { root.anchoredPosition = new Vector2(0, YLimit.y); }
                else
                { root.anchoredPosition = new Vector2(0, YLimit.x); }
            }

            else // Posible Movement
            { root.anchoredPosition = new Vector2(0, posMod); }
        }
    }

    public void Reset()
    {
        root.anchoredPosition = new Vector2(0, YLimit.x);
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

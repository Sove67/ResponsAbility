using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{
    // Variables
    public RectTransform mask;
    public RectTransform root;
    public RectTransform content;
    private float? oldContentHeight;
    public float? listLength;
    private float oldListLength;
    public Vector2 YLimit;
    public float YBuffer;

    public void UpdateLimits() // Update the limits of the titleScrolling root
    {
        if ((listLength != null && listLength != oldListLength) || (content != null && content.rect.height != oldContentHeight) || YLimit == null)
        {
            if (listLength != null) // List Root Handling
            {
                float temp = listLength ?? default; // Int? to Int conversion taken from https://stackoverflow.com/questions/5995317/how-to-convert-c-sharp-nullable-int-to-int/5995418

                YLimit.x = 0;
                YLimit.y = temp;
                oldListLength = temp;
            }

            else if (content != null) // Text Object Handling
            {
                YLimit.x = -content.rect.height / 2;
                YLimit.y = content.rect.height / 2;
                oldContentHeight = content.rect.height;
            }

            else
            {
                Debug.LogError("No Limiting Object Was Attached To The Scrolling Script On " + this.name + ".");
                YLimit = Vector2.zero;
            }
            YLimit.y += YBuffer - mask.rect.height;
            Reset();
        }
    }

    public void Swipe(float dist, Touch touch) // Filters input to correct root, and scrolls it to the new position, if it is within the limits.
    {
        float posMod = root.anchoredPosition.y + dist;

        if (mask.gameObject.activeSelf && RectTransformUtility.RectangleContainsScreenPoint(mask, touch.position, FindObjectOfType<Camera>()))
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

    public void Reset() // Moves the titleScrolling root to the default position
    {
        root.anchoredPosition = new Vector2(0, YLimit.x);
    }
}
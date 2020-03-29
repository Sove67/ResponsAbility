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
        if ((listLength != null && listLength != oldListLength) || (content != null && GetWorldRect(content, Vector2.one).height != oldContentHeight) || YLimit == null)
        {
            if (listLength != null) // List Root Handling
            {
                float temp = listLength ?? default(float); // Int? to Int conversion taken from https://stackoverflow.com/questions/5995317/how-to-convert-c-sharp-nullable-int-to-int/5995418

                YLimit.x = 0;
                YLimit.y = temp - GetWorldRect(mask, Vector2.one).height; 
                oldListLength = temp;
            }

            else if (content != null) // Text Object Handling
            {
                YLimit.x = -GetWorldRect(content, Vector2.one).height / 2;
                YLimit.y = GetWorldRect(content, Vector2.one).height / 2;
                oldContentHeight = GetWorldRect(content, Vector2.one).height;
            }

            else
            {
                Debug.LogError("No Limiting Object Was Attached To The Scrolling Script On " + this.name + ".");
                YLimit = Vector2.zero;
            }

            Reset();
        }
        YLimit.y += YBuffer;
    }

    public void Swipe(float dist, Touch touch) // Filters input to correct root, and scrolls it to the new position, if it is within the limits.
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

    public void Reset() // Moves the titleScrolling root to the default position
    {
        root.anchoredPosition = new Vector2(0, YLimit.x);
    }

    
    static public Rect GetWorldRect(RectTransform rt, Vector2 scale) // Code from Ash-Blue's accepted answer at https://answers.unity.com/questions/1100493/convert-recttransformrect-to-rect-world.html
    {
        // THIS FUNCTION NEEDS CANVAS TYPE SET TO "Screen Space - Overlay" TO FUNCTION PROPERLY

        // Convert the rectangle to world corners and grab the top left
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        Vector3 topLeft = corners[0];

        // Rescale the size appropriately based on the current Canvas scale
        Vector2 scaledSize = new Vector2(scale.x * rt.rect.size.x, scale.y * rt.rect.size.y);

        return new Rect(topLeft, scaledSize);
    }
}
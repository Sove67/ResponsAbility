using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{
    // Variables
    public RectTransform mask;
    public RectTransform root;
    public RectTransform content;
    public RectTransform scrollBar;
    private float? oldContentHeight;
    public float? listLength;
    private float oldListLength;
    public Vector2 YLimit;
    public float YBuffer;

    public float contentRange;
    public float barRange;
    public float posRatio;


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

            YLimit.y += YBuffer - mask.rect.height;
            Reset();

            if (YLimit.x <= YLimit.y)
            { scrollBar.gameObject.SetActive(true); }
            else
            { scrollBar.gameObject.SetActive(false); }
        }
    }

    public void Swipe(float dist, Touch touch) // Filters input to correct root, and scrolls it to the new position, if it is within the limits.
    {
        float newPos = root.anchoredPosition.y + dist;

        if (mask.gameObject.activeSelf && RectTransformUtility.RectangleContainsScreenPoint(mask, touch.position, FindObjectOfType<Camera>()))
        {
            Debug.Log("Scroll At: " + this.name);
            if (newPos <= YLimit.x) // Set to Lowest Limit
            { root.anchoredPosition = new Vector2(0, YLimit.x); }

            else if (newPos >= YLimit.y) // Set to Highest Limit
            { 
                if (YLimit.x <= YLimit.y)
                { root.anchoredPosition = new Vector2(0, YLimit.y); }
                else
                { root.anchoredPosition = new Vector2(0, YLimit.x); }
            }

            else // Unlimited Movement
            { root.anchoredPosition = new Vector2(0, newPos); }
        }

        if (listLength != null)
        { MoveScrollBar(root.anchoredPosition.y, false); }

        else if (content != null)
        { MoveScrollBar(root.anchoredPosition.y, true); }
    }

    public void MoveScrollBar(float yPos, bool isContent)
    {
        if (isContent)
        { yPos += content.rect.height /2; } // Offset the position by half the content height for content scrolling only

        contentRange = Mathf.Abs(YLimit.x - YLimit.y); // Get the total range of movement of the root object
        barRange = mask.rect.height - scrollBar.rect.height; // Get the distance the scroll bar can move
        posRatio = yPos / contentRange * barRange; // Scale the ratio of the root's distance to the scroll bar range
        posRatio = -(posRatio + scrollBar.rect.height/2); // Offset the pos by half the scrollbar's height and invert it

        scrollBar.anchoredPosition = new Vector2(0, posRatio);
    }

    public void Reset() // Moves the titleScrolling root to the default position
    {
        root.anchoredPosition = new Vector2(0, YLimit.x);
    }
}
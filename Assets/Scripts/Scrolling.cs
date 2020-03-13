using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{
    // Variables
    public RectTransform mask;
    public RectTransform root;
    public RectTransform content = null;
    public float? listLength = null;
    public Vector2 YLimit;
    public float YBuffer;

    public void Update()
    {
        if (listLength != null)
        {
            YLimit.x = 0;
            YLimit.y = listLength ?? default(float); // Taken from https://stackoverflow.com/questions/5995317/how-to-convert-c-sharp-nullable-int-to-int/5995418
        }

        else if (content != null)
        {
            YLimit.x = -content.rect.height /2;
            YLimit.y = content.anchoredPosition.y;
        }
        else
        {
            YLimit = Vector2.zero;
        }
    }

    public void Swipe(float dist, Touch touch) // Filters input to correct root
    {
        float posMod = root.anchoredPosition.y + dist;
        if (GetWorldRect(mask, Vector2.one).Contains(touch.position))
        {
            if (posMod <= YLimit.x) // Lowest Limit
            { root.anchoredPosition = new Vector2(0, YLimit.x); }

            else if (posMod >= YLimit.y + YBuffer) // Highest Limit
            { 
                if (YLimit.x <= YLimit.y + YBuffer)
                { root.anchoredPosition = new Vector2(0, YLimit.y + YBuffer); }
                else
                { root.anchoredPosition = new Vector2(0, YLimit.x); }
            }

            else // Posible Movement
            { root.anchoredPosition = new Vector2(0, posMod); }
            Debug.Log(root.anchoredPosition.y);
        }
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

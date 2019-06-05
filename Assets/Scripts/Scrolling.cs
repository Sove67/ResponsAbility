using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{
    public int current_layer = 1;
    public List<RectTransform> layer_1 = new List<RectTransform>();
    public List<RectTransform> layer_2 = new List<RectTransform>();
    
    public void SetLayer(int choice)
    {
        current_layer = choice;
    }

    // Handles scrolling using vertical swipes
    public void Swipe(float dist, Touch touch)
    {
        if (current_layer == 1)
        {
            // Check each RectTransform in the "areas" list for touch events, and moves it based on "Input.cs" calculations
            foreach (var rect in layer_1)
            {
                if (GetWorldRect(rect, Vector2.one).Contains(touch.position))
                { rect.Translate(0, dist, 0); }
            }
        }
        else if (current_layer == 2)
        {
            // Check each RectTransform in the "areas" list for touch events, and moves it based on "Input.cs" calculations
            foreach (var rect in layer_2)
            {
                if (GetWorldRect(rect, Vector2.one).Contains(touch.position))
                { rect.Translate(0, dist, 0); }
            }
        }
    }

    // Code taken from Ash-Blue's accepted answer at https://answers.unity.com/questions/1100493/convert-recttransformrect-to-rect-world.html to solve rects being locally sized
    static public Rect GetWorldRect(RectTransform rt, Vector2 scale)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{
    public RectTransform schedule;
    public RectTransform calendar;
    public RectTransform list_preview;
    private List<RectTransform> vertical_swipe_areas = new List<RectTransform>();

    // Initializes a couple variables
    void Start()
    {
        vertical_swipe_areas.Add(schedule);
        vertical_swipe_areas.Add(calendar);
        vertical_swipe_areas.Add(list_preview);
    }

    // Handles scrolling using vertical swipes
    public void Swipe(float dist, Touch touch)
    {
        foreach (var rect_transform in vertical_swipe_areas)
        {
            if (GetWorldRect(rect_transform, Vector2.one).Contains(touch.position))
            {
                //Debug.Log("Moved " + rect_transform.name + " " + dist + "Units.");
                rect_transform.Translate(0, dist, 0);
            }
        }
    }

    // Code taken from Ash-Blue's accepted answer at https://answers.unity.com/questions/1100493/convert-recttransformrect-to-rect-world.html
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

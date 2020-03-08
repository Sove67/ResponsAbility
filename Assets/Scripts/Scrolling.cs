using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{
    //Variables
    public List<ScrollMask> sectors = new List<ScrollMask>();
    public List<RectTransform> masks = new List<RectTransform>();
    public List<RectTransform> roots = new List<RectTransform>();

    // Classes
    public class ScrollMask
    {
        public RectTransform mask { get; set; }
        public RectTransform root { get; set; }
        public float limitY { get; set; }
        public ScrollMask(RectTransform mask, RectTransform root, float limitY)
        {
            this.mask = mask;
            this.root = root;
            this.limitY = limitY;
        }
    }

    // Functions
    public void Start() // Assigns variables to main list
    {
        for (int i = 0; i < masks.Count; i++)
        {
            sectors.Add(new ScrollMask(masks[i], roots[i], 0));
        }
    }

    public void Swipe(float dist, Touch touch) // Filters input to correct root
    {
        for (int i = 0; i < sectors.Count; i++)
        {
            if (GetWorldRect(sectors[i].mask, Vector2.one).Contains(touch.position))
            {
                if (sectors[i].root.anchoredPosition.y + dist <= 0)
                {
                    Debug.Log("A");
                    sectors[i].root.anchoredPosition = new Vector2(0, 0);
                }
                else if (sectors[i].root.anchoredPosition.y + dist >= sectors[i].limitY)
                {
                    Debug.Log("B");
                    sectors[i].root.anchoredPosition = new Vector2(0, sectors[i].limitY);
                }
                else
                {
                    Debug.Log("C");
                    sectors[i].root.anchoredPosition = new Vector2(0, sectors[i].root.anchoredPosition.y + dist);
                }
                Debug.Log(sectors[i].root.anchoredPosition.y);
            }
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

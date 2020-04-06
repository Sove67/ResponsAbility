using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input : MonoBehaviour
{
    // The following is a modified version of Programmer's accepted solution at https://stackoverflow.com/questions/41491765/detect-swipe-gesture-direction
    private Vector2 finger_last;
    private Vector2 finger_now;

    public int touchMod;
    public Transition Transition;
    public List<Scrolling> Scrolling;

    void Update()// Seperates Touch inputs
    {
        foreach (Touch touch in UnityEngine.Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                finger_last = touch.position;
                finger_now = touch.position;
            }
            
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Ended)
            {
                finger_now = touch.position;

                // Waits until the input has moved more than the threshold, and then continues until the touch event ends
                CheckSwipe(touch);
            }
        }
    }

    void CheckSwipe(Touch touch)// Organise each touch input into horizontal or vertical, and send the swipe's distance to the corresponding function
    {
        float horizontal_dist = 0;
        float vertical_dist = 0;

        // Horizontal Swipe
        if (Mathf.Abs(finger_now.x - finger_last.x) > Mathf.Abs(finger_now.y - finger_last.y))
        { horizontal_dist = (finger_now.x - finger_last.x); }

        // Vertical Swipe
        else if (Mathf.Abs(finger_now.y - finger_last.y) > Mathf.Abs(finger_now.x - finger_last.x))
        { vertical_dist = (finger_now.y - finger_last.y); }

        // Scales Input to Screen Size
        horizontal_dist = horizontal_dist / Screen.width * touchMod;
        vertical_dist = vertical_dist / Screen.height * touchMod;

        // Sends Values to their handlers
        Transition.Swipe(horizontal_dist, touch);
        foreach (var script in Scrolling)
        {
            script.Swipe(vertical_dist, touch);
        }

        // Updates touch position
        finger_last = finger_now;
    }
}
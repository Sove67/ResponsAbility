using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input : MonoBehaviour
{
    // The following is a modified version of Programmer's accepted solution at https://stackoverflow.com/questions/41491765/detect-swipe-gesture-direction

    private Vector2 finger_init;
    private Vector2 finger_last;
    private Vector2 finger_now;
    private bool passed_threshold;
    public float swipe_threshold = 20f;

    void Update()
    {
        foreach (Touch touch in UnityEngine.Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                finger_init = touch.position;
                finger_last = touch.position;
                finger_now = touch.position;
            }

            // Detects Swipe while finger is moving
            if (touch.phase == TouchPhase.Moved)
            {
                finger_now = touch.position;

                // Waits until the input has oved more than the threshold, and then continues until the touch event ends
                if ((finger_now - finger_init).magnitude > swipe_threshold || passed_threshold)
                { passed_threshold = true; CheckSwipe(); }
            }

            // Detects swipe when finger is released
            if (touch.phase == TouchPhase.Ended)
            {
                finger_now = touch.position;
                CheckTap(finger_init);
                passed_threshold = false;
            }
        }
    }

    // This function organises the touch input into the cardinal directions, and sends the distance of the swipe to the corresponding function
    void CheckSwipe()
    {
        // Vertical Swipe
        if (Mathf.Abs(finger_now.y - finger_last.y) > Mathf.Abs(finger_now.x - finger_last.x))
        {
            //Debug.Log("Vertical Swipe Of: " + (finger_now.y - finger_last.y) + "Units");
        }

        // Horizontal Swipe
        else if (Mathf.Abs(finger_now.x - finger_last.x) > Mathf.Abs(finger_now.y - finger_last.y))
        {
            //Left Swipe
            if (finger_now.x - finger_last.x < 0)
            {
                //Debug.Log("Left Swipe Of: " + Mathf.Abs(finger_now.x - finger_last.x) + "Units"); 
            }

            //Right Swipe
            else if (finger_now.x - finger_last.x > 0)
            { 
                //Debug.Log("Right Swipe Of: " + Mathf.Abs(finger_now.x - finger_last.x) + "Units"); 
            }
        }

        finger_last = finger_now;
    }

    // Checks to see if the distance the touch input moved is greater than the threshold, and sends a tap input if so.
    void CheckTap(Vector2 init_pos)
    {
        if ((finger_now - init_pos).magnitude < swipe_threshold)
        {
            //Debug.Log("Tap"); 
        }
    }
}
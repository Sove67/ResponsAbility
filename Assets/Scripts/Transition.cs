using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public RectTransform background;
    public int transition_threshold;
    public int transition_distance;
    private string transition_state;

    public int revert_speed;
    public float target = .001f;
    private Vector2 old_pos;
    private Quaternion old_rot;
    private bool can_move_left = true;
    private bool can_move_right = true;

    // Initializes a couple variables
    private void Start()
    {
        old_pos = background.position;
        old_rot = background.rotation;
    }

    // Handles transitions using horizontal swipes
    public void Swipe(float dist, Touch touch)
    {
        background.Translate(dist, 0, 0);

        if (touch.phase == TouchPhase.Ended)
        {
            // Left Position && Right Swipe
            if (!can_move_left && can_move_right && old_pos.x - background.position.x < -transition_threshold)
            {
                background.SetPositionAndRotation(new Vector2(old_pos.x + transition_distance, background.position.y), background.rotation);
                old_pos = background.position;
                old_rot = background.rotation;
                can_move_left = true;
            }

            // Middle Position
            else if (can_move_left && can_move_right)
            {
                // Right Swipe
                if (old_pos.x - background.position.x < -transition_threshold)
                {
                    background.SetPositionAndRotation(new Vector2(old_pos.x + transition_distance, background.position.y), background.rotation);
                    old_pos = background.position;
                    old_rot = background.rotation;
                    can_move_right = false;
                }

                // Left Swipe
                else if (old_pos.x - background.position.x > transition_threshold)
                {
                    background.SetPositionAndRotation(new Vector2(old_pos.x - transition_distance, background.position.y), background.rotation);
                    old_pos = background.position;
                    old_rot = background.rotation;
                    can_move_left = false;
                }
            }

            // Right Position && Left Swipe
            else if (can_move_left && !can_move_right && old_pos.x - background.position.x > transition_threshold)
            {
                background.SetPositionAndRotation(new Vector2(old_pos.x - transition_distance, background.position.y), background.rotation);
                old_pos = background.position;
                old_rot = background.rotation;
                can_move_right = true;
            }

            if (old_pos.x - background.position.x > -transition_threshold && old_pos.x - background.position.x < transition_threshold)
            { StartCoroutine(ReturnToPos()); }
        }
    }

    // This function loops when called, gradually bringing the background back to one of 3 centered position before returning a "yield break".
    private IEnumerator ReturnToPos()
    {
        float distance = old_pos.x - background.position.x;

        if (distance == 0)
        { yield break; }

        yield return new WaitForSeconds(target);
        if (distance < 0)
        {
            if (distance + revert_speed <= 0)
            { background.Translate(-revert_speed, 0, 0); }
            
            else if (distance != 0)
            { background.Translate(distance, 0, 0); }
        }

        else if (distance > 0)
        {
            if (distance - revert_speed >= 0)
            { background.Translate(revert_speed, 0, 0); }

            else if (distance != 0)
            { background.Translate(distance, 0, 0); }
        }

        StartCoroutine(ReturnToPos());
    }
}

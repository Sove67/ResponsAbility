using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public RectTransform background;
    public int transition_threshold;
    public int transition_distance;
    private string transition_state;

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

            // Input less than required
            if (old_pos.x - background.position.x > -transition_threshold && old_pos.x - background.position.x < transition_threshold)
            { background.SetPositionAndRotation(old_pos, old_rot); }

            else
            { background.SetPositionAndRotation(old_pos, old_rot); }
        }
    }
}

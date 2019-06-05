using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public RectTransform ui_container;
    public int transition_threshold;
    public RectTransform transition_distance_reference;
    private string transition_state;

    public bool screen_lock;
    public int revert_speed;
    private Vector2 old_pos;
    private Quaternion old_rot;
    private bool can_move_left = true;
    private bool can_move_right = true;

    // Initializes a couple variables
    private void Start()
    {
        old_pos = ui_container.position;
        old_rot = ui_container.rotation;
    }

    public void SetLock(bool choice)
    { screen_lock = choice; }

    // Handles transitions using horizontal swipes
    public void Swipe(float dist, Touch touch)
    {
        if (!screen_lock)
        {
            ui_container.Translate(dist, 0, 0);

            if (touch.phase == TouchPhase.Ended)
            {
                // Left Position && Right Swipe
                if (!can_move_left && can_move_right && old_pos.x - ui_container.position.x < -transition_threshold)
                {
                    ui_container.SetPositionAndRotation(new Vector2(old_pos.x + GetWorldRect(transition_distance_reference, Vector2.one).width, ui_container.position.y), ui_container.rotation);
                    old_pos = ui_container.position;
                    old_rot = ui_container.rotation;
                    can_move_left = true;
                }

                // Middle Position
                else if (can_move_left && can_move_right)
                {
                    // Right Swipe
                    if (old_pos.x - ui_container.position.x < -transition_threshold)
                    {
                        ui_container.SetPositionAndRotation(new Vector2(old_pos.x + GetWorldRect(transition_distance_reference, Vector2.one).width, ui_container.position.y), ui_container.rotation);
                        old_pos = ui_container.position;
                        old_rot = ui_container.rotation;
                        can_move_right = false;
                    }

                    // Left Swipe
                    else if (old_pos.x - ui_container.position.x > transition_threshold)
                    {
                        ui_container.SetPositionAndRotation(new Vector2(old_pos.x - GetWorldRect(transition_distance_reference, Vector2.one).width, ui_container.position.y), ui_container.rotation);
                        old_pos = ui_container.position;
                        old_rot = ui_container.rotation;
                        can_move_left = false;
                    }
                }

                // Right Position && Left Swipe
                else if (can_move_left && !can_move_right && old_pos.x - ui_container.position.x > transition_threshold)
                {
                    ui_container.SetPositionAndRotation(new Vector2(old_pos.x - GetWorldRect(transition_distance_reference, Vector2.one).width, ui_container.position.y), ui_container.rotation);
                    old_pos = ui_container.position;
                    old_rot = ui_container.rotation;
                    can_move_right = true;
                }

                if (old_pos.x - ui_container.position.x > -transition_threshold && old_pos.x - ui_container.position.x < transition_threshold)
                { StartCoroutine(ReturnToPos()); }

                else
                { StartCoroutine(ReturnToPos()); }
            }
        }
    }

    // This function loops when called, gradually bringing the ui_container back to one of 3 centered position before returning a "yield break".
    private IEnumerator ReturnToPos()
    {
        float distance = old_pos.x - ui_container.position.x;

        if (distance == 0)
        { yield break; }

        yield return 0;

        if (distance < 0)
        {
            if (distance + revert_speed <= 0)
            { ui_container.Translate(-revert_speed, 0, 0); }
            
            else if (distance != 0)
            { ui_container.Translate(distance, 0, 0); }
        }

        else if (distance > 0)
        {
            if (distance - revert_speed >= 0)
            { ui_container.Translate(revert_speed, 0, 0); }

            else if (distance != 0)
            { ui_container.Translate(distance, 0, 0); }
        }

        StartCoroutine(ReturnToPos());
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

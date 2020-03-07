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
    private int page;

    // Initializes a couple variables
    private void Start()
    {
        old_pos = ui_container.position;
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
                if (page == 1)
                {
                    //Swipe Right
                    if (old_pos.x - ui_container.position.x > transition_threshold)
                    {
                        ui_container.SetPositionAndRotation(new Vector2(old_pos.x + GetWorldRect(transition_distance_reference, Vector2.one).width, ui_container.position.y), new Quaternion());
                        old_pos = ui_container.position;
                        page = 2;
                    }
                }
                else if (page == 2)
                {
                    //Swipe Right
                    if (old_pos.x - ui_container.position.x > transition_threshold)
                    {

                        ui_container.SetPositionAndRotation(new Vector2(old_pos.x + GetWorldRect(transition_distance_reference, Vector2.one).width, ui_container.position.y), new Quaternion());
                        old_pos = ui_container.position;
                        page = 3;
                    }
                    //Swipe Left
                    if (old_pos.x - ui_container.position.x < transition_threshold)
                    {

                        ui_container.SetPositionAndRotation(new Vector2(old_pos.x + GetWorldRect(transition_distance_reference, Vector2.one).width, ui_container.position.y), new Quaternion());
                        old_pos = ui_container.position;
                        page = 1;
                    }
                }
                else if (page == 3)
                {
                    //Swipe Left
                    if (old_pos.x - ui_container.position.x < transition_threshold)
                    {

                        ui_container.SetPositionAndRotation(new Vector2(old_pos.x + GetWorldRect(transition_distance_reference, Vector2.one).width, ui_container.position.y), new Quaternion());
                        old_pos = ui_container.position;
                        page = 2;
                    }
                }

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

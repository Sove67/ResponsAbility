using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schedule_Renderer : MonoBehaviour
{
    public List<Schedule_Populator.ItemCreate> items;
    public int scroll_input;

    private void Start()
    {
        GameObject container = GameObject.Find("Script Container");
        Schedule_Populator schedule_populator = container.GetComponent<Schedule_Populator>();
        items = schedule_populator.current_schedule;
    }

    void Update()
    {
        RenderSchedule(items);
        Scroll(scroll_input);
    }

    // Create each of the items as a combination of U.I. objects
    private void RenderSchedule(List<Schedule_Populator.ItemCreate> items)
    {
        //Render the schedule in order of start time
        foreach (var item in items)
        {
            var hour = int.Parse(item.beginning.ToString("HH"));
            var minute = int.Parse(item.beginning.ToString("mm"));

            Debug.Log("Name: " + item + "Hour: " + hour + "Minute: " + minute);
        }
        //Add "see tomorrow" button at bottom
    }
    
    // Move the entire list using input in a distance calculation
    private void Scroll(int input)
    {

    }
}
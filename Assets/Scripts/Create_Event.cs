using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Create_Event : MonoBehaviour
{   
    // Event Variables
    public Text title;
    public int icon;
    public Color colour;

    public System.DateTime date;
    public System.DateTime beginning;
    public System.TimeSpan duration;
    public int interval;

    public Toggle task_reset;
    public List<string> child_tasks;

    public Toggle calendar;

    // Display Assets
    public Image display_icon;
    public Text display_date;
    public Text display_beginning;
    public Text display_duration;
    public Text display_interval;

    // Outside Script Defining
    public GameObject global_script_container;
    private Schedule_Populator schedule_populator;
    public GameObject pop_ups_container;
    private Pop_Ups pop_ups;

    private void Start()
    {
        schedule_populator = global_script_container.GetComponent<Schedule_Populator>();
        pop_ups = pop_ups_container.GetComponent<Pop_Ups>();
    }

    //These dont sync
    public void SyncValue(string choice)
    {
        if (choice == "all")
        {
            icon = pop_ups.icon;
            date = pop_ups.date;
            beginning = pop_ups.beginning;
            duration = pop_ups.duration;
            interval = pop_ups.interval;
        }

        if (choice == "icon")
        { icon = pop_ups.icon; }

        if (choice == "date")
        {
            date = pop_ups.date;
            display_date.text = date.ToString("ddd, MMM d, yyyy");
        }

        if (choice == "beginning")
        {
            beginning = pop_ups.beginning;
            display_beginning.text = beginning.ToString("h:mm tt");
        }

        if (choice == "duration")
        {
            duration = pop_ups.duration;
            display_duration.text = (duration.ToString("%h") + " Hour(s), " + duration.ToString("%m") + " Minute(s)");
        }

        if (choice == "interval")
        {
            interval = pop_ups.interval;
            display_interval.text = (interval.ToString() + " Day(s)");
        }
    }

    // neither does this
    public void ChangeColour()
    { colour = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color; }

    public void PackageEvent()
    {
        schedule_populator.master_schedule.Add(new Schedule_Populator.ItemCreate(title.text, icon, colour, beginning, date, duration, interval, child_tasks, calendar.isOn, task_reset.isOn));
        schedule_populator.PopulateDay();
        schedule_populator.DebugSchedule(schedule_populator.master_schedule);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schedule_Populator : MonoBehaviour
{
    /*      
     *      Master Populator Syntax
     *  master_schedule.Add(new ItemCreate( VALUES ));
     *  
     *      Unique List Index
     *  List<Master_Schedule_Populator.ItemCreate> NAME
     *  
     *      Call "PopulateDay()" every time the current day's schedule is changed.
     */

    public List<ItemCreate> master_schedule = new List<ItemCreate>();
    public List<ItemCreate> current_schedule = new List<ItemCreate>();

    // This class and its functon are used to create a schedule item.
    public class ItemCreate
    {
        public string name { get; set; }
        public System.DateTime next_appearance { get; set; }
        public System.DateTime start { get; set; }
        public System.TimeSpan duration { get; set; }
        public int interval { get; set; }
        public int icon { get; set; }
        public List<string> children { get; set; }
        public bool reset { get; set; }
        public bool calendar { get; set; }

        public ItemCreate(string name, System.DateTime next_appearance, System.DateTime start, System.TimeSpan duration, int interval, int icon, List<string> children, bool reset, bool calendar)
        {
            this.name = name;
            this.next_appearance = next_appearance;
            this.start = start;
            this.duration = duration;
            this.interval = interval;
            this.icon = icon;
            this.children = children;
            this.reset = reset;
            this.calendar = calendar;
        }
    }

    // This function checks which items in the master schedule occur on the current date
    public void PopulateDay()
    {
        current_schedule.Clear();
        foreach (var item in master_schedule)
        {
            if (item.next_appearance.Date == System.DateTime.Today)
            {
                current_schedule.Add(item);
                Debug.Log("Current Schedule:");
                DebugSchedule(current_schedule);
            }
        }
    }

    // Prints each part of the selected schedule to console
    public void DebugSchedule(List<ItemCreate> schedule)
    {
        foreach (var component in schedule)
        {
            Debug.Log("Name: " + component.name);
            Debug.Log("Start: " + component.start.ToString("t"));
            Debug.Log("Duration: " + component.duration.ToString());
            Debug.Log("Next Appearance: " + component.next_appearance.ToString("D"));
            Debug.Log("Interval: " + component.interval.ToString() + " Day(s)");
            Debug.Log("Icon Index: " + component.icon);
            if (component.children != null)
            {
                Debug.Log(component.name + " Child Tasks:");
                foreach (var child in component.children) { Debug.Log(child); }
            }
            Debug.Log("Is Shown On Calendar: " + component.calendar);
            Debug.Log("Resets: " + component.reset);
        }
    }
}

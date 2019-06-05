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
        public string title { get; set; }
        public int icon { get; set; }
        public Color colour { get; set; }
        public System.DateTime next_appearance { get; set; }
        public System.DateTime beginning { get; set; }
        public System.TimeSpan duration { get; set; }
        public int interval { get; set; }
        public List<string> children { get; set; }
        public bool reset { get; set; }
        public bool calendar { get; set; }

        public ItemCreate(string title, int icon, Color colour, System.DateTime next_appearance, System.DateTime beginning, System.TimeSpan duration, int interval, List<string> children, bool reset, bool calendar)
        {
            this.title = title;
            this.icon = icon;
            this.colour = colour;
            this.next_appearance = next_appearance;
            this.beginning = beginning;
            this.duration = duration;
            this.interval = interval;
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
        Debug.Log("Debugging Schedule:");

        foreach (var task in schedule)
        {
            Debug.Log("Title: " + task.title);
            Debug.Log("Icon Index: " + task.icon);
            Debug.Log("Colour: " + task.colour);

            Debug.Log(" ");
            Debug.Log("Next Appearance (Date): " + task.next_appearance.ToString("D"));
            Debug.Log("Beginning: " + task.beginning.ToString("t"));
            Debug.Log("Duration: " + task.duration.ToString());
            Debug.Log("Interval: " + task.interval.ToString() + " Day(s)");

            Debug.Log(" ");
            if (task.children.Count != 0)
            {
                Debug.Log("'" + task.title + "'" + " Child Tasks:");
                Debug.Log("Resets: " + task.reset);
                foreach (var childtitle in task.children) { Debug.Log(childtitle); }
            }
            else
            { Debug.Log("Sub-Tasks Not Applicable"); }

            Debug.Log(" ");
            Debug.Log("Is Shown On Calendar: " + task.calendar);

            Debug.Log(" ");
        }
    }
}
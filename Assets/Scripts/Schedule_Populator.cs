using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schedule_Populator : MonoBehaviour
{
    /*      
     *      Master Populator Syntax
     *  master_schedule.Add(new ItemCreate(name, start HHMM, end HHMM, next_appearance MMDD, interval DD, is_displayed_on_calendar, children));
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
        public string next_appearance { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string interval { get; set; }
        public bool calendar { get; set; }
        public List<string> children { get; set; }

        public ItemCreate(string name, string start, string end, string next_appearance, string interval, bool calendar, List<string> children)
        {
            this.name = name;
            this.start = start;
            this.end = end;
            this.next_appearance = next_appearance;
            this.interval = interval;
            this.calendar = calendar;
            this.children = children;
        }
    }

    // This function checks which items in the master schedule occur on the current date
    public void PopulateDay()
    {
        var date = System.DateTime.Today.ToString("MMdd");
        foreach (var item in master_schedule)
        {
            if (item.next_appearance == date)
            {
                current_schedule.Add(item);
                Debug.Log("Current Schedule:");
                DebugScheduleItem(current_schedule);
            }
        }
    }

    // Prints each part of the selected schedule to console
    public void DebugScheduleItem(List<ItemCreate> schedule)
    {
        foreach (var component in schedule)
        {
            Debug.Log("Name: " + component.name);
            Debug.Log("Start: " + component.start + " (24HR Format)");
            Debug.Log("End: " + component.end + " (24HR Format)");
            Debug.Log("Next Appearance: " + component.next_appearance + " (MMDD Format)");
            Debug.Log("Interval: " + component.interval + " Days");
            Debug.Log("Is Shown On Calendar: " + component.calendar);
            if (component.children != null)
            {
                Debug.Log(component.name + " Child Tasks");
                foreach (var children in component.children) { Debug.Log(children); }
            }
        }
    }
}

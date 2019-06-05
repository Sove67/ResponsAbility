using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pop_Ups : MonoBehaviour
{
    /* Pop_Ups.cs handles any button involved, in the opening, closing, or operation of "Pop-Up Menus".
     * This covers:
     *      Icons
     *      The Date
     *      The Start of The Task
    */

    // Variables for the currently selected settings
    public int icon;
    public System.DateTime date;
    public System.DateTime beginning;
    public System.TimeSpan duration;
    public int interval;

    // Display Objects in Pop-Ups to give the variables to
    public Text date_month;
    public Text date_day;
    public Text date_year;
    public Text beginning_hour;
    public Text beginning_min;
    public Text beginning_half;
    public Text duration_hour;
    public Text duration_min;
    public Text interval_day;

    // Display Objects in Input Fields to give the variables to
    public Text date_display;
    public Text time_display;
    public Text duration_display;
    public Text interval_display;


    public GameObject create_event_container;
    private Create_Event create_event;

    // Initalizes each variable, and assigns a handle to the Create_Event.cs script, and some of it's variables
    public void Start()
    {
        create_event = create_event_container.GetComponent<Create_Event>();
        ResetValues("all");
        UpdateDisplay();
    }

    // +/- 1 from the current index based on which button was pressed
    public void Icon(int modifier)
    {
        if (!(icon + modifier < 0))
        { icon = +modifier; }
        UpdateDisplay();
    }

    // +/- 1 from month, day, or year, based on which button was pressed, then recompile the result into a DateTime 
    public void Date(string modifier)
    {
        string str = date.ToString("MMddyyyy");
        int month = int.Parse(str.Substring(0, 2));
        int day = int.Parse(str.Substring(2, 2));
        int year = int.Parse(str.Substring(4, 4));

        if (modifier.Substring(0,1) == "-") {

            if (month - int.Parse(modifier.Substring(1, 2)) >= 1)
            { month -= int.Parse(modifier.Substring(1, 2)); }
            
            if (day - int.Parse(modifier.Substring(3, 2)) >= 1)
            { day -= int.Parse(modifier.Substring(3, 2)); }

            if (year - int.Parse(modifier.Substring(5, 4)) >= 1)
            { year -= int.Parse(modifier.Substring(5, 4)); }

            string result = (month.ToString().PadLeft(2, '0') + day.ToString().PadLeft(2, '0') + year.ToString().PadLeft(4, '0'));
            date = System.DateTime.ParseExact(result, "MMddyyyy", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
        }

        else if (modifier.Substring(0, 1) == "+")
        {
            if (month + int.Parse(modifier.Substring(1, 2)) <= 12)
            { month += int.Parse(modifier.Substring(1, 2)); }

            if (day + int.Parse(modifier.Substring(3, 2)) <= System.DateTime.DaysInMonth(date.Year, date.Month))
            { day += int.Parse(modifier.Substring(3, 2)); }
            
            year += int.Parse(modifier.Substring(5, 4));

            string result = (month.ToString().PadLeft(2, '0') + day.ToString().PadLeft(2, '0') + year.ToString().PadLeft(2, '0'));
            date = System.DateTime.ParseExact(result, "MMddyyyy", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
        }
        UpdateDisplay();
    }

    // +/- 1 from hour or minute based on which button was pressed, then recompile the result into a DateTime 
    public void Beginning(string modifier)
    {
        string str = beginning.ToString("HHmm");
        int hour = int.Parse(str.Substring(0, 2));
        int minute = int.Parse(str.Substring(2, 2));

        if (modifier.Substring(0, 1) == "-")
        {
            // if the calculation doesnt make the time negative
            if (hour - int.Parse(modifier.Substring(1, 2)) >= 0)
            { hour -= int.Parse(modifier.Substring(1, 2)); }

            if (minute - int.Parse(modifier.Substring(3, 2)) >= 0)
            { minute -= int.Parse(modifier.Substring(3, 2)); }

            string result = (hour.ToString().PadLeft(2, '0') + minute.ToString().PadLeft(2, '0'));
            beginning = System.DateTime.ParseExact(result, "HHmm", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
        }

        else if (modifier.Substring(0, 1) == "+")
        {
            // if the calculation doesnt make the time overflow
            if (hour + int.Parse(modifier.Substring(1, 2)) <= 23)
            { hour += int.Parse(modifier.Substring(1, 2)); }

            if (minute + int.Parse(modifier.Substring(3, 2)) <= 59)
            { minute += int.Parse(modifier.Substring(3, 2)); }

            string result = (hour.ToString().PadLeft(2, '0') + minute.ToString().PadLeft(2, '0'));
            beginning = System.DateTime.ParseExact(result, "HHmm", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
        }
        UpdateDisplay();
    }
    
    // +/- 1 from hour or minute based on which button was pressed, then recompile the result into a TimeSpan 
    public void Duration(string modifier)
    {
        string str = duration.ToString("hhmm");
        int hour = int.Parse(str.Substring(0, 2));
        int minute = int.Parse(str.Substring(2, 2));

        if (modifier.Substring(0, 1) == "-")
        {
            // if the calculation doesnt make the time negative
            if (hour - int.Parse(modifier.Substring(1, 2)) >= 0)
            { hour -= int.Parse(modifier.Substring(1, 2)); }

            if (minute - int.Parse(modifier.Substring(3, 2)) >= 0)
            { minute -= int.Parse(modifier.Substring(3, 2)); }

            string result = (hour.ToString().PadLeft(2, '0') + minute.ToString().PadLeft(2, '0'));
            duration = System.TimeSpan.ParseExact(result, "hhmm", null, System.Globalization.TimeSpanStyles.None);
        }

        else if (modifier.Substring(0, 1) == "+")
        {
            // if the calculation doesnt make the time overflow
            if (hour + int.Parse(modifier.Substring(1, 2)) <= 24)
            { hour += int.Parse(modifier.Substring(1, 2)); }

            if (minute + int.Parse(modifier.Substring(3, 2)) <= 60)
            { minute += int.Parse(modifier.Substring(3, 2)); }

            string result = (hour.ToString().PadLeft(2, '0') + minute.ToString().PadLeft(2, '0'));
            duration = System.TimeSpan.ParseExact(result, "hhmm", null, System.Globalization.TimeSpanStyles.None);
        }
        UpdateDisplay();
    }

    // +/- 1 from the current day count based on which button was pressed
    public void Interval(int modifier)
    {
        if (!(interval + modifier < 0))
        { interval += modifier; }
        UpdateDisplay();
    }
    
    // Update the Pop-Up menu displays
    public void UpdateDisplay()
    {
        date_month.text = date.ToString("MMM");
        //The below conversion is unique because unity sees single characters as presets, not as a custom style, so a space is needed.
        //Then, after the correct string has been gotten, the space needs to be removed.
        date_day.text = date.ToString("d ").Remove(date.ToString("d ").Length - 1);
        date_year.text = date.ToString("yyyy");
        beginning_hour.text = beginning.ToString("h ").Remove(beginning.ToString("h ").Length - 1);
        beginning_min.text = beginning.ToString("mm");
        beginning_half.text = beginning.ToString("tt");
        duration_hour.text = duration.ToString("%h");
        duration_min.text = duration.ToString("mm");
        interval_day.text = (interval.ToString() + " Day(s)");
    }

    public void ResetValues(string choice)
    {
        if (choice == "all")
        {
            icon = 1;
            date = System.DateTime.Today;
            beginning = System.DateTime.Today;
            duration = System.TimeSpan.Zero;
            interval = 0;
        }

        if (choice == "icon")
        { icon = 1; }

        if (choice == "date")
        { date = System.DateTime.Today; }

        if (choice == "time")
        { beginning = System.DateTime.Now; }

        if (choice == "duration")
        { duration = System.TimeSpan.Zero; }

        if (choice == "interval")
        { interval = 0; }

        UpdateDisplay();
    }
}
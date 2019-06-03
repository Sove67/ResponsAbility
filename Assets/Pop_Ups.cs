using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pop_Ups : MonoBehaviour
{
    // Variables for the currently selected settings
    public int icon;
    public System.DateTime date;
    public System.DateTime time;
    public System.TimeSpan duration;
    public int interval;

    // Display Objects to give the variables to
    public Text date_month;
    public Text date_day;
    public Text date_year;
    public Text time_hour;
    public Text time_min;
    public Text duration_hour;
    public Text duration_min;
    public Text interval_day;

    // Initalizes each variable
    public void Start()
    {
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
        if (modifier.Substring(0,1) == "-") {
            string str = date.ToString("MMddyyyy");

            int month = int.Parse(str.Substring(0, 2)) - int.Parse(modifier.Substring(1, 2));
            int day = int.Parse(str.Substring(2, 2)) - int.Parse(modifier.Substring(3, 2));
            int year = int.Parse(str.Substring(4, 4)) - int.Parse(modifier.Substring(5, 4));

            string result = (month.ToString().PadLeft(2, '0') + day.ToString().PadLeft(2, '0') + year.ToString().PadLeft(2, '0'));
            date = System.DateTime.ParseExact(result, "MMddyyyy", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
        }

        else if (modifier.Substring(0, 1) == "+")
        {
            string str_date = date.ToString("MMddyyyy");

            int month = int.Parse(str_date.Substring(0, 2)) + int.Parse(modifier.Substring(1, 2));
            int day = int.Parse(str_date.Substring(2, 2)) + int.Parse(modifier.Substring(3, 2));
            int year = int.Parse(str_date.Substring(4, 4)) + int.Parse(modifier.Substring(5, 4));

            string result = (month.ToString().PadLeft(2, '0') + day.ToString().PadLeft(2, '0') + year.ToString().PadLeft(2, '0'));
            date = System.DateTime.ParseExact(result, "MMddyyyy", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
        }
        UpdateDisplay();
    }

    // +/- 1 from hour or minute based on which button was pressed, then recompile the result into a DateTime 
    public void Time(string modifier)
    {
        if (modifier.Substring(0, 1) == "-")
        {
            string str = time.ToString("HHmm");

            int hour = int.Parse(str.Substring(0, 2)) - int.Parse(modifier.Substring(1, 2));
            int minute = int.Parse(str.Substring(2, 2)) - int.Parse(modifier.Substring(3, 2));

            string result = (hour.ToString().PadLeft(2, '0') + minute.ToString().PadLeft(2, '0'));
            time = System.DateTime.ParseExact(result, "HHmm", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
        }
        else if (modifier.Substring(0, 1) == "+")
        {
            string str = time.ToString("HHmm");

            int hour = int.Parse(str.Substring(0, 2)) + int.Parse(modifier.Substring(1, 2));
            int minute = int.Parse(str.Substring(2, 2)) + int.Parse(modifier.Substring(3, 2));

            string result = (hour.ToString().PadLeft(2, '0') + minute.ToString().PadLeft(2, '0'));
            time = System.DateTime.ParseExact(result, "HHmm", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
        }
        UpdateDisplay();
    }

    // +/- 1 from hour or minute based on which button was pressed, then recompile the result into a TimeSpan 
    public void Duration(string modifier)
    {
        if (modifier.Substring(0, 1) == "-")
        {
            string str = duration.ToString("hhmm");

            int hour = int.Parse(str.Substring(0, 2)) - int.Parse(modifier.Substring(1, 2));
            int minute = int.Parse(str.Substring(2, 2)) - int.Parse(modifier.Substring(3, 2));

            string result = (hour.ToString().PadLeft(2, '0') + minute.ToString().PadLeft(2, '0'));
            Debug.Log(result);
            duration = System.TimeSpan.ParseExact(result, "hhmm", null, System.Globalization.TimeSpanStyles.None);
        }
        else if (modifier.Substring(0, 1) == "+")
        {
            string str = duration.ToString("hhmm");

            int hour = int.Parse(str.Substring(0, 2)) + int.Parse(modifier.Substring(1, 2));
            int minute = int.Parse(str.Substring(2, 2)) + int.Parse(modifier.Substring(3, 2));

            string result = (hour.ToString().PadLeft(2, '0') + minute.ToString().PadLeft(2, '0'));
            Debug.Log(result);
            duration = System.TimeSpan.ParseExact(result, "hhmm", null, System.Globalization.TimeSpanStyles.None);
        }
        UpdateDisplay();
    }

    public void Interval(int modifier)
    {
        if (!(interval + modifier < 0))
        { interval += modifier; }
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        date_month.text = date.ToString("MMM");
        //The below conversion is unique because unity sees single characters as presets, not as a custom style, so a space is needed.
        //Then, after the correct string has been gotten, the space needs to be removed.
        date_day.text = date.ToString("d ").Remove(date.ToString("d ").Length - 1);
        date_year.text = date.ToString("yyyy");
        time_hour.text = time.ToString("H ").Remove(time.ToString("H ").Length - 1);
        time_min.text = time.ToString("mm");
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
            time = System.DateTime.Now;
            duration = System.TimeSpan.Zero;
            interval = 0;
        }

        if (choice == "icon")
        { icon = 1; }

        if (choice == "date")
        { date = System.DateTime.Today; }

        if (choice == "time")
        { time = System.DateTime.Now; }

        if (choice == "duration")
        { duration = System.TimeSpan.Zero; }

        if (choice == "interval")
        { interval = 0; }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pop_Ups : MonoBehaviour
{
    // Variables for the currently selected settings
    public int icon;
    public System.DateTime date;
    public System.DateTime time;
    public System.TimeSpan duration;
    public int interval;

    // Initalizes each variable
    public void Start()
    {
        icon = 1;
        date = System.DateTime.Today;
        time = System.DateTime.Now;
        duration = System.TimeSpan.Zero;
        interval = 0;
    }

    // +/- 1 from the current index based on which button was pressed
    public void Icon(int modifier)
    {
        if (!(icon + modifier < 0))
        { icon = +modifier; }
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
            duration = System.TimeSpan.ParseExact(result, "hhmm", null, System.Globalization.TimeSpanStyles.None);
        }
        else if (modifier.Substring(0, 1) == "+")
        {
            string str = duration.ToString("hhmm");

            int hour = int.Parse(str.Substring(0, 2)) + int.Parse(modifier.Substring(1, 2));
            int minute = int.Parse(str.Substring(2, 2)) + int.Parse(modifier.Substring(3, 2));

            string result = (hour.ToString().PadLeft(2, '0') + minute.ToString().PadLeft(2, '0'));
            duration = System.TimeSpan.ParseExact(result, "hhmm", null, System.Globalization.TimeSpanStyles.None);
        }
    }

    public void Interval(int modifier)
    {
        if (!(interval + modifier < 0))
        { interval += modifier; }
    }
}
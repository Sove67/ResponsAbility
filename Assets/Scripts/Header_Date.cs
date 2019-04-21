using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Header_Date : MonoBehaviour
{
    public string preview_day;

    public void Update()
    {
        var sched_date = GameObject.Find("Schedule").transform.Find("Header").transform.Find("Date");
        var cal_date = GameObject.Find("Calendar").transform.Find("Header").transform.Find("Date");
        System.DateTime result;

        if (string.IsNullOrEmpty(preview_day))
        {
            sched_date.GetComponent<Text>().text = System.DateTime.Now.ToString("ddd, MMM d, h:mm");
            cal_date.GetComponent<Text>().text = System.DateTime.Now.ToString("MMMM yyyy");
        }
        
        else if (System.DateTime.TryParseExact(preview_day, "yyyyMMdd", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out result))
        {
            sched_date.GetComponent<Text>().text = result.ToString("ddd, MMM d, PREVIEW");
            cal_date.GetComponent<Text>().text = result.ToString("MMMM yyyy");
        }

        else {
            sched_date.GetComponent<Text>().text = "ERROR";
            cal_date.GetComponent<Text>().text = "ERROR";
        }
    }
}
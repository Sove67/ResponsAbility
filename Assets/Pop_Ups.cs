using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pop_Ups : MonoBehaviour
{
    public GameObject Icon;
    public GameObject Date;
    public GameObject Time;
    public GameObject Duration;
    public GameObject Interval;
    public GameObject Task;

    public void IconOpen()
    { Icon.SetActive(true); }

    public void IconClose()
    { Icon.SetActive(false); }

    public void DateOpen()
    { Date.SetActive(true); }

    public void DateClose()
    { Date.SetActive(false); }

    public void TimeOpen()
    { Time.SetActive(true); }

    public void TimeClose()
    { Time.SetActive(false); }

    public void DurationOpen()
    { Duration.SetActive(true); }

    public void DurationClose()
    { Duration.SetActive(false); }

    public void IntervalOpen()
    { Interval.SetActive(true); }

    public void IntervalClose()
    { Interval.SetActive(false); }

    public void TaskOpen()
    { Task.SetActive(true); }

    public void TaskClose()
    { Task.SetActive(false); }
}

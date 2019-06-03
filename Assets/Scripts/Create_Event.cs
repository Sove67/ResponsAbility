using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Create_Event : MonoBehaviour
{
    public Text input_name;
    public Text input_date;
    public Text input_start;
    public Text input_duration;
    public Text input_interval;
    public int input_icon;
    public List<string> child_tasks;
    public Toggle input_task_reset;
    public Toggle input_calendar;

    public GameObject global_script_container;
    private Schedule_Populator schedule_populator;
    private List<Schedule_Populator.ItemCreate> master;

    private void Start()
    {
        schedule_populator = global_script_container.GetComponent<Schedule_Populator>();
        master = schedule_populator.master_schedule;
    }

    public void ChangeColour(int colour)
    {

    }

    public void CreateEvent()
    {
        master.Add(new Schedule_Populator.ItemCreate(input_name.text, input_start.text, input_duration.text, input_date.text, input_interval.text,
                                                input_icon, child_tasks, input_calendar.isOn, input_task_reset.isOn));
        schedule_populator.PopulateDay();
        schedule_populator.DebugSchedule(master);
    }
}

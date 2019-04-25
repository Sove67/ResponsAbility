using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Create_Event : MonoBehaviour
{
    public Text input_name;
    private string input_date;
    private string input_start;
    private string input_end;
    private string input_interval;
    private int? input_icon;
    public Toggle input_completable;
    public Toggle input_calendar;
    public Toggle input_task_reset;
    public Toggle input_task_ordered;

    public Text input_task_new;
    public Transform task_root;
    public GameObject event_base;
    private List<string> input_task_list;
    
    public Text example_title;
    public Image example_icon;
    public Text example_start;
    public Text example_end;

    private Schedule_Populator schedule_populator;
    private List<Schedule_Populator.ItemCreate> master;

    private void Start()
    {
        GameObject container = GameObject.Find("Script Container");
        schedule_populator = container.GetComponent<Schedule_Populator>();
        master = schedule_populator.master_schedule;
    }

    private void Update()
    {
        SyncVariables();
    }

    void SyncVariables()
    {
        // Set these to the custom input script's variables
        input_date = null;
        input_start = null;
        input_end = null;
        input_interval = null;
        input_icon = null;
    }

    void RenderTasks()
    {

        foreach (var item in input_task_list)
        {
            // calulate these based on the # of items
            Vector2 positon = Vector2.zero;
            Quaternion rotation = Quaternion.Euler(Vector2.zero);

            var new_event = Instantiate(event_base, positon, rotation, task_root);

            // change the text to that items string
            new_event.GetComponent<Text>().text = item;

            // add task text to consistent_text (small)
        }
    }

    public void CreateEvent()
    {
        master.Add(new Schedule_Populator.ItemCreate(input_name.text, input_start, input_end, input_date, input_interval,
                                                input_icon, input_completable.isOn, input_calendar.isOn, input_task_list, 
                                                input_task_reset.isOn, input_task_ordered.isOn));
        schedule_populator.PopulateDay();
        schedule_populator.DebugSchedule(master);
    }
}

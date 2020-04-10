using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Note_Handler : MonoBehaviour
{
    // Variables
    // Note Data
    [SerializeField] public List<Note> dataList = new List<Note>();
    public List<GameObject> UIList = new List<GameObject>();
    private int selection;

    // Enviroment Data
    private int noteEditorColourIndex;
    public GameObject noteEditorColourIndicator;
    public GameObject dataListContainer;
    public GameObject titlePrefab;
    public ToggleGroup titleToggleGroup;

    // Buttons
    public Button editNote;
    public Button deleteNote;

    // Outputs To Update
    public Text noteContent;
    public InputField noteEditorTitle;
    public InputField noteEditorContent;

    // Other
    public List<Material> colourOptions;
    public Scrolling titleScrolling;
    public Scrolling contentScrolling;

    // Classes
    [Serializable] public class Note // The details that make up one note
    {
        public string title { get; set; }
        public int colour { get; set; }
        public string content { get; set; }
        public bool instantiated { get; set; }
        public Note(string title, int colour, string content,  bool instantiated)
        {
            this.title = title;
            this.colour = colour;
            this.content = content;
            this.instantiated = instantiated;
        }
    }

    // Functions
    public void Start()
    {
        // Start the scrollable areas with a maximum distance of 0
        titleScrolling.listLength = 0;
        titleScrolling.UpdateLimits();
        SetValues(-1);
        UpdateList();
    }

    public void UpdateList() // run through each note, and create/update a prefab to match it's data
    {
        int count = 0;

        foreach (var note in dataList) // Update All Cards
        {
            if (!note.instantiated) // Create Title Card.
            {
                GameObject newNoteUI = Instantiate(titlePrefab, dataListContainer.transform);
                UIList.Add(newNoteUI);
                UIList[count].GetComponent<Toggle>().group = titleToggleGroup;
                dataList[count].instantiated = true;
            }

            // Set Card Properties
            int index = count; // Seperate index for listener, otherwise it would return the latest value of count rather than time of assignment.
            float spacing = titlePrefab.GetComponent<RectTransform>().rect.height;
            UIList[count].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -spacing/2 + (count * -spacing));
            UIList[count].GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
            UIList[count].GetComponent<Toggle>().onValueChanged.AddListener((isOn) => ParseToggleToSelect(isOn, index)); // Code from https://answers.unity.com/questions/902399/addlistener-to-a-toggle.html
            UIList[count].transform.Find("Title").GetComponent<Text>().text = dataList[count].title;
            UIList[count].transform.Find("Colour Indicator").GetComponent<Image>().color = colourOptions[dataList[count].colour].color;

            count++;
        }

        // Update Scroll Limit for Titles
        titleScrolling.listLength = (count) * titlePrefab.GetComponent<RectTransform>().rect.height;
        titleScrolling.UpdateLimits();
    }

    public void ParseToggleToSelect(bool isOn, int index)
    {
        if (isOn)
        {
            SetValues(index); 
        }
        else if (!titleToggleGroup.AnyTogglesOn())
        { SetValues(-1); }
    }

    public void Select(int index)
    {
        titleToggleGroup.SetAllTogglesOff();
        UIList[index].GetComponent<Toggle>().isOn = true;
    }

    public void SetValues(int index) // Select note of index "index" from the dataList, assigning all visuals accordingly
    {
        selection = index;
        if (index == -1)
        {
            noteContent.text = "";
            noteEditorTitle.text = "";
            noteEditorContent.text = "";
            noteEditorColourIndex = 0;
            noteEditorColourIndicator.GetComponent<Image>().color = colourOptions[0].color;
            
            editNote.interactable = false;
            deleteNote.interactable = false;
        }

        else
        {
            noteContent.text = dataList[index].content;
            noteEditorTitle.text = dataList[index].title;
            noteEditorContent.text = dataList[index].content;
            noteEditorColourIndex = dataList[index].colour;
            noteEditorColourIndicator.GetComponent<Image>().color = colourOptions[dataList[index].colour].color;

            editNote.interactable = true;
            deleteNote.interactable = true;
        }

        contentScrolling.UpdateLimits();
        contentScrolling.Reset();
    }

    public void SetColour(int mod) // Move once through the choice of colours by a step size of "mod" and assign the new value to the indicator to preview
    {
        noteEditorColourIndex = (noteEditorColourIndex + mod + colourOptions.Count) % colourOptions.Count;
        noteEditorColourIndicator.GetComponent<Image>().color = colourOptions[noteEditorColourIndex].color;
    }

    public void Create() // Create a new, empty note
    {
        dataList.Add(new Note("Untitled", 0, "", false));
        UpdateList();
        Select(dataList.Count - 1);
    }

    public void Save() // Save all changes to the selected note
    {
        if (noteEditorTitle.text == "")
        {
            dataList[selection] = new Note("Untitled", noteEditorColourIndex, noteEditorContent.text, dataList[selection].instantiated);
        }
        else
        {
            dataList[selection] = new Note(noteEditorTitle.text, noteEditorColourIndex, noteEditorContent.text, dataList[selection].instantiated);
        }
        UpdateList();
        Select(selection);
    }

    public void Delete() // Delete the note's info and prefab
    {
        dataList.RemoveAt(selection);
        Destroy(UIList[selection]);
        UIList.RemoveAt(selection);
        UpdateList();
        SetValues(-1);
    }
}
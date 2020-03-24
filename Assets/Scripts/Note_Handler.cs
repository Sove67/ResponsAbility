﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Note_Handler : MonoBehaviour
{
    // Variables
    // Note Data
    [SerializeField] public List<Note> noteList = new List<Note>();
    public List<GameObject> noteUIList = new List<GameObject>();
    private int selectedNote;

    // Enviroment Data
    private int noteEditorColourIndex;
    public GameObject noteEditorColourIndicator;
    public GameObject noteListContainer;
    public GameObject titlePrefab;

    // Buttons
    public Button editNote;
    public Button deleteNote;

    // Outputs To Update
    public Text noteContent;
    public InputField noteEditorTitle;
    public InputField noteEditorContent;

    // Other
    public List<Material> colourOptions;
    public Scrolling titleScroller;
    public Scrolling listScroller;

    // Classes
    [Serializable] public class Note // The details that make up one note
    {
        public string title { get; set; }
        public System.DateTime date { get; set; }
        public int colour { get; set; }
        public string content { get; set; }
        public bool instatiated { get; set; }
        public Note(string title, System.DateTime date, int colour, string content,  bool instatiated)
        {
            this.title = title;
            this.date = date;
            this.colour = colour;
            this.content = content;
            this.instatiated = instatiated;
        }
    }

    // Functions
    public void Start()
    {
        // Start the scrollable areas with a maximum distance of 0
        titleScroller.listLength = 0;
        titleScroller.UpdateLimits();
        Select(-1);
        UpdateList();
    }

    public void UpdateList() // run through each note, and create/update a prefab to match it's data
    {
        int count = 0;

        foreach (var note in noteList) // Update All Cards
        {
            if (!note.instatiated) // Create Title Card.
            {
                GameObject newNoteUI = Instantiate(titlePrefab, noteListContainer.transform);
                noteUIList.Add(newNoteUI);
                noteList[count].instatiated = true;
            }
            if (note.instatiated)// Set Card Properties
            {
                Vector2 position = noteUIList[count].GetComponent<RectTransform>().anchoredPosition;
                int index = count;
                float spacing = titlePrefab.GetComponent<RectTransform>().rect.height;
                noteUIList[count].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -spacing/2 + (count * -spacing));
                noteUIList[count].GetComponent<Button>().onClick.RemoveAllListeners();
                noteUIList[count].GetComponent<Button>().onClick.AddListener(() => Select(index)); // Code from https://answers.unity.com/questions/938496/buttononclickaddlistener.html & https://answers.unity.com/questions/1384803/problem-with-onclickaddlistener.html
                noteUIList[count].transform.Find("Title").GetComponent<Text>().text = noteList[count].title;
                noteUIList[count].transform.Find("Colour Indicator").GetComponent<Image>().color = colourOptions[noteList[count].colour].color;
            }
            count++;
        }

        // Update Scroll Limit for Titles
        if (count > 5)
        { titleScroller.listLength = (count-5) * titlePrefab.GetComponent<RectTransform>().rect.height; }
        else
        { titleScroller.listLength = 0; }
        titleScroller.UpdateLimits();
    }

    public void Select(int index) // Select note of index "index" from the deckList, assigning all visuals accordingly
    {
        if (index == -1)
        {
            noteContent.text = "";
            noteEditorTitle.text = "";
            noteEditorContent.text = "";
            noteEditorColourIndex = 0;
            noteEditorColourIndicator.GetComponent<Image>().color = colourOptions[0].color;
            selectedNote = index;
            editNote.interactable = false;
            deleteNote.interactable = false;
        }

        else
        {
            noteContent.text = noteList[index].content;
            noteEditorTitle.text = noteList[index].title;
            noteEditorContent.text = noteList[index].content;
            noteEditorColourIndex = noteList[index].colour;
            noteEditorColourIndicator.GetComponent<Image>().color = colourOptions[noteList[index].colour].color;
            selectedNote = index;
            editNote.interactable = true;
            deleteNote.interactable = true;
        }

        listScroller.Reset();
    }

    public void SetColour(int mod) // Move once through the choice of colours by a step size of "mod" and assign the new value to the indicator to preview
    {
        noteEditorColourIndex = (noteEditorColourIndex + mod + colourOptions.Count) % colourOptions.Count;
        noteEditorColourIndicator.GetComponent<Image>().color = colourOptions[noteEditorColourIndex].color;
    }

    public void Create() // Create a new, empty note
    {
        noteList.Add(new Note("Untitled", System.DateTime.Now, 0, "", false));
        Select(noteList.Count-1);
        UpdateList();
    }

    public void Save() // Save all changes to the selected note
    {
        if (noteEditorTitle.text == "")
        {
            noteList[selectedNote] = new Note("Untitled", System.DateTime.Now, noteEditorColourIndex, noteEditorContent.text, noteList[selectedNote].instatiated);
        }
        else
        {
            noteList[selectedNote] = new Note(noteEditorTitle.text, System.DateTime.Now, noteEditorColourIndex, noteEditorContent.text, noteList[selectedNote].instatiated);
        }
        Select(selectedNote);
        UpdateList();
    }

    public void Delete() // Delete the note's info and prefab
    {
        noteList.RemoveAt(selectedNote);
        Destroy(noteUIList[selectedNote]);
        noteUIList.RemoveAt(selectedNote);
        Select(-1);
        UpdateList();
    }
}
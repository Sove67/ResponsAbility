using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note_Handler : MonoBehaviour
{
    // Variables
    // Note Data
    private List<Note> noteInfoList = new List<Note>();
    private List<GameObject> noteUIList = new List<GameObject>();
    private int selectedNote;

    // Enviroment Data
    private int noteEditorColourIndex;
    private System.DateTime noteEditorDate;
    public GameObject noteEditorColourIndicator;
    public GameObject noteListContainer;
    public GameObject noteEditorContainer;
    public GameObject noteTitlePrefab;


    // Buttons
    public Button editNote;
    public Button deleteNote;

    // Outputs To Update
    public Text noteContent;
    public Text noteEditorTitle;
    public Text noteEditorContent;
    public Scrolling scrolling;

    // Other
    public List<Material> colourOptions;

    // Classes
    public class Note
    {
        public string title { get; set; }
        public System.DateTime date { get; set; }
        public int colour { get; set; }
        public string content { get; set; }
        public bool instatiated { get; set; }
        public Note(string title, System.DateTime date, int colour, string content, bool instatiated)
        {
            this.title = title;
            this.date = date;
            this.colour = colour;
            this.content = content;
            this.instatiated = instatiated;
        }
    }

    // Functions
    public void Update()
    {
        // Update Selection
        // Update content
    }

    public void UpdateList()
    {
        int count = 0;

        foreach (var note in noteInfoList) // Update All Cards
        {
            if (!note.instatiated) // Create Title Card.
            {
                GameObject newNoteUI = Instantiate(noteTitlePrefab, noteListContainer.transform);
                noteUIList.Add(newNoteUI);
                noteInfoList[count].instatiated = true;
            }
            if (note.instatiated)// Set Card Properties
            {
                Vector2 position = noteUIList[count].GetComponent<RectTransform>().anchoredPosition;
                int index = count;
                float spacing = noteTitlePrefab.GetComponent<RectTransform>().rect.height;
                noteUIList[count].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -spacing/2 + (count * -spacing));
                noteUIList[count].GetComponent<Button>().onClick.RemoveAllListeners();
                noteUIList[count].GetComponent<Button>().onClick.AddListener(() => UpdateSelection(index)); // Code from https://answers.unity.com/questions/938496/buttononclickaddlistener.html & https://answers.unity.com/questions/1384803/problem-with-onclickaddlistener.html
                noteUIList[count].transform.Find("Title").GetComponent<Text>().text = noteInfoList[count].title;
                noteUIList[count].transform.Find("Colour Indicator").GetComponent<Image>().color = colourOptions[noteInfoList[count].colour].color;
            }
            count++;
        }
        if (count > 5)
        {
            scrolling.sectors[1].limitY = (count-5) * noteTitlePrefab.GetComponent<RectTransform>().rect.height;
        }
        else
        {
            scrolling.sectors[1].limitY = 0;
        }
    }
    public void UpdateSelection(int index)
    {
        if (index == -1)
        {
            noteContent.text = "";
            noteEditorContainer.SetActive(true);
            noteEditorTitle.text = "";
            noteEditorContent.text = "";
            noteEditorColourIndex = 0;
            noteEditorColourIndicator.GetComponent<Image>().color = colourOptions[0].color;
            noteEditorDate = System.DateTime.Now;
            noteEditorContainer.SetActive(false);
            selectedNote = index;
            editNote.interactable = false;
            deleteNote.interactable = false;
        }
        else
        {
            noteContent.text = noteInfoList[index].content;
            noteEditorContainer.SetActive(true);
            noteEditorTitle.text = noteInfoList[index].title;
            noteEditorContent.text = noteInfoList[index].content;
            noteEditorColourIndex = noteInfoList[index].colour;
            noteEditorColourIndicator.GetComponent<Image>().color = colourOptions[noteInfoList[index].colour].color;
            noteEditorDate = noteInfoList[index].date;
            noteEditorContainer.SetActive(false);
            selectedNote = index;
            editNote.interactable = true;
            deleteNote.interactable = true;
        }
    }

    public void SetColour(int mod)
    {
        noteEditorColourIndex = (noteEditorColourIndex + mod + colourOptions.Count) % colourOptions.Count;
        noteEditorColourIndicator.GetComponent<Image>().color = colourOptions[noteEditorColourIndex].color;
    }

    public void Create()
    {
        noteInfoList.Add(new Note("Untitled", System.DateTime.Now, 0, "", false));
        UpdateSelection(noteInfoList.Count-1);
        UpdateList();
    }

    public void Save()
    {
        noteInfoList[selectedNote] = new Note(noteEditorTitle.text, System.DateTime.Now, noteEditorColourIndex, noteEditorContent.text, noteInfoList[selectedNote].instatiated);
        UpdateList();
    }

    public void Delete()
    {
        noteInfoList.RemoveAt(selectedNote);
        Destroy(noteUIList[selectedNote]);
        noteUIList.RemoveAt(selectedNote);
        UpdateSelection(-1);
        UpdateList();
    }
}
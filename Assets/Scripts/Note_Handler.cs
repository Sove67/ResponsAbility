using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note_Handler : MonoBehaviour
{
    // Variables
    // Note Data
    public List<Note> noteInfoList = new List<Note>();
    public List<GameObject> noteUIList = new List<GameObject>();
    public GameObject noteListContainer;
    public GameObject noteTitlePrefab;
    public int selectedNote;

    // Visuals To Update
    public Text noteContentContainer;
    public Text noteEditorTitle;
    public Text noteEditorContent;
    public Color noteEditorColour;
    public System.DateTime noteEditorDate;

    // Classes
    public class Note
    {
        public string title { get; set; }
        public System.DateTime date { get; set; }
        public Color colour { get; set; }
        public string content { get; set; }
        public bool instatiated { get; set; }
        public Note(string title, System.DateTime date, Color colour, string content, bool instatiated)
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
            Debug.Log("Checking Note " + count + ".");

            if (!note.instatiated) // Create Title Card.
            {
                GameObject newNoteUI = Instantiate(noteTitlePrefab, noteListContainer.transform);
                noteUIList.Add(newNoteUI);
                noteInfoList[count].instatiated = true;
                Debug.Log("Note was instantiated");
            }
            if (note.instatiated)// Set Card Properties
            {
                Vector2 position = noteUIList[count].GetComponent<RectTransform>().anchoredPosition;
                int index = count;

                //noteUIList[count].GetComponent<RectTransform>().anchoredPosition = new Vector2(position.x, position.y - (count * noteTitlePrefab.GetComponent<RectTransform>().rect.height));
                noteUIList[count].GetComponent<Button>().onClick.RemoveAllListeners();
                noteUIList[count].GetComponent<Button>().onClick.AddListener(() => UpdateSelection(index)); // Code from https://answers.unity.com/questions/938496/buttononclickaddlistener.html & https://answers.unity.com/questions/1384803/problem-with-onclickaddlistener.html
                noteUIList[count].GetComponentInChildren<Text>().text = noteInfoList[count].title;
                noteUIList[count].GetComponentInChildren<Image>().color = noteInfoList[count].colour;
            }
            count++;
        }
    }
    public void UpdateSelection(int index)
    {
        Debug.Log("Updated Selection to " + index + ".");
        noteContentContainer.text = noteInfoList[index].content;
        noteEditorTitle.text = noteInfoList[index].title;
        noteEditorContent.text = noteInfoList[index].content;
        noteEditorColour = noteInfoList[index].colour;
        noteEditorDate = noteInfoList[index].date;
        selectedNote = index;
    }

    public void Create()
    {
        noteInfoList.Add(new Note("Untitled", System.DateTime.Now, Color.white, "", false));
        UpdateSelection(noteInfoList.Count-1);
        UpdateList();
    }

    public void Save()
    {
        noteInfoList[selectedNote] = new Note(noteEditorTitle.text, System.DateTime.Now, noteEditorColour, noteEditorContent.text, noteInfoList[selectedNote].instatiated);
        UpdateList();
    }

    public void Delete()
    {
        noteInfoList.RemoveAt(selectedNote);
        Destroy(noteUIList[selectedNote]);
        noteUIList.RemoveAt(selectedNote);
        UpdateList();
    }
}
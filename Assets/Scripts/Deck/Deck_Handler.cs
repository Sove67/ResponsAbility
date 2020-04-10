
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.Notifications.Android;

public class Deck_Handler : MonoBehaviour
{
    // Variables
    // Deck & Card Data
    [SerializeField] public List<Deck> dataList = new List<Deck>();
    readonly private List<GameObject> UIList = new List<GameObject>();
    public int selection = -1;

    // Enviroment Data
    public Text description;
    private int editorColourIndex;
    public GameObject editorColourIndicator;
    public Text reminderPeriod;
    public GameObject listRoot;
    public GameObject titlePrefab;
    public ToggleGroup titleToggleGroup;

    // Buttons
    public Button practiceButton;
    public Button editButton;
    public Button deleteButton;
    public Button viewScoreButton;
    public Toggle multipleChoiceToggle;

    // Inputs
    public InputField editorTitle;
    public InputField editorDescription;

    // Other
    public List<Material> colourOptions;
    public Scrolling titleScrolling;
    public Scrolling contentScrolling;
    public Card_Handler card_handler;
    public Reminder_Handler reminder_handler;
    public Statistics_Handler statistics_handler;

    // Classes
    [Serializable] public class Deck // The details of one flashcard deck, including the cards created and marks received
    {
        public string title { get; set; }
        public string description { get; set; }
        public List<Deck_Practice.SessionResult> practiceSessions { get; set; }
        public int colour { get; set; }
        public List<Card_Handler.Card> content { get; set; }
        public Reminder_Handler.Reminder reminder { get; set; }
        public bool instantiated { get; set; }
        public Deck(string title, string description, List<Deck_Practice.SessionResult> practiceSessions, int colour, List<Card_Handler.Card> content, Reminder_Handler.Reminder reminder, bool instantiated)
        {
            this.title = title;
            this.description = description;
            this.practiceSessions = practiceSessions;
            this.colour = colour;
            this.content = content;
            this.reminder = reminder;
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

    public void UpdateList() // run through each set of decks, and create/update a prefab to match it's data
    {
        int count = 0;

        foreach (var deck in dataList) // Update All Cards
        {
            if (!deck.instantiated) // Create Title Card.
            {
                GameObject newDeckUI = Instantiate(titlePrefab, listRoot.transform);
                UIList.Add(newDeckUI);
                UIList[count].GetComponent<Toggle>().group = titleToggleGroup;
                dataList[count].instantiated = true;
            }
            // Set Card Properties
            Vector2 position = UIList[count].GetComponent<RectTransform>().anchoredPosition;
            int index = count;
            float spacing = titlePrefab.GetComponent<RectTransform>().rect.height;
            UIList[count].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -spacing / 2 + (count * -spacing));
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

    public void ParseToggleToSelect(bool isOn, int index) // Set values based on if the toggle calling is active. Otherwise, remove values if no toggle is active.
    {
        if (isOn)
        {
            SetValues(index); 
        }
        else if (!titleToggleGroup.AnyTogglesOn())
        { SetValues(-1); }
    }

    public void Select(int index) // Selections must be handled by toggling the button, because the button cannot be assigned without triggering its "On Value Changed" condition and creating a recursive loop.
    {
        titleToggleGroup.SetAllTogglesOff();
        UIList[index].GetComponent<Toggle>().isOn = true;
    }

    public void SetValues(int index) // Select deck of index "index" from the dataList, assigning all visuals accordingly
    {
        card_handler.DeleteCardUI(); // Clear Old First
        selection = index;

        if (index == -1)
        {
            description.text = "";
            editorTitle.text = "";
            editorDescription.text = "";
            editorColourIndex = 0;
            editorColourIndicator.GetComponent<Image>().color = colourOptions[0].color;
            reminderPeriod.text = "";

            practiceButton.interactable = false;
            editButton.interactable = false;
            deleteButton.interactable = false;
            viewScoreButton.interactable = false;
            multipleChoiceToggle.interactable = false;
            multipleChoiceToggle.isOn = false;
            card_handler.cardList = new List<Card_Handler.Card>();
        }
        else
        {
            description.text = dataList[index].description;
            editorTitle.text = dataList[index].title;
            editorDescription.text = dataList[index].description;
            editorColourIndex = dataList[index].colour;
            editorColourIndicator.GetComponent<Image>().color = colourOptions[dataList[index].colour].color;
            reminder_handler.ChangeReminder(0);

            if (dataList[index].content.Count > 0)
            { practiceButton.interactable = true; }
            else
            { practiceButton.interactable = false; }
            editButton.interactable = true;
            deleteButton.interactable = true;
            if (dataList[index].practiceSessions.Count > 0)
            { viewScoreButton.interactable = true; }
            else { viewScoreButton.interactable = false; }
            if (dataList[index].practiceSessions.Count > 4)
            { multipleChoiceToggle.interactable = true; }
            else { 
                multipleChoiceToggle.interactable = false;
                multipleChoiceToggle.isOn = false;
            }
            card_handler.cardList = new List<Card_Handler.Card>(dataList[index].content);
        }

        contentScrolling.UpdateLimits();
        contentScrolling.Reset();
        card_handler.SelectCard(-1);
    }

    public void SetColour(int mod) // Move once through the choice of colours by a step size of "mod" and assign the new value to the indicator to preview
    {
        editorColourIndex = (editorColourIndex + mod + colourOptions.Count) % colourOptions.Count;
        editorColourIndicator.GetComponent<Image>().color = colourOptions[editorColourIndex].color;
    }

    public void Create() // Create a new, empty deck with one empty card
    {
        dataList.Add(new Deck("Untitled", "", new List<Deck_Practice.SessionResult>(), 0, new List<Card_Handler.Card>(), new Reminder_Handler.Reminder(0, TimeSpan.Zero, System.DateTime.Now), false));
        UpdateList();
        Select(dataList.Count - 1);
    }

    public void Save() // Save all changes to the selected deck
    {
        if (editorTitle.text == "")
        {
            dataList[selection] = new Deck("Untitled", editorDescription.text, dataList[selection].practiceSessions, editorColourIndex, card_handler.cardList, dataList[selection].reminder, dataList[selection].instantiated);
        }
        else
        {
            dataList[selection] = new Deck(editorTitle.text, editorDescription.text, dataList[selection].practiceSessions, editorColourIndex, card_handler.cardList, dataList[selection].reminder, dataList[selection].instantiated);
        }
        UpdateList();
        Select(selection);
    }

    public void Cancel() // Reset editor values to card values
    { Select(selection); }

    public void Delete() // Delete the deck's info and prefab
    {
        int ID = dataList[selection].reminder.ID ?? default;
        AndroidNotificationCenter.CancelNotification(ID);
        dataList.RemoveAt(selection);
        Destroy(UIList[selection]);
        UIList.RemoveAt(selection);
        SetValues(-1);
        UpdateList();
    }
}
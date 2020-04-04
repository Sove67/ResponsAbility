
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
    [SerializeField] public List<Deck> deckList = new List<Deck>();
    readonly private List<GameObject> deckUIList = new List<GameObject>();
    public int selection = -1;

    // Enviroment Data
    private int editorColourIndex;
    public GameObject editorColourIndicator;
    public Text reminderPeriod;
    public GameObject listRoot;
    public GameObject titlePrefab;

    // Buttons
    public Button practiceButton;
    public Button editButton;
    public Button deleteButton;
    public Button viewScoreButton;

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
        Select(-1);
        UpdateList();
    }

    public void UpdateList() // run through each set of decks, and create/update a prefab to match it's data
    {
        int count = 0;

        foreach (var deck in deckList) // Update All Cards
        {
            if (!deck.instantiated) // Create Title Card.
            {
                GameObject newDeckUI = Instantiate(titlePrefab, listRoot.transform);
                deckUIList.Add(newDeckUI);
                deckList[count].instantiated = true;
            }
            if (deck.instantiated)// Set Card Properties
            {
                Vector2 position = deckUIList[count].GetComponent<RectTransform>().anchoredPosition;
                int index = count;
                float spacing = titlePrefab.GetComponent<RectTransform>().rect.height;
                deckUIList[count].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -spacing / 2 + (count * -spacing));
                deckUIList[count].GetComponent<Button>().onClick.RemoveAllListeners();
                deckUIList[count].GetComponent<Button>().onClick.AddListener(() => Select(index)); // Code from https://answers.unity.com/questions/938496/buttononclickaddlistener.html & https://answers.unity.com/questions/1384803/problem-with-onclickaddlistener.html
                deckUIList[count].transform.Find("Title").GetComponent<Text>().text = deckList[count].title;
                deckUIList[count].transform.Find("Colour Indicator").GetComponent<Image>().color = colourOptions[deckList[count].colour].color;
            }
            count++;
        }

        // Update Scroll Limit for Titles
        titleScrolling.listLength = (count) * titlePrefab.GetComponent<RectTransform>().rect.height;
        titleScrolling.UpdateLimits();
    }

    public void Select(int index) // Select deck of index "index" from the deckList, assigning all visuals accordingly
    {
        card_handler.DeleteCardUI(); // Clear Old First
        selection = index;
        if (index == -1)
        {
            editorTitle.text = "";
            editorDescription.text = "";
            reminderPeriod.text = "";
            editorColourIndex = 0;
            editorColourIndicator.GetComponent<Image>().color = colourOptions[0].color;
            practiceButton.interactable = false;
            editButton.interactable = false;
            deleteButton.interactable = false;
            viewScoreButton.interactable = false;
            card_handler.cardList = new List<Card_Handler.Card>();
        }
        else
        {
            editorTitle.text = deckList[selection].title;
            editorDescription.text = deckList[selection].description;
            reminder_handler.ChangeReminder(0);
            editorColourIndex = deckList[selection].colour;
            editorColourIndicator.GetComponent<Image>().color = colourOptions[deckList[selection].colour].color;
            if (deckList[selection].content.Count > 0)
            { practiceButton.interactable = true; }
            else
            { practiceButton.interactable = false; }
            editButton.interactable = true;
            deleteButton.interactable = true;
            if (deckList[selection].practiceSessions.Count > 0)
            { viewScoreButton.interactable = true; }
            else { viewScoreButton.interactable = false; }
            card_handler.cardList = new List<Card_Handler.Card>(deckList[selection].content);
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

    public void Create() // Create a new, empty deck
    {
        deckList.Add(new Deck("Untitled", "", new List<Deck_Practice.SessionResult>(), 0, new List<Card_Handler.Card>(), new Reminder_Handler.Reminder(0, TimeSpan.Zero, System.DateTime.Now), false));
        Select(deckList.Count - 1);
        UpdateList();
    }

    public void Save() // Save all changes to the selected deck
    {
        if (editorTitle.text == "")
        {
            deckList[selection] = new Deck("Untitled", editorDescription.text, deckList[selection].practiceSessions, editorColourIndex, card_handler.cardList, deckList[selection].reminder, deckList[selection].instantiated);
        }
        else
        {
            deckList[selection] = new Deck(editorTitle.text, editorDescription.text, deckList[selection].practiceSessions, editorColourIndex, card_handler.cardList, deckList[selection].reminder, deckList[selection].instantiated);
        }
        Select(selection);
        UpdateList();
    }

    public void Cancel() // Reset editor values to card values
    { Select(selection); }

    public void Delete() // Delete the deck's info and prefab
    {
        int ID = deckList[selection].reminder.ID ?? default;
        AndroidNotificationCenter.CancelNotification(ID);
        deckList.RemoveAt(selection);
        Destroy(deckUIList[selection]);
        deckUIList.RemoveAt(selection);
        Select(-1);
        UpdateList();
    }
}
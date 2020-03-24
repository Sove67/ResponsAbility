
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Deck_Handler : MonoBehaviour
{
    // Variables
    // Deck & Card Data
    [SerializeField] public List<Deck> deckList = new List<Deck>();
    private List<GameObject> deckUIList = new List<GameObject>();
    public int selection = -1;

    // Enviroment Data
    private int editorColourIndex;
    public GameObject editorColourIndicator;
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
    public Scrolling scrolling;
    public Card_Handler card_handler;


    // Classes
    [Serializable] public class Deck // The details of one flashcard deck, including the cards created and marks received
    {
        public string title { get; set; }
        public string description { get; set; }
        public List<Deck_Practice.Mark> mark { get; set; }
        public int colour { get; set; }
        public List<Card_Handler.Card> content { get; set; }
        public int reminderPeriod { get; set; }
        public bool instatiated { get; set; }
        public Deck(string title, string description, List<Deck_Practice.Mark> mark, int colour, List<Card_Handler.Card> content, int reminderPeriod, bool instatiated)
        {
            this.title = title;
            this.description = description;
            this.mark = mark;
            this.colour = colour;
            this.content = content;
            this.reminderPeriod = reminderPeriod;
            this.instatiated = instatiated;
        }
    }


    // Functions
    public void Start()
    {
        // Start the scrollable areas with a maximum distance of 0
        scrolling.listLength = 0;
        scrolling.UpdateLimits();
        Select(-1);
        UpdateList();
    }

    public void UpdateList() // run through each set of decks, and create/update a prefab to match it's data
    {
        int count = 0;

        foreach (var deck in deckList) // Update All Cards
        {
            if (!deck.instatiated) // Create Title Card.
            {
                GameObject newDeckUI = Instantiate(titlePrefab, listRoot.transform);
                deckUIList.Add(newDeckUI);
                deckList[count].instatiated = true;
            }
            if (deck.instatiated)// Set Card Properties
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
        if (count > 5)
        { scrolling.listLength = (count - 5) * titlePrefab.GetComponent<RectTransform>().rect.height; }
        else
        { scrolling.listLength = 0; }
    }

    public void Select(int index) // Select deck of index "index" from the deckList, assigning all visuals accordingly
    {
        card_handler.DeleteCardUI(); // Clear Old First
        selection = index;
        if (index == -1)
        {
            editorTitle.text = "";
            editorDescription.text = "";
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
            editorColourIndex = deckList[selection].colour;
            editorColourIndicator.GetComponent<Image>().color = colourOptions[deckList[selection].colour].color;
            if (deckList[selection].content.Count > 0)
            { practiceButton.interactable = true; }
            else
            { practiceButton.interactable = false; }
            editButton.interactable = true;
            deleteButton.interactable = true;
            if (deckList[selection].mark.Count > 0)
            { viewScoreButton.interactable = true; }
            else { viewScoreButton.interactable = false; }
            card_handler.cardList = deckList[selection].content;
        }
        card_handler.SelectCard(-1);
    }

    public void SetColour(int mod) // Move once through the choice of colours by a step size of "mod" and assign the new value to the indicator to preview
    {
        editorColourIndex = (editorColourIndex + mod + colourOptions.Count) % colourOptions.Count;
        editorColourIndicator.GetComponent<Image>().color = colourOptions[editorColourIndex].color;
    }

    public void Create() // Create a new, empty deck
    {
        deckList.Add(new Deck("Untitled", "", new List<Deck_Practice.Mark>(), 0, new List<Card_Handler.Card>(), 0, false));
        Select(deckList.Count - 1);
        UpdateList();
    }

    public void Save() // Save all changes to the selected deck
    {
        if (editorTitle.text == "")
        {
            deckList[selection] = new Deck("Untitled", editorDescription.text, deckList[selection].mark, editorColourIndex, card_handler.cardList, deckList[selection].reminderPeriod, deckList[selection].instatiated);
        }
        else
        {
            deckList[selection] = new Deck(editorTitle.text, editorDescription.text, deckList[selection].mark, editorColourIndex, card_handler.cardList, deckList[selection].reminderPeriod, deckList[selection].instatiated);
        }
        Select(selection);
        UpdateList();
    }

    public void Delete() // Delete the deck's info and prefab
    {
        deckList.RemoveAt(selection);
        Destroy(deckUIList[selection]);
        deckUIList.RemoveAt(selection);
        Select(-1);
        UpdateList();
    }
}
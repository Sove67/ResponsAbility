
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Flashcard_Handler : MonoBehaviour
{
    // Variables
    // Deck & Card Data
    [SerializeField] public List<Deck> decks = new List<Deck>();
    [SerializeField] public List<Card> currentCards = new List<Card>();
    public List<GameObject> deckUIList = new List<GameObject>();
    public List<GameObject> cardUIList = new List<GameObject>();
    private int selectedDeck = -1;
    public int selectedCard = -1;

    // Enviroment Data
    private int deckEditorColourIndex;
    public GameObject deckEditorColourIndicator;
    public GameObject deckListContainer;
    public GameObject cardListContainer;
    public GameObject deckTitlePrefab;
    public GameObject cardTitlePrefab;

    // Buttons
    public Button practiceDeck;
    public Button editDeck;
    public Button deleteDeck;
    public Button saveCard;
    public Button deleteCard;

    // Outputs To Update
    public Text deckDescription;
    public InputField deckEditorTitle;
    public InputField cardEditorTitle;
    public InputField deckEditorDescription;
    public InputField cardEditorQuestion;
    public InputField cardEditorAnswer;

    // Other
    public List<Material> colourOptions;
    public Scrolling deckTitleScroller;
    public Scrolling cardTitleScroller;
    public float debug1;
    public float debug2;

    // Classes
    [Serializable] public class Card
    {
        public string title { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public bool instatiated { get; set; }
        public Card(string title, string question, string answer, bool instatiated)
        {
            this.title = title;
            this.question = question;
            this.answer = answer;
            this.instatiated = instatiated;
        }
    }

    [Serializable] public class Deck
    {
        public string title { get; set; }
        public string description { get; set; }
        public System.DateTime date { get; set; }
        public int colour { get; set; }
        public List<Card> content { get; set; }
        public bool instatiated { get; set; }
        public Deck(string title, string description, System.DateTime date, int colour, List<Card> content, bool instatiated)
        {
            this.title = title;
            this.description = description;
            this.date = date;
            this.colour = colour;
            this.content = content;
            this.instatiated = instatiated;
        }
    }

    // Functions
    public void Start()
    {
        deckTitleScroller.listLength = 0;
        cardTitleScroller.listLength = 0;
    }

    // Deck
    public void UpdateDeckList()
    {
        int count = 0;

        foreach (var deck in decks) // Update All Cards
        {
            if (!deck.instatiated) // Create Title Card.
            {
                GameObject newDeckUI = Instantiate(deckTitlePrefab, deckListContainer.transform);
                deckUIList.Add(newDeckUI);
                decks[count].instatiated = true;
            }
            if (deck.instatiated)// Set Card Properties
            {
                Vector2 position = deckUIList[count].GetComponent<RectTransform>().anchoredPosition;
                int index = count;
                float spacing = deckTitlePrefab.GetComponent<RectTransform>().rect.height;
                deckUIList[count].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -spacing / 2 + (count * -spacing));
                deckUIList[count].GetComponent<Button>().onClick.RemoveAllListeners();
                deckUIList[count].GetComponent<Button>().onClick.AddListener(() => SelectDeck(index)); // Code from https://answers.unity.com/questions/938496/buttononclickaddlistener.html & https://answers.unity.com/questions/1384803/problem-with-onclickaddlistener.html
                deckUIList[count].transform.Find("Title").GetComponent<Text>().text = decks[count].title;
                deckUIList[count].transform.Find("Colour Indicator").GetComponent<Image>().color = colourOptions[decks[count].colour].color;
            }
            count++;
        }

        // Update Scroll Limit for Titles
        if (count > 5)
        { 
            deckTitleScroller.listLength = (count - 5) * deckTitlePrefab.GetComponent<RectTransform>().rect.height;
            debug1 = (count - 5) * deckTitlePrefab.GetComponent<RectTransform>().rect.height;
        }
        else
        { 
            deckTitleScroller.listLength = 0;
            deckTitleScroller.listLength = debug1;
        }
    }

    public void SelectDeck(int index)
    {
        DeleteCardUI(); // Clear Old First

        selectedDeck = index;
        SelectCard(-1);
        if (index == -1)
        {
            deckDescription.text = "";
            deckEditorTitle.text = "";
            deckEditorDescription.text = "";
            deckEditorColourIndex = 0;
            deckEditorColourIndicator.GetComponent<Image>().color = colourOptions[0].color;
            practiceDeck.interactable = false;
            editDeck.interactable = false;
            deleteDeck.interactable = false;
            currentCards = new List<Card>();
        }
        else
        {
            deckDescription.text = decks[selectedDeck].description;
            deckEditorTitle.text = decks[selectedDeck].title;
            deckEditorDescription.text = decks[selectedDeck].description;
            deckEditorColourIndex = decks[selectedDeck].colour;
            deckEditorColourIndicator.GetComponent<Image>().color = colourOptions[decks[selectedDeck].colour].color;
            practiceDeck.interactable = true;
            editDeck.interactable = true;
            deleteDeck.interactable = true;
            currentCards = decks[selectedDeck].content;
        }
    }

    public void SetDeckColour(int mod)
    {
        deckEditorColourIndex = (deckEditorColourIndex + mod + colourOptions.Count) % colourOptions.Count;
        deckEditorColourIndicator.GetComponent<Image>().color = colourOptions[deckEditorColourIndex].color;
    }

    public void CreateDeck()
    {
        decks.Add(new Deck("Untitled", "", System.DateTime.Now, 0, new List<Card>(), false));
        SelectDeck(decks.Count - 1);
        UpdateDeckList();
    }

    public void SaveDeck()
    {
        if (deckEditorTitle.text == "")
        {
            decks[selectedDeck] = new Deck("Untitled", deckEditorDescription.text, System.DateTime.Now, deckEditorColourIndex, currentCards, decks[selectedDeck].instatiated);
        }
        else
        {
            decks[selectedDeck] = new Deck(deckEditorTitle.text, deckEditorDescription.text, System.DateTime.Now, deckEditorColourIndex, currentCards, decks[selectedDeck].instatiated);
        }
        SelectDeck(selectedDeck);
        UpdateDeckList();
    }

    public void DeleteDeck()
    {
        decks.RemoveAt(selectedDeck);
        Destroy(deckUIList[selectedDeck]);
        deckUIList.RemoveAt(selectedDeck);
        SelectDeck(-1);
        UpdateDeckList();
    }

    // Card
    public void UpdateCardList()
    {
        int count = 0;
        foreach (var card in currentCards) // Update All Cards
        {
            if (!card.instatiated) // Create Title Card.
            {
                Debug.Log("A");
                cardUIList.Add(Instantiate(cardTitlePrefab, cardListContainer.transform));
                currentCards[count].instatiated = true;
            }
            if (card.instatiated)// Set Card Properties
            {
                Vector2 position = cardUIList[count].GetComponent<RectTransform>().anchoredPosition;
                int index = count;
                float spacing = cardTitlePrefab.GetComponent<RectTransform>().rect.height;
                cardUIList[count].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -spacing / 2 + (count * -spacing));
                cardUIList[count].GetComponent<Button>().onClick.RemoveAllListeners();
                cardUIList[count].GetComponent<Button>().onClick.AddListener(() => SelectCard(index)); // Code from https://answers.unity.com/questions/938496/buttononclickaddlistener.html & https://answers.unity.com/questions/1384803/problem-with-onclickaddlistener.html
                cardUIList[count].transform.Find("Title").GetComponent<Text>().text = currentCards[count].title;
            }
            count++;
        }
        /*
        Debug.Log(currentCards.Count + ", " + cardUIList.Count);
        for (int i = currentCards.Count; i < cardUIList.Count; i++) // If list overflows, purge the rest.
        {
            Destroy(cardUIList[i]);
            cardUIList.RemoveAt(i);
        }
        */

        // Update Scroll Limit for Titles
        if (count > 5)
        { 
            cardTitleScroller.listLength = (count - 5) * cardTitlePrefab.GetComponent<RectTransform>().rect.height;
            debug2 = (count - 5) * cardTitlePrefab.GetComponent<RectTransform>().rect.height;
        }
        else
        { 
            cardTitleScroller.listLength = 0;
            debug2 = 0;
        }
    }

    public void SelectCard(int index)
    {
        if (index == -1)
        {
            cardEditorTitle.text = "";
            cardEditorQuestion.text = "";
            cardEditorAnswer.text = "";
            selectedCard = index;
            cardEditorTitle.interactable = false;
            cardEditorQuestion.interactable = false;
            cardEditorAnswer.interactable = false;
            saveCard.interactable = false;
            deleteCard.interactable = false;
}
        else
        {
            cardEditorTitle.text = currentCards[index].title;
            cardEditorQuestion.text = currentCards[index].question;
            cardEditorAnswer.text = currentCards[index].answer;
            selectedCard = index;
            cardEditorTitle.interactable = true;
            cardEditorQuestion.interactable = true;
            cardEditorAnswer.interactable = true;
            saveCard.interactable = true;
            deleteCard.interactable = true;
        }
    }

    public void CreateCard()
    {
        currentCards.Add(new Card("Untitled", "", "", false));
        SelectCard(currentCards.Count - 1);
        UpdateCardList();
    }

    public void SaveCard()
    {
        if (cardEditorTitle.text == "")
        {
            currentCards[selectedCard] = new Card("Untitled", cardEditorQuestion.text, cardEditorAnswer.text, currentCards[selectedCard].instatiated);
        }
        else
        {
            currentCards[selectedCard] = new Card(cardEditorTitle.text, cardEditorQuestion.text, cardEditorAnswer.text, currentCards[selectedCard].instatiated);
        }
        SelectCard(-1);
        UpdateCardList();
    }

    public void DeleteCard()
    {
        currentCards.RemoveAt(selectedCard);
        Destroy(cardUIList[selectedCard]);
        cardUIList.RemoveAt(selectedCard);
        SelectCard(-1);
        UpdateCardList();
    }

    public void DeleteCardUI()
    {
        foreach (GameObject gameObject in cardUIList)
        { Destroy(gameObject); }
        int limit = currentCards.Count;
        for (int i = 0; i < limit; i++)
        { currentCards[i].instatiated = false; }
        cardUIList.Clear();
    }
}
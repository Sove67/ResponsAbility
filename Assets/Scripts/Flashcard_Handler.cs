
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Flashcard_Handler : MonoBehaviour
{
    // Variables
    // Deck & Card Data
    [SerializeField] public List<Deck> deckList = new List<Deck>();
    private List<Card> currentCards = new List<Card>();
    private List<GameObject> deckUIList = new List<GameObject>();
    private List<GameObject> cardUIList = new List<GameObject>();
    public Text deckDescription;
    private int selectedDeck = -1;
    private int selectedCard = -1;

    // Practice Data
    private List<Card> practiceCards = new List<Card>();
    public Text deckTitle;
    public Text cardTitle;
    public Text question;
    private int selectedPracticeCard = -1;
    int chosenAnswer;
    private int correctAnswer;
    private List<int> practiceMarkIndex;
    private List<bool> practiceMark = new List<bool>();

    // Enviroment Data
    private int deckEditorColourIndex;
    public GameObject practiceWindow;
    public GameObject multipleChoiceWindow;
    public GameObject deckEditorColourIndicator;
    public GameObject deckListContainer;
    public GameObject cardListContainer;
    public GameObject deckTitlePrefab;
    public GameObject cardTitlePrefab;

    // Buttons
    public Button practiceDeck;
    public Button editDeck;
    public Button deleteDeck;
    public Toggle shuffleToggle;
    public Toggle multipleChoiceToggle;
    public Toggle[] multipleChoices = new Toggle[4];
    public Button saveCard;
    public Button deleteCard;

    // Inputs
    public InputField deckEditorTitle;
    public InputField cardEditorTitle;
    public InputField deckEditorDescription;
    public InputField cardEditorQuestion;
    public InputField cardEditorAnswer;
    public InputField answerInput;

    // Other
    public List<Material> colourOptions;
    public Scrolling deckTitleScroller;
    public Scrolling cardTitleScroller;

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
        public List<Mark> mark { get; set; }
        public int colour { get; set; }
        public List<Card> content { get; set; }
        public bool instatiated { get; set; }
        public Deck(string title, string description, List<Mark> mark, int colour, List<Card> content, bool instatiated)
        {
            this.title = title;
            this.description = description;
            this.mark = mark;
            this.colour = colour;
            this.content = content;
            this.instatiated = instatiated;
        }
    }

    [Serializable] public class Mark
    {
        public float grade { get; set; }
        public List<bool> correct { get; set; }
        public Mark(float grade, List<bool> correct)
        {
            this.grade = grade;
            this.correct = correct;
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

        foreach (var deck in deckList) // Update All Cards
        {
            if (!deck.instatiated) // Create Title Card.
            {
                GameObject newDeckUI = Instantiate(deckTitlePrefab, deckListContainer.transform);
                deckUIList.Add(newDeckUI);
                deckList[count].instatiated = true;
            }
            if (deck.instatiated)// Set Card Properties
            {
                Vector2 position = deckUIList[count].GetComponent<RectTransform>().anchoredPosition;
                int index = count;
                float spacing = deckTitlePrefab.GetComponent<RectTransform>().rect.height;
                deckUIList[count].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -spacing / 2 + (count * -spacing));
                deckUIList[count].GetComponent<Button>().onClick.RemoveAllListeners();
                deckUIList[count].GetComponent<Button>().onClick.AddListener(() => SelectDeck(index)); // Code from https://answers.unity.com/questions/938496/buttononclickaddlistener.html & https://answers.unity.com/questions/1384803/problem-with-onclickaddlistener.html
                deckUIList[count].transform.Find("Title").GetComponent<Text>().text = deckList[count].title;
                deckUIList[count].transform.Find("Colour Indicator").GetComponent<Image>().color = colourOptions[deckList[count].colour].color;
            }
            count++;
        }

        // Update Scroll Limit for Titles
        if (count > 5)
        { deckTitleScroller.listLength = (count - 5) * deckTitlePrefab.GetComponent<RectTransform>().rect.height; }
        else
        { deckTitleScroller.listLength = 0; }
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
            deckDescription.text = deckList[selectedDeck].description;
            deckEditorTitle.text = deckList[selectedDeck].title;
            deckEditorDescription.text = deckList[selectedDeck].description;
            deckEditorColourIndex = deckList[selectedDeck].colour;
            deckEditorColourIndicator.GetComponent<Image>().color = colourOptions[deckList[selectedDeck].colour].color;
            practiceDeck.interactable = true;
            editDeck.interactable = true;
            deleteDeck.interactable = true;
            currentCards = deckList[selectedDeck].content;
        }
    }

    public void SetDeckColour(int mod)
    {
        deckEditorColourIndex = (deckEditorColourIndex + mod + colourOptions.Count) % colourOptions.Count;
        deckEditorColourIndicator.GetComponent<Image>().color = colourOptions[deckEditorColourIndex].color;
    }

    public void CreateDeck()
    {
        deckList.Add(new Deck("Untitled", "", new List<Mark>(), 0, new List<Card>(), false));
        SelectDeck(deckList.Count - 1);
        UpdateDeckList();
    }

    public void SaveDeck()
    {
        if (deckEditorTitle.text == "")
        {
            deckList[selectedDeck] = new Deck("Untitled", deckEditorDescription.text, deckList[selectedDeck].mark, deckEditorColourIndex, currentCards, deckList[selectedDeck].instatiated);
        }
        else
        {
            deckList[selectedDeck] = new Deck(deckEditorTitle.text, deckEditorDescription.text, deckList[selectedDeck].mark, deckEditorColourIndex, currentCards, deckList[selectedDeck].instatiated);
        }
        SelectDeck(selectedDeck);
        UpdateDeckList();
    }

    public void DeleteDeck()
    {
        deckList.RemoveAt(selectedDeck);
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

        // Update Scroll Limit for Titles
        if (count > 5)
        { cardTitleScroller.listLength = (count - 5) * cardTitlePrefab.GetComponent<RectTransform>().rect.height; }
        else
        { cardTitleScroller.listLength = 0; }
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
        if (selectedCard != -1)
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

    // Practice
    public void LoadPracticeDeck()
    {
        if (shuffleToggle.isOn) // if shuffling deck
        {
            practiceCards = new List<Card>(deckList[selectedDeck].content);
            practiceMarkIndex = new List<int>(practiceCards.Count);
            for (int i = 0; i < practiceCards.Count; i++)
            {
                Card temp1 = practiceCards[i];
                int temp2 = practiceMarkIndex[i];

                int randomIndex = UnityEngine.Random.Range(i, practiceCards.Count);

                practiceCards[i] = practiceCards[randomIndex];
                practiceCards[randomIndex] = temp1;

                practiceMarkIndex[i] = randomIndex;
                practiceMarkIndex[randomIndex] = temp2;
            }
        }
        else
        { practiceCards = deckList[selectedDeck].content; }

        if (multipleChoiceToggle.isOn) // If using multiple choice
        {
            Debug.Log("Activating Choices");
            multipleChoiceWindow.gameObject.SetActive(true);
            answerInput.gameObject.SetActive(false);
        }
        else
        {
            multipleChoiceWindow.gameObject.SetActive(false);
            answerInput.gameObject.SetActive(true);
        }

        if (practiceMark != null)
        { practiceMark.Clear(); }
        deckTitle.text = deckList[selectedDeck].title;
        LoadPracticeCard(-1);
    }

    public void LoadPracticeCard(int mod)
    {

        if (mod == -1) // If choosing first card
        {
            selectedPracticeCard = 0;
            cardTitle.text = practiceCards[selectedPracticeCard].title;
            question.text = practiceCards[selectedPracticeCard].question;
        }

        else if (selectedPracticeCard + mod < practiceCards.Count) // If choosing card within deck
        {
            selectedPracticeCard += mod;
            cardTitle.text = practiceCards[selectedPracticeCard].title;
            question.text = practiceCards[selectedPracticeCard].question;
        }

        else
        { Finish(); }

        if (multipleChoiceToggle.isOn) // if multiple choice
        {
            for (int i = 0; i < 4; i++) // Assign random answers
            {
                int randomIndex;
                randomIndex = UnityEngine.Random.Range(0, practiceCards.Count - 1);
                if (randomIndex == selectedPracticeCard)
                { randomIndex++; }

                multipleChoices[i].GetComponentInChildren<Text>().text = practiceCards[randomIndex].answer;
            }
            // Assign one correct answer
            correctAnswer = UnityEngine.Random.Range(0, 3);
            multipleChoices[correctAnswer].GetComponentInChildren<Text>().text = practiceCards[selectedPracticeCard].answer;
        }

    }

    public void ChooseAnswer()
    {
        for (int i = 0; i < multipleChoices.Length; i++)
        {
            if (multipleChoices[i].isOn)
            {
                chosenAnswer = i;
            }
        }
    }

    public void CheckAnswer()
    {
        bool correct = false;
        if (multipleChoiceToggle.isOn && chosenAnswer == correctAnswer) // If looking for multiple choice & correct
        { correct = true; }

        else if (answerInput.text != null && answerInput.text == practiceCards[selectedPracticeCard].answer) // If looking for text input & correct
        { correct = true; }

        practiceMark.Add(correct); // Record Result
        LoadPracticeCard(1); // Shift to next card
    }

    public void Finish()
    {
        practiceWindow.SetActive(false);

        float count = 0;
        List<bool> organizedMarks = new List<bool>(practiceMark.Count);

        for (int i = 0; i < practiceMark.Count; i++)
        {
            organizedMarks[i] = practiceMark[practiceMarkIndex[i]]; // Sort the marks by the shuffled index

            if (practiceMark[i])
            { count++; }
        }

        Debug.Log("Newest Score: " + (count / practiceMark.Count));
        deckList[selectedDeck].mark.Add(new Mark(count / practiceMark.Count, organizedMarks));
    }
}
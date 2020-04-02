using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class Card_Handler : MonoBehaviour
{
    // Variables
    // General
    public List<Card_Handler.Card> cardList = new List<Card_Handler.Card>();
    readonly private List<GameObject> cardUIList = new List<GameObject>();
    private int selection = -1;

    // Enviroment
    public GameObject listRoot;
    public GameObject titlePrefab;

    // Input
    public Button saveButton;
    public Button deleteButton;
    public InputField editorTitle;
    public InputField editorQuestion;
    public InputField editorAnswer;
    public Scrolling titleScrolling;

    // Classes
    [Serializable] public class Card // The details that make up one card in a deck
    {
        public string title { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public bool instantiated { get; set; }
        public Card(string title, string question, string answer, bool instantiated)
        {
            this.title = title;
            this.question = question;
            this.answer = answer;
            this.instantiated = instantiated;
        }
    }


    // Functions
    public void Start()
    {
        titleScrolling.listLength = 0;
        titleScrolling.UpdateLimits();
    }
    public void UpdateCardList() // run through each set of cards, and create/update a prefab to match it's data
    {
        int count = 0;
        foreach (var card in cardList) // Update All Cards
        {
            if (!card.instantiated) // Create Title Card.
            {
                cardUIList.Add(Instantiate(titlePrefab, listRoot.transform));
                cardList[count].instantiated = true;
            }
            if (card.instantiated)// Set Card Properties
            {
                Vector2 position = cardUIList[count].GetComponent<RectTransform>().anchoredPosition;
                int index = count;
                float spacing = titlePrefab.GetComponent<RectTransform>().rect.height;
                cardUIList[count].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -spacing / 2 + (count * -spacing));
                cardUIList[count].GetComponent<Button>().onClick.RemoveAllListeners();
                cardUIList[count].GetComponent<Button>().onClick.AddListener(() => SelectCard(index)); // Code from https://answers.unity.com/questions/938496/buttononclickaddlistener.html & https://answers.unity.com/questions/1384803/problem-with-onclickaddlistener.html
                cardUIList[count].transform.Find("Title").GetComponent<Text>().text = cardList[count].title;
            }
            count++;
        }

        // Update Scroll Limit for Titles
        if (count > 5)
        { titleScrolling.listLength = (count - 5) * titlePrefab.GetComponent<RectTransform>().rect.height; }
        else
        { titleScrolling.listLength = 0; }
    }

    public void SelectCard(int index) // Select card of index "index" from the cardList, assigning all visuals accordingly
    {
        if (index == -1)
        {
            editorTitle.text = "";
            editorQuestion.text = "";
            editorAnswer.text = "";
            selection = index;
            editorTitle.interactable = false;
            editorQuestion.interactable = false;
            editorAnswer.interactable = false;
            saveButton.interactable = false;
            deleteButton.interactable = false;
        }
        else
        {
            editorTitle.text = cardList[index].title;
            editorQuestion.text = cardList[index].question;
            editorAnswer.text = cardList[index].answer;
            selection = index;
            editorTitle.interactable = true;
            editorQuestion.interactable = true;
            editorAnswer.interactable = true;
            saveButton.interactable = true;
            deleteButton.interactable = true;
        }
    }

    public void CreateCard() // Create a new, empty card
    {
        cardList.Add(new Card("Untitled", "", "", false));
        SelectCard(cardList.Count - 1);
        UpdateCardList();
    }

    public void SaveCard() // Save all changes to the selected card
    {
        if (selection != -1)
        {
            if (editorTitle.text == "")
            {
                cardList[selection] = new Card("Untitled", editorQuestion.text, editorAnswer.text, cardList[selection].instantiated);
            }
            else if (editorQuestion.text != "" && editorAnswer.text != "")
            {
                cardList[selection] = new Card(editorTitle.text, editorQuestion.text, editorAnswer.text, cardList[selection].instantiated);
            }
            SelectCard(-1);
            UpdateCardList();
        }
    }

    public void DeleteCard() // Delete the card's info and prefab
    {
        cardList.RemoveAt(selection);
        Destroy(cardUIList[selection]);
        cardUIList.RemoveAt(selection);
        SelectCard(-1);
        UpdateCardList();
    }

    public void DeleteCardUI() // Delete all card prefabs, and mark the data as "uninstantiated"
    {
        foreach (GameObject gameObject in cardUIList)
        { Destroy(gameObject); }
        int limit = cardList.Count;
        for (int i = 0; i < limit; i++)
        { cardList[i].instantiated = false; }
        cardUIList.Clear();
    }
}

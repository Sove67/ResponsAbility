using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class Card_Handler : MonoBehaviour
{
    // Variables
    // General
    public List<Card_Handler.Card> dataList = new List<Card_Handler.Card>();
    readonly private List<GameObject> UIList = new List<GameObject>();
    private int selection = -1;

    // Enviroment
    public GameObject listRoot;
    public GameObject titlePrefab;
    public ToggleGroup titleToggleGroup;
    public AudioSource audioSource;

    // Input
    public Button deleteButton;
    public InputField editorTitle;
    public InputField editorQuestion;
    public InputField editorAnswer;
    public Scrolling scrolling;

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
        scrolling.listLength = 0;
        scrolling.UpdateLimits();
    }

    public void UpdateList() // run through each set of cards, and create/update a prefab to match it's data
    {
        int count = 0;
        foreach (var card in dataList) // Update All Cards
        {
            if (!card.instantiated) // Create Title Card.
            {
                UIList.Add(Instantiate(titlePrefab, listRoot.transform));
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
            UIList[count].GetComponent<Toggle>().onValueChanged.AddListener((isOn) => audioSource.Play()); // Code from https://answers.unity.com/questions/902399/addlistener-to-a-toggle.html
            UIList[count].transform.Find("Title").GetComponent<Text>().text = dataList[count].title;
            
            count++;
        }

        // Update Scroll Limit for Titles
        scrolling.listLength = count * titlePrefab.GetComponent<RectTransform>().rect.height;
        scrolling.UpdateLimits();
    }

    public void ParseToggleToSelect(bool isOn, int index) // Set values based on if the toggle calling is active. Otherwise, remove values if no toggle is active.
    {
        if (isOn)
        { Select(index); }
        else if (!titleToggleGroup.AnyTogglesOn())
        { Select(-1); }
    }

    public void Select(int newSelection) // Select card of index "index" from the dataList, assigning all visuals accordingly
    {
        if (newSelection == -1)
        {

            // Remove the listeners from the buttons that will be affected
            editorTitle.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
            editorQuestion.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
            editorAnswer.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
            // Change the values
            editorTitle.text = "";
            editorQuestion.text = "";
            editorAnswer.text = "";
            // Re-apply the listeners
            editorTitle.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
            editorQuestion.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
            editorAnswer.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.RuntimeOnly);

            // Remove the listeners from the buttons that will be affected
            if (0 < selection && selection < UIList.Count) 
            {
                UIList[selection].GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
            }
            // Change the values
            titleToggleGroup.SetAllTogglesOff(); 
            // Re-apply the listeners
            if (0 < selection && selection < UIList.Count)
            {
                UpdateList();
            }


            editorTitle.interactable = false;
            editorQuestion.interactable = false;
            editorAnswer.interactable = false;
            deleteButton.interactable = false;
        }
        else
        {

            // Remove the listeners from the buttons that will be affected
            editorTitle.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
            editorQuestion.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
            editorAnswer.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
            // Change the values
            editorTitle.text = dataList[newSelection].title;
            editorQuestion.text = dataList[newSelection].question;
            editorAnswer.text = dataList[newSelection].answer;
            // Re-apply the listeners
            editorTitle.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
            editorQuestion.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
            editorAnswer.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.RuntimeOnly);


            // Remove the listeners from the buttons that will be affected
            UIList[newSelection].GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
            // Change the values
            titleToggleGroup.SetAllTogglesOff();
            UIList[newSelection].GetComponent<Toggle>().isOn = true;
            // Re-apply the listeners
            UpdateList();


            editorTitle.interactable = true;
            editorQuestion.interactable = true;
            editorAnswer.interactable = true;
            deleteButton.interactable = true;
        }

        selection = newSelection;
    }

    public void CreateCard() // Create a new, empty card
    {
        dataList.Add(new Card("Untitled", "Question", "Answer", false));
        Select(dataList.Count - 1);
        UpdateList();
    }

    public void SaveCard() // Save all changes to the selected card
    {
        if (selection != -1)
        {
            string title = editorTitle.text;
            string question = editorQuestion.text;
            string answer = editorAnswer.text;

            if (editorTitle.text == "")
            { title = "Untitled"; }
            if (editorQuestion.text == "")
            { question = "Question"; }
            if (editorAnswer.text == "")
            { answer = "Answer"; }

            dataList[selection] = new Card(title, question, answer, dataList[selection].instantiated);

            UpdateList();
        }
    }

    public void DeleteCard() // Delete the card's info and prefab
    {
        dataList.RemoveAt(selection);
        Destroy(UIList[selection]);
        UIList.RemoveAt(selection);
        Select(-1);
        UpdateList();
    }

    public void DeleteCardUI() // Delete all card prefabs, and mark the data as "uninstantiated"
    {
        foreach (GameObject gameObject in UIList)
        { Destroy(gameObject); }
        int limit = dataList.Count;
        for (int i = 0; i < limit; i++)
        { dataList[i].instantiated = false; }
        UIList.Clear();
    }
}

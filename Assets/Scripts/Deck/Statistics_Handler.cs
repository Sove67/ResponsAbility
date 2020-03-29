using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Statistics_Handler : MonoBehaviour
{
    // Variables
    // General
    public Deck_Handler deck_handler;
    private Deck_Handler.Deck currentDeck;

    // Detailed Marks
    public int selectedPractice;
    public InputField sessionInput;
    public GameObject markContainer;
    public GameObject markPrefab;
    public List<GameObject> markUIList = new List<GameObject>();

    // Output
    public Text practiceCount;
    public Text recentScore;
    public Text averageScore;
    public Text recentAverageScore;

    // Enviroment
    public MeshFilter graph;
    public RectTransform container;
    public Scrolling titleScrolling;

    // Functions
    public void UpdateContent() // assign all visual values to the parameters in the selected deck
    {
        currentDeck = deck_handler.deckList[deck_handler.selection];

        practiceCount.text = currentDeck.practiceSessions.Count.ToString(); // # of practices
        recentScore.text = ParseGrade(currentDeck.practiceSessions[currentDeck.practiceSessions.Count-1].grade); // most recent score

        float avgScoreTotal = 0f;
        for (int i = 0; i < currentDeck.practiceSessions.Count; i++)
        {
             avgScoreTotal += currentDeck.practiceSessions[i].grade;
        }
        averageScore.text = ParseGrade(avgScoreTotal / currentDeck.practiceSessions.Count); // average score for 3 most recent

        float recentAvgScoreTotal = 0f;
        int count = 0;
        for (int i = 3; i > 0; i--)
        {
            if (currentDeck.practiceSessions.Count >= i)
            {
                recentAvgScoreTotal += currentDeck.practiceSessions[currentDeck.practiceSessions.Count - i].grade;
                count++;
            }
        }
        recentAverageScore.text = ParseGrade(recentAvgScoreTotal / count); // average score for 3 most recent

        Graph_Renderer.GenerateGraph(currentDeck, container, graph);
        UpdateMarkSet();
    }

    static public string ParseGrade(float grade)
    {
        string text = (grade * 100).ToString();

        if (text.Length > 4)
        { text = text.Substring(0, 4); }

        return text + "%";
    }

    public void SelectSession(int mod)
    {
        int newSelection = 0;
        if (mod == 0) // Input Field
        { newSelection = (int.Parse(sessionInput.text) - 1); }
        else // Buttons
        { newSelection = selectedPractice + mod; }

        // Check input is within bounds, reject if not
        if (0 <= newSelection && newSelection < currentDeck.practiceSessions.Count)
        { 
            // Clear old list if it was changed
            if (selectedPractice != newSelection)
            { ClearMarkSet(); }
            selectedPractice = newSelection;
            sessionInput.text = (selectedPractice + 1).ToString();

            UpdateMarkSet();
        }
        else
        { sessionInput.text = (selectedPractice + 1).ToString(); }
    }

    public void UpdateMarkSet()
    {
        int count = 0;
        Deck_Practice.Mark[] details = currentDeck.practiceSessions[selectedPractice].details;
        foreach (var detail in details) // Update All Cards
        {
            if (!detail.instantiated) // Create Title Card.
            {
                GameObject newNoteUI = Instantiate(markPrefab, markContainer.transform);
                markUIList.Add(newNoteUI);
                details[count].instantiated = true;
            }
            if (detail.instantiated)// Set Card Properties
            {
                Vector2 position = markUIList[count].GetComponent<RectTransform>().anchoredPosition;
                int index = count;
                float spacing = markPrefab.GetComponent<RectTransform>().rect.height;
                markUIList[count].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -spacing / 2 + (count * -spacing));
                markUIList[count].transform.Find("Question Text").GetComponent<Text>().text = details[count].question;
                markUIList[count].transform.Find("Answer Text").GetComponent<Text>().text = details[count].expectedAnswer;
                markUIList[count].transform.Find("Result Text").GetComponent<Text>().text = details[count].givenAnswer;

                if (currentDeck.practiceSessions[selectedPractice].details[count].correct)
                {
                    markUIList[count].transform.Find("Correct Indicator").gameObject.SetActive(true);
                    markUIList[count].transform.Find("Incorrect Indicator").gameObject.SetActive(false);
                }
                else
                {
                    markUIList[count].transform.Find("Correct Indicator").gameObject.SetActive(false);
                    markUIList[count].transform.Find("Incorrect Indicator").gameObject.SetActive(true);
                }
            }
            count++;
        }

        // Update Scroll Limit for Titles
        titleScrolling.listLength = (count) * markPrefab.GetComponent<RectTransform>().rect.height;
        titleScrolling.UpdateLimits();
    }

    public void ClearMarkSet()
    {
        foreach (GameObject gameObject in markUIList)
        { Destroy(gameObject); }
        int limit = currentDeck.practiceSessions[selectedPractice].details.Length;
        for (int i = 0; i < limit; i++)
        { currentDeck.practiceSessions[selectedPractice].details[i].instantiated = false; }
        markUIList.Clear();
    }
}

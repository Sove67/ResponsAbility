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

    // Graph
    public MeshFilter graph;
    public RectTransform container;
    public GameObject graphError;
    public GameObject graphPointer;

    // Detailed Marks
    public int selectedPractice;
    public InputField sessionInput;
    public GameObject markContainer;
    public GameObject markPrefab;
    public List<GameObject> markUIList = new List<GameObject>();
    public RectTransform header;
    public Text sessionMark;

    // Detailed Text
    public GameObject detailedTextGUI;
    public Text detailedText;

    // Output
    public Text practiceCount;
    public Text recentScore;
    public Text averageScore;
    public Text recentAverageScore;

    // Enviroment
    public Scrolling markScrolling;

    // Functions
    public void UpdateContent() // assign all visual values to the parameters in the selected deck
    {
        currentDeck = deck_handler.dataList[deck_handler.selection];
        practiceCount.text = currentDeck.practiceSessions.Count.ToString(); // # of practices
        recentScore.text = ParseGrade(currentDeck.practiceSessions[currentDeck.practiceSessions.Count - 1].grade); // most recent score

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

        if (Graph_Renderer.GenerateGraph(currentDeck, container.rect, graph))
        { 
            graphError.SetActive(false);
            graphPointer.SetActive(true);
        }
        else
        { 
            graphError.SetActive(true);
            graphPointer.SetActive(false);
        }

        sessionInput.text = "1";
        SelectSession(0);
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
        int newSelection;
        if (mod == 0) // Input from Text Field
        { newSelection = (int.Parse(sessionInput.text) - 1); }
        else // Input from Buttons
        { newSelection = selectedPractice + mod; }

        // Check input is within bounds 
        if (0 <= newSelection && newSelection < currentDeck.practiceSessions.Count)
        {
            ClearDetails();
            selectedPractice = newSelection;
            sessionInput.text = (selectedPractice + 1).ToString();
            sessionMark.text = ParseGrade(currentDeck.practiceSessions[selectedPractice].grade);
            UpdateMarkSet();

            // Move Pointer In Graph
            if (graphPointer.activeSelf)
            {
                graphPointer.GetComponent<RectTransform>().anchoredPosition = graph.mesh.vertices[selectedPractice * 2 + 1];
            }
        }
        else // reject if not
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
                int index = count;
                float spacing = markPrefab.GetComponent<RectTransform>().rect.height;
                markUIList[count].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -spacing / 2 + (count * -spacing));

                string question = details[count].question;
                markUIList[count].transform.Find("Question Text").GetComponent<Text>().text = question;
                markUIList[count].transform.Find("Question Text").GetComponent<Button>().onClick.RemoveAllListeners();
                markUIList[count].transform.Find("Question Text").GetComponent<Button>().onClick.AddListener(() => DetailedText(question)); // Code from https://answers.unity.com/questions/938496/buttononclickaddlistener.html & https://answers.unity.com/questions/1384803/problem-with-onclickaddlistener.html

                string answer = details[count].answer;
                markUIList[count].transform.Find("Answer Text").GetComponent<Text>().text = answer;
                markUIList[count].transform.Find("Answer Text").GetComponent<Button>().onClick.RemoveAllListeners();
                markUIList[count].transform.Find("Answer Text").GetComponent<Button>().onClick.AddListener(() => DetailedText(answer)); // Code from https://answers.unity.com/questions/938496/buttononclickaddlistener.html & https://answers.unity.com/questions/1384803/problem-with-onclickaddlistener.html

                string input = details[count].input;
                markUIList[count].transform.Find("Input Text").GetComponent<Text>().text = input;
                markUIList[count].transform.Find("Input Text").GetComponent<Button>().onClick.RemoveAllListeners();
                markUIList[count].transform.Find("Input Text").GetComponent<Button>().onClick.AddListener(() => DetailedText(input)); // Code from https://answers.unity.com/questions/938496/buttononclickaddlistener.html & https://answers.unity.com/questions/1384803/problem-with-onclickaddlistener.html

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
        markScrolling.listLength = ((count) * markPrefab.GetComponent<RectTransform>().rect.height) + header.rect.height;
        markScrolling.UpdateLimits();
    }

    public void ClearDetails()
    {
        foreach (GameObject gameObject in markUIList)
        { Destroy(gameObject); }
        for (int i = 0; i < currentDeck.practiceSessions[selectedPractice].details.Length; i++)
        { currentDeck.practiceSessions[selectedPractice].details[i].instantiated = false; }

        markUIList.Clear();
    }

    public void DetailedText(string text)
    {
        detailedTextGUI.SetActive(true);
        detailedText.text = text;
    }
}

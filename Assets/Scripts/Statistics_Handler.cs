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

    // Output
    public Text practiceCount;
    public Text recentScore;
    public Text averageScore;
    public Text recentAverageScore;

    // Enviroment
    public MeshFilter graph;
    public RectTransform container;

    // Functions
    public void UpdateContent() // assign all visual values to the parameters in the selected deck
    {
        currentDeck = deck_handler.deckList[deck_handler.selection];

        practiceCount.text = currentDeck.mark.Count.ToString(); // # of practices
        recentScore.text = ParseGrade(currentDeck.mark[currentDeck.mark.Count-1].grade); // most recent score

        float avgScoreTotal = 0f;
        for (int i = 0; i < currentDeck.mark.Count; i++)
        {
             avgScoreTotal += currentDeck.mark[i].grade;
        }
        averageScore.text = ParseGrade(avgScoreTotal / currentDeck.mark.Count); // average score for 3 most recent

        float recentAvgScoreTotal = 0f;
        int count = 0;
        for (int i = 3; i > 0; i--)
        {
            if (currentDeck.mark.Count >= i)
            {
                recentAvgScoreTotal += currentDeck.mark[currentDeck.mark.Count - i].grade;
                count++;
            }
        }
        recentAverageScore.text = ParseGrade(recentAvgScoreTotal / count); // average score for 3 most recent

        Graph_Renderer.GenerateGraph(currentDeck, container, graph);
        UpdateMarkSet();
    }

    public string ParseGrade(float grade)
    {
        string text = (grade * 100).ToString();

        if (text.Length > 4)
        { text = text.Substring(0, 4); }

        return text + "%";
    }

    public void UpdateGraph()
    {
        // Visual for improvement
    }

    public void UpdateMarkSet()
    {
        // All marks ordered by date
    }
}

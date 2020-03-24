using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Statistics_Handler : MonoBehaviour
{
    // Variables
    // General
    public Deck_Handler flashcard_handler;
    private Deck_Handler.Deck currentDeck;
    private int reminderInt;
    // Output
    public Text practiceCount;
    public Text recentScore;
    public Text averageScore;
    public Text recentAverageScore;
    public Text reminderInterval;


    // Functions
    public void UpdateContent() // assign all visual values to the parameters in the selected deck
    {
        currentDeck = flashcard_handler.deckList[flashcard_handler.selection];

        practiceCount.text = currentDeck.mark.Count.ToString(); // # of practices
        recentScore.text = (currentDeck.mark[currentDeck.mark.Count-1].grade * 100).ToString().Substring(0, 4) + "%"; // most recent score

        float avgScoreTotal = 0f;
        for (int i = 0; i < currentDeck.mark.Count; i++)
        {
             avgScoreTotal += currentDeck.mark[i].grade;
        }
        averageScore.text = (avgScoreTotal / currentDeck.mark.Count * 100).ToString().Substring(0, 4) + "%"; // average score for 3 most recent

        float recentAvgScoreTotal = 0f;
        int count = 0;
        for (int i = 3; i > 0; i--)
        {
            Debug.Log(i);
            if (currentDeck.mark.Count >= i)
            {
                recentAvgScoreTotal += currentDeck.mark[currentDeck.mark.Count - i].grade;
                count++;
            }
        }
        recentAverageScore.text = (recentAvgScoreTotal / count * 100).ToString().Substring(0,4) + "%"; // average score for 3 most recent

        ChangeReminder(currentDeck.reminderPeriod);
        UpdateGraph();
        UpdateMarkSet();
    }

    public void ChangeReminder(int mod) // change the reminder period by a step size of "mod", allowing for a time period of Never, Hours, and Days
    {
        reminderInt += mod;
        if (reminderInt <= 0)
        {
            reminderInt = 0;
            reminderInterval.text = "Never";
        }
        if (0 < reminderInt && reminderInt < 24)
        {
            reminderInterval.text = (reminderInt).ToString() + " Hour";
            if (reminderInt > 1)
            { reminderInterval.text += "s"; }
        }
        if (reminderInt >= 24)
        {
            reminderInterval.text = (reminderInt - 23).ToString() + " Day";
            if (reminderInt > 24)
            { reminderInterval.text += "s"; }
        }
        currentDeck.reminderPeriod = reminderInt;
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

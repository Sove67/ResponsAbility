using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Statistics_Handler : MonoBehaviour
{
    // Variables
    public List<GameObject> mixedContainers;
    int selectedTitle;
    public List<Material> colourOptions;
    public GameObject titlePrefab;
    public Text generalBestStreak;
    public Text generalCurrentStreak;
    public Text flashcardPractices;
    public InputField flashcardRecentScoreRange;
    public List<List<Text>> flashcardScoreArray;
    public InputField flashcardReminder;
    // Visual for improvement


    // Classes


    // Functions
    public void generalContent()
    {
        // Use Records
        // Total Time Readout
        // Days in a row this app was used

        // Input Records
        // # of words typed
        // # of characters typed
        // # of notes created
        // # of decks created
        // # of cards created
        // # of practices
        // average score for x most recent
        // All marks ordered by date
        // Visual for improvement
        // set a reminder if x time has passed since last study
    }
}

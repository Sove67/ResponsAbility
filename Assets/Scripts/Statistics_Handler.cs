using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statistics_Handler : MonoBehaviour
{
    // Variables
    // Universal Data
    public Dropdown typeSelector;
    public GameObject contentRoot;
    int selectedTitle;

    // 'General Info' Data
    public Text MainTotalTime;
    public Text MainWordsTyped;
    public Text MainLettersTyped;
    public Text MainNotesCreated;
    public Text MainDecksCreated;
    public Text MainCardsCreated;
    public Text MainStreak;

    // 'Note Info' Data

    public Text NoteTotalTime;
    public Text NoteWordsTyped;
    public Text NoteLettersTyped;
    public Text NoteLinesTyped;
    public InputField NoteReminder;

    // 'Flashcard Info' Data

    public Text FlashcardPractices;
    public InputField FlashcardRecentScoreRange;
    public List<List<Text>> FlashcardScoreArray;
    public InputField FlashcardReminder;
    // Visual for improvement


    // Functions
    public void OnTypeSelection()
    {
        int selectedField = typeSelector.value;

        // Hide all visuals
        foreach (Transform child in contentRoot.transform)
        { child.gameObject.SetActive(false); }

        if (selectedField == 0)
        {
            // Main Style
            MainContent();
        }

        else if (selectedField == 1)
        {
            // Note Style
            NoteTitleList();
            NoteTitleSelect(-1);
        }

        else if (selectedField == 2)
        {
            // Flashcard Style
            FlashcardTitleList();
            FlashcardTitleSelect(-1);
        }
    }


    // Main Info
    public void MainContent()
    {
        // Use Records
        // Total Time Readout
        // Days in a row this app was used

        // Input Records
        // # of words typed
        // # of letters typed
        // # of notes created
        // # of decks created
        // # of cards created
    }


    // Note Info
    public void NoteTitleList()
    {

    }

    public void NoteTitleSelect(int selection)
    {
        // Total time viewed
        // # of words typed
        // # of letters typed
        // # of lines
        // set a reminder if x time has passed since last study
    }

    // Flashcard Info
    public void FlashcardTitleList()
    {

    }
    public void FlashcardTitleSelect(int selection)
    {
        // # of practices
        // average score for x most recent
        // All marks ordered by date
        // Visual for improvement
        // set a reminder if x time has passed since last study
    }
}

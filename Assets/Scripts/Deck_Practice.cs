using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Deck_Practice : MonoBehaviour
{
    // Variables
    // General
    private List<Card_Handler.Card> cardList = new List<Card_Handler.Card>();
    private int selection = -1;
    private int chosenAnswer;
    private int correctAnswer;
    private int[] markListIndex;
    private Mark[] markList;

    // Enviroment
    public GameObject window;
    public GameObject resultGUI;
    public GameObject recentScoreGUI;
    public GameObject colourIndicator;
    public GameObject multipleChoiceWindow;

    // Inputs
    public Toggle shuffleToggle;
    public Toggle multipleChoiceToggle;
    public Toggle[] multipleChoices = new Toggle[4];
    public Button submitAnswer;
    public InputField answerInput;

    // Outputs
    public Text recentScoreText;
    public Text resultGUIResult;
    public Text resultGUIAnswer;
    public Text deckTitle;
    public Text cardTitle;
    public Text question;

    // Other
    public Deck_Handler deck_handler;
    public List<Material> colourOptions;


    // Classes
    [Serializable] public class SessionResult // The marks given for one practice session for a given deck
    {
        public float grade { get; set; }
        public Mark[] details { get; set; }
        public SessionResult(float grade, Mark[] details)
        {
            this.grade = grade;
            this.details = details;
        }
    }
    [Serializable] public class Mark // The marks given for one practice session for a given deck
    {
        public string question { get; set; }
        public string expectedAnswer { get; set; }
        public string givenAnswer { get; set; }
        public bool correct { get; set; }
        public bool instantiated { get; set; }
        public Mark(string question, string expectedAnswer, string givenAnswer, bool correct, bool instantiated)
        {
            this.question = question;
            this.expectedAnswer = expectedAnswer;
            this.givenAnswer = givenAnswer;
            this.correct = correct;
            this.instantiated = instantiated;
        }
    }


    // Functions
    public void LoadPracticeDeck() // Start a practice session with the selected deck and settings
    {
        colourIndicator.GetComponent<Image>().material = colourOptions[deck_handler.deckList[deck_handler.selection].colour];
        if (shuffleToggle.isOn) // if shuffling deck
        {
            cardList = new List<Card_Handler.Card>(deck_handler.deckList[deck_handler.selection].content);
            markListIndex = new int[cardList.Count];

            for (int i = 0; i < cardList.Count; i++)
            {
                Card_Handler.Card temp1 = cardList[i];
                int temp2 = markListIndex[i];

                int randomIndex = UnityEngine.Random.Range(i, cardList.Count);

                cardList[i] = cardList[randomIndex];
                cardList[randomIndex] = temp1;

                markListIndex[i] = randomIndex;
                markListIndex[randomIndex] = temp2;
            }
        }
        else
        { cardList = deck_handler.deckList[deck_handler.selection].content; }

        if (multipleChoiceToggle.isOn) // If using multiple choice
        {
            multipleChoiceWindow.gameObject.SetActive(true);
            answerInput.gameObject.SetActive(false);
        }
        else
        {
            multipleChoiceWindow.gameObject.SetActive(false);
            answerInput.gameObject.SetActive(true);
        }

        //Empty the list
        markList = new Mark[cardList.Count];

        deckTitle.text = deck_handler.deckList[deck_handler.selection].title;
        LoadPracticeCard(-1);
    }

    public void LoadPracticeCard(int mod) // Either load the first card, or load the card ahead by an step size of "mod"
    {
        submitAnswer.interactable = false;
        if (mod == -1) // If choosing first card
        {
            selection = 0;
            cardTitle.text = cardList[selection].title;
            question.text = cardList[selection].question;
        }

        else if (selection + mod < cardList.Count) // If choosing card within deck
        {
            selection += mod;
            cardTitle.text = cardList[selection].title;
            question.text = cardList[selection].question;
        }

        else
        { Finish(); }

        if (multipleChoiceToggle.isOn) // if multiple choice
        {
            // Create list of options
            List<int> numbersToChooseFrom = new List<int>();
            for (int i = 0; i < cardList.Count; i++)
            {
                numbersToChooseFrom.Add(i);
            }
            numbersToChooseFrom.Remove(selection);

            for (int i = 0; i < 4; i++) // Assign random answers
            {
                int listIndex = UnityEngine.Random.Range(0, numbersToChooseFrom.Count);
                int randomIndex = numbersToChooseFrom[listIndex];
                numbersToChooseFrom.RemoveAt(listIndex);

                multipleChoices[i].GetComponentInChildren<Text>().text = cardList[randomIndex].answer;
            }

            // Assign one correct answer
            correctAnswer = UnityEngine.Random.Range(0, 3);
            multipleChoices[correctAnswer].GetComponentInChildren<Text>().text = cardList[selection].answer;

            // Clear the last answer
            for (int i = 0; i < multipleChoices.Length; i++)
            {
                multipleChoices[i].isOn = false;
            }
        }
        else
        {
            // Clear the last answer
            answerInput.text = "";
        }
    }

    public void InputButtonToggle()
    {
        if (answerInput.text != "")
        { submitAnswer.interactable = true; }
        else
        { submitAnswer.interactable = false; }
    }

    public void ChooseAnswer() // Update the selected answer based on the toggle selected
    {
        bool active = false;
        for (int i = 0; i < multipleChoices.Length; i++)
        {
            if (multipleChoices[i].isOn)
            {
                chosenAnswer = i;
                active = true;
            }
        }
        submitAnswer.interactable = active;
    }

    public void CheckAnswer() // Match the given answer to the expected answer, and send "true" if correct
    {
        bool correct = false;
        string givenAnswer = "Null";
        if ((multipleChoiceToggle.isOn && chosenAnswer == correctAnswer) || (answerInput.text == cardList[selection].answer)) // If looking for multiple choice & correct
        {
            correct = true;

            if (multipleChoiceToggle.isOn)
            { givenAnswer = multipleChoices[chosenAnswer].GetComponentInChildren<Text>().text; }
            else
            { givenAnswer = answerInput.text; }

            resultGUIResult.text = "Correct!";
            resultGUIAnswer.text = "";
        }
        else
        {
            resultGUIResult.text = "Incorrect.";
            resultGUIAnswer.text = "The answer was: " + cardList[selection].answer;
        }

        Card_Handler.Card currentContent = deck_handler.deckList[deck_handler.selection].content[selection];

        markList[selection] = new Mark(currentContent.question, currentContent.answer, givenAnswer, correct, false); // Record Result
        resultGUI.SetActive(true);
    }

    public void Finish() // Complete the practice session by adding the marks to the deck and opening a pop-up to display the mark
    {
        window.SetActive(false);

        float count = 0;
        Mark[] organizedMarkList = new Mark[markList.Length];

        for (int i = 0; i < markList.Length; i++)
        {
            if (shuffleToggle.isOn)
            { organizedMarkList[i] = markList[markListIndex[i]]; } // Sort the marks by the shuffled index

            if (markList[i].correct)
            { count++; }
        }

        if (shuffleToggle.isOn)
        { deck_handler.deckList[deck_handler.selection].practiceSessions.Add(new SessionResult(count / markList.Length, organizedMarkList)); }
        else
        { deck_handler.deckList[deck_handler.selection].practiceSessions.Add(new SessionResult(count / markList.Length, markList)); }

        recentScoreGUI.SetActive(true);
        recentScoreText.text = count.ToString() + " out of " + markList.Length.ToString() + " answers correct!";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Header : MonoBehaviour
{
    public Text date;
    public Text fact;
    public void Update()
    {
        Date();
    }
    public void Start()
    {
        FunFact();
    }

    public void Date()
    {
        date.text = System.DateTime.Now.ToString("ddd, MMM d, h:mm");
    }

    public void FunFact()
    {
        string[] funFacts = new string[]
        {
            "You can improve the quality of your study notes if you imagine you're making them for someone else.",
            "20 minutes of exercise before an exam will improve your performance!",
            "Chewing gum is actually a brain booster, one study found a 40 per cent increase in memory test scores.",
            "Studying at a green surrounding or glancing at colour green can make you more creative.",
            "Drawing diagrams help you memorize them.",
            "You’re more likely to retain information from your class if you review what you’ve learned every day.",
            "Changing your environment helps you retain what you're studying.",
            "Music helps improve language skills",
            "Losing one nights sleep can impair reasoning and memory for up to four days.",
            "Foreign languages encourage growth in your brain.",
            "Studying the most challenging material right before sleeping makes it easier to remember the next morning.",
            "The harder something is to remember, the harder it is to forget.",
            "To improve your memory of a subject, practice at spaced intervals over time"
        };

        fact.text = "Did you know? " + funFacts[Random.Range(0, funFacts.Length - 1)];
    }
}
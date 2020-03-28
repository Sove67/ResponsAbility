using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Notifications.Android;

public class Reminder_Handler : MonoBehaviour
{
    // Variables
    public Text periodText;
    public Deck_Handler deck_handler;

    bool days;

    // Classes
    [Serializable] public class Reminder // The details of one flashcard deck, including the cards created and marks received
    {
        public int? ID { get; set; }
        public TimeSpan period { get; set; }
        public DateTime date { get; set; }
        public Reminder( int? ID, TimeSpan Period, DateTime date)
        {
            this.ID = ID;
            this.period = Period;
            this.date = date;
        }
    }

    // Functions
    void Start()
    { 
        CreateNotificationChannel();
    }

    void CreateNotificationChannel() // Create the default notification channel
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "default",
            Name = "Study Reminder",
            Importance = Importance.High,
            Description = "Reminds the user to study after the interval they set has eclipsed",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    public int SendRepeatNotification(string title, string text, System.TimeSpan period) // Start a notification that repeats every 'period'
    {
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = text;
        notification.FireTime = DateTime.Now + period;
        notification.RepeatInterval = period;

        int ID = AndroidNotificationCenter.SendNotification(notification, "default");
        return ID;
    }

    public void ChangeReminder(int mod) // change the reminder period by a step size of "mod", allowing for a time period of Never, Hours, and Days
    {
        Deck_Handler.Deck currentDeck = deck_handler.deckList[deck_handler.selection];

        if (currentDeck.reminder.period.Days + mod >= 0) 
        {
            currentDeck.reminder.period += new TimeSpan(mod, 0, 0, 0);

            periodText.text = currentDeck.reminder.period.Days + " Day";

            if (currentDeck.reminder.period.Days > 1) // Plural
            { periodText.text += "s"; }
            else if (currentDeck.reminder.period.Days == 0) // None
            { periodText.text = "Never"; }
        }

        if (currentDeck.reminder != null) // If a reminder is assigned, remove it
        {
            int ID = currentDeck.reminder.ID ?? default(int); // Parsing nullable int into an int. Taken from:https://stackoverflow.com/questions/5995317/how-to-convert-c-sharp-nullable-int-to-int/5995418
            AndroidNotificationCenter.CancelNotification(ID);
        }

        if (currentDeck.reminder.period > TimeSpan.Zero) // if a period is assigned schedule a notification
        { currentDeck.reminder.ID = SendRepeatNotification("Practice Reminder", "Don't forget to practice the " + currentDeck.title + " deck!", currentDeck.reminder.period); }
    }
}
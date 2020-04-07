using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Unity.Notifications.Android;
using UnityEngine.Networking;
public class Save_Load : MonoBehaviour // Most of this script is adapted from https://www.youtube.com/watch?v=zAhjm_-Y-SA
{
    private readonly string dataPath = "/Save.dat";
    private readonly string tutorialPath = "/Tutorial.dat";
    private Save_File loadedFile;

    public Note_Handler note_handler;
    public Deck_Handler deck_handler;
    public Settings settings;
    public Scrolling note_title_scroller;
    public Scrolling deck_title_scroller;

    private void Awake()
    {
        // If program has saved a file before, load it.
        if (File.Exists(Application.persistentDataPath + dataPath))
        {
            Load(Application.persistentDataPath + dataPath);
        }

        // Otherwize, it must be the first opening, so load the tutorial.
        else
        {
#if UNITY_ANDROID // Need to extract data from compressed file using WebLoad if running on an android
            WebLoad(Application.streamingAssetsPath + tutorialPath, Application.persistentDataPath + dataPath);
            Load(Application.persistentDataPath + dataPath);

#else // Just load the data otherwise
            Load(Application.streamingAssetsPath + tutorialPath);
#endif
        }
    }

    void OnApplicationQuit()
    { Save(note_handler.noteList, deck_handler.deckList, settings.audioToggle.isOn); }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        { Save(note_handler.noteList, deck_handler.deckList, settings.audioToggle.isOn); }
    }

    void Save(List<Note_Handler.Note> noteList, List<Deck_Handler.Deck> deckList, bool audio) // Save the notes and flashcard decks currently available
    {
        FileStream file = null;

        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            if (noteList.Count > 0 || deckList.Count > 0)
            {
                file = File.Create(Application.persistentDataPath + dataPath);
                Save_File dataSet = new Save_File(noteList, deckList, audio);
                bf.Serialize(file, dataSet);
            }
        } 

        catch (Exception e)
        {
            if(e != null)
            { 
                Debug.LogError("File Saving Failed.");
                Debug.LogException(e, this);
            }
        }
        finally
        {
            if(file != null)
            { file.Close(); }
        }
    }

    void Load(string path) // Load the notes and decks that were saved on quit
    {
        FileStream file = null;

        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            file = File.Open(path, FileMode.Open);
            loadedFile = bf.Deserialize(file) as Save_File;

            List<Note_Handler.Note> clearedNoteList = new List<Note_Handler.Note>(loadedFile.noteList);
            List<Deck_Handler.Deck> clearedDeckList = new List<Deck_Handler.Deck>(loadedFile.deckList);

            if (clearedNoteList != null)
            {
                foreach (var note in clearedNoteList)
                { note.instantiated = false; }
            }
            if (clearedDeckList != null)
            {
                foreach (var deck in clearedDeckList)
                {
                    deck.instantiated = false;
                    foreach (var card in deck.content)
                    { card.instantiated = false; }

                    foreach (var session in deck.practiceSessions)
                    {
                        foreach (var detail in session.details)
                        { detail.instantiated = false; }
                    }
                }
            }

            if (clearedNoteList != null && clearedDeckList != null)
            {
                note_handler.noteList = clearedNoteList;
                note_handler.UpdateList();

                deck_handler.deckList = clearedDeckList;
                deck_handler.UpdateList();
            }
            else
            { throw new ArgumentException("Load data is empty."); }

            settings.audioToggle.isOn = loadedFile.audio;
            settings.ToggleAudio();
        }

        catch (Exception e)
        {
            if (e != null)
            {
                Debug.LogError("File Loading Failed.");
                Debug.LogException(e, this);
            }
        }
        finally
        {
            if (file != null)
            { file.Close(); }
        }
    }

    void WebLoad(string inputPath, string outputPath) // Load a file from the compressed StreamingAssets folder on android
    {
        try
        {
            var loadingRequest = UnityWebRequest.Get(inputPath);
            loadingRequest.SendWebRequest();
            while (!loadingRequest.isDone)
            {
                if (loadingRequest.isNetworkError || loadingRequest.isHttpError)
                { throw new ArgumentException("Network Error."); }
            }
            if (!(loadingRequest.isNetworkError || loadingRequest.isHttpError))
            {
                File.WriteAllBytes(outputPath, loadingRequest.downloadHandler.data);
            }
        }
        catch (Exception e)
        {
            if (e != null)
            {
                Debug.LogError("Web File Loading Failed.");
                Debug.LogException(e, this);
            }
        }
    }
    public void Delete() // Save a Save_File with an empty note and deck list, and clear the currently used lists.
    {
        try
        {
            for (int i = note_handler.noteList.Count-1; i >= 0; i--)
            {
                note_handler.Select(i);
                note_handler.Delete();
            }
            note_handler.noteList = new List<Note_Handler.Note>();
            for (int i = deck_handler.deckList.Count - 1; i >= 0; i--)
            {
                deck_handler.Select(i);
                deck_handler.Delete();
            }
            deck_handler.deckList = new List<Deck_Handler.Deck>();

            Save(note_handler.noteList, deck_handler.deckList, settings.audioToggle.isOn);

            AndroidNotificationCenter.CancelAllScheduledNotifications();
        }
        catch (Exception e)
        {
            Debug.LogError("File Deletion Failed.");
            Debug.LogException(e);
        }
    }
}

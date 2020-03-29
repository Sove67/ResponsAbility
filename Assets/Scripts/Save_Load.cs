using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public class Save_Load : MonoBehaviour // Most of this script is adapted from https://www.youtube.com/watch?v=zAhjm_-Y-SA
{
    private readonly string dataPath = "/Save.dat";
    private Save_File loadedFile;

    public Note_Handler note_handler;
    public Deck_Handler deck_handler;
    public Settings settings;
    public Scrolling note_title_scroller;
    public Scrolling deck_title_scroller;

    private void Awake()
    {
        if (File.Exists(Application.persistentDataPath + dataPath))
        { Load(); }
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

            if (noteList != null && deckList != null)
            {
                foreach (var note in noteList)
                { 
                    note.instantiated = false;
                }

                foreach (var deck in deckList)
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

                file = File.Create(Application.persistentDataPath + dataPath);
                Save_File dataSet = new Save_File(noteList, deckList, audio);
                bf.Serialize(file, dataSet);
            }
            else
            { throw new ArgumentException("Save data is empty."); }
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

    void Load() // Load the notes and decks that were saved on quit
    {
        FileStream file = null;

        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            file = File.Open(Application.persistentDataPath + dataPath, FileMode.Open);
            loadedFile = bf.Deserialize(file) as Save_File;

            if (loadedFile.noteList != null && loadedFile.deckList != null)
            {
                note_handler.noteList = loadedFile.noteList;
                note_handler.UpdateList();

                deck_handler.deckList = loadedFile.deckList;
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
    public void Delete()
    {
        try
        {
            File.Delete(Application.persistentDataPath + dataPath);
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
        }
        catch (Exception e)
        {
            Debug.LogError("File Deletion Failed.");
            Debug.LogException(e);
        }
    }
}

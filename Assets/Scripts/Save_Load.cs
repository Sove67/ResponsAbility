using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class Save_Load : MonoBehaviour // Most of this script is adapted from https://www.youtube.com/watch?v=zAhjm_-Y-SA
{
    private readonly string dataPath = "/Save.dat";
    private Save_File loadedFile;

    public Note_Handler note_handler;
    public Flashcard_Handler flashcard_handler;
    public Scrolling note_title_scroller;
    public Scrolling flashcard_title_scroller;
    private void Awake()
    {
        if (File.Exists(Application.persistentDataPath + dataPath))
        { Load(); }
    }

    void OnApplicationQuit()
    {
        Debug.Log("Quit");
        Save(note_handler.noteList, flashcard_handler.deckList);
    }

    void Save(List<Note_Handler.Note> noteList, List<Flashcard_Handler.Deck> deckList)
    {
        FileStream file = null;

        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            if (noteList != null && deckList != null)
            {
                foreach (var note in noteList)
                { note.instatiated = false; }

                foreach (var deck in deckList)
                { 
                    deck.instatiated = false;
                    foreach (var card in deck.content)
                    { card.instatiated = false; }
                }

                file = File.Create(Application.persistentDataPath + dataPath);
                Save_File dataSet = new Save_File(noteList, deckList);
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

    void Load()
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

                flashcard_handler.deckList = loadedFile.deckList;
                flashcard_handler.UpdateDeckList();
            }
            else
            { throw new ArgumentException("Load data is empty."); }
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
}

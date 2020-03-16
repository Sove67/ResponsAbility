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

    private void Awake()
    {
        if (File.Exists(Application.persistentDataPath + dataPath))
        { Load(); }
    }

    void OnApplicationQuit()
    {
        Debug.Log("Quit");
        Save(note_handler.notes, flashcard_handler.decks);
    }

    void Save(List<Note_Handler.Note> notes, List<Flashcard_Handler.Deck> decks)
    {
        FileStream file = null;

        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            if (notes != null && decks != null)
            {
                foreach (var note in notes)
                { note.instatiated = false; }

                foreach (var deck in decks)
                { deck.instatiated = false; }

                file = File.Create(Application.persistentDataPath + dataPath);
                Save_File dataSet = new Save_File(notes, decks);
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

            if (loadedFile.notes != null && loadedFile.decks != null)
            {
                note_handler.notes = loadedFile.notes;
                note_handler.UpdateList();

                flashcard_handler.decks = loadedFile.decks;
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

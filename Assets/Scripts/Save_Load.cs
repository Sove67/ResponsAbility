using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class Save_Load : MonoBehaviour
{
    private readonly string dataPath = "/Save.dat";
    private Save_File currentInstance;

    public Note_Handler note_Handler;

    private void Awake()
    {
        if (File.Exists(Application.persistentDataPath + dataPath))
        { Load(); }
    }

    void OnApplicationQuit()
    {
        Debug.Log("Quit");
        Save(note_Handler.noteInfoList, note_Handler.noteUIList);
    }

    void Save(List<Note_Handler.Note> noteInfoList, List<GameObject> noteUIList)
    {
        FileStream file = null;

        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            if (noteInfoList != null && noteUIList != null)
            {
                foreach (var noteInfo in noteInfoList)
                { noteInfo.instatiated = false; }

                file = File.Create(Application.persistentDataPath + dataPath);
                Save_File dataSet = new Save_File(noteInfoList);
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
            currentInstance = bf.Deserialize(file) as Save_File;

            if (currentInstance.noteInfoList != null)
            {
                note_Handler.noteInfoList = currentInstance.noteInfoList;
                note_Handler.UpdateList();
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

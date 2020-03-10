using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Save_File
{
    private List<Note_Handler.Note> _noteInfoList;

    public Save_File() { }
    public Save_File(List<Note_Handler.Note> noteInfoListParam)
    {
        _noteInfoList = noteInfoListParam;
    }

    public List<Note_Handler.Note> noteInfoList
    {
        get { return _noteInfoList; }
        set { _noteInfoList = value;  }
    }
}

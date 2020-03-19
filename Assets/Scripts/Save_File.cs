using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Save_File // Most of this script is adapted from https://www.youtube.com/watch?v=zAhjm_-Y-SA
{
    private List<Note_Handler.Note> _noteList;
    private List<Flashcard_Handler.Deck> _deckList;

    public Save_File() { }
    public Save_File(List<Note_Handler.Note> noteListParam, List<Flashcard_Handler.Deck> deckListParam)
    {
        _noteList = noteListParam;
        _deckList = deckListParam;
    }

    public List<Note_Handler.Note> noteList
    {
        get { return _noteList; }
        set { _noteList = value;  }
    }

    public List<Flashcard_Handler.Deck> deckList
    {
        get { return _deckList; }
        set { _deckList = value; }
    }
}

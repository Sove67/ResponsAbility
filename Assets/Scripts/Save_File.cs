using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable] public class Save_File // Most of this script is adapted from https://www.youtube.com/watch?v=zAhjm_-Y-SA
{
    private List<Note_Handler.Note> _noteList;
    private List<Deck_Handler.Deck> _deckList;
    private bool _audio;

    public Save_File() { }
    public Save_File(List<Note_Handler.Note> noteListParam, List<Deck_Handler.Deck> deckListParam, bool audioParam)
    {
        _noteList = noteListParam;
        _deckList = deckListParam;
        _audio = audioParam;
    }

    public List<Note_Handler.Note> noteList
    {
        get { return _noteList; }
        set { _noteList = value;  }
    }

    public List<Deck_Handler.Deck> deckList
    {
        get { return _deckList; }
        set { _deckList = value; }
    }
    public bool audio
    {
        get { return _audio; }
        set { _audio = value; }
    }

}

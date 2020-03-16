using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Save_File // Most of this script is adapted from https://www.youtube.com/watch?v=zAhjm_-Y-SA
{
    private List<Note_Handler.Note> _notes;
    private List<Flashcard_Handler.Deck> _decks;

    public Save_File() { }
    public Save_File(List<Note_Handler.Note> notesParam, List<Flashcard_Handler.Deck> decksParam)
    {
        _notes = notesParam;
        _decks = decksParam;
    }

    public List<Note_Handler.Note> notes
    {
        get { return _notes; }
        set { _notes = value;  }
    }

    public List<Flashcard_Handler.Deck> decks
    {
        get { return _decks; }
        set { _decks = value; }
    }
}

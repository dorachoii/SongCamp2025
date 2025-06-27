using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public class MidiReader
{
    public static IEnumerable<Note> LoadNotes(string path)
    {
        var midiFile = MidiFile.Read(path);
        var tempoMap = midiFile.GetTempoMap();
        return midiFile.GetNotes(); 
    }
}


using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public class NoteConverter
{
    public static List<NoteData> Convert(IEnumerable<Note> midiNotes, TempoMap tempoMap)
    {
        List<NoteData> result = new List<NoteData>();

        foreach (var note in midiNotes)
        {
            var pitch = note.NoteNumber;
            var rail = pitch % 6;

            var start = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, tempoMap).TotalSeconds;

            result.Add(new NoteData
            {
                railIdx = rail,
                type = (int)NoteType.SHORT,
                time = (float)start
            });
        }

        return result;
    }
}

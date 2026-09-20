using System;
using System.Collections.Generic;

using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

namespace Project_Snowflake;

// Reads a Standard MIDI File with DryWetMIDI and flattens it into a MidiSong.
// This is the only file that should reference DryWetMIDI types - everything
// downstream (sequencer, synth) works with the plain MidiSong representation.
public static class MidiFileLoader
{
    public static MidiSong Load(string path)
    {
        var midiFile = MidiFile.Read(path);

        if (midiFile.TimeDivision is not TicksPerQuarterNoteTimeDivision timeDivision)
            throw new NotSupportedException("Only tick-based (non-SMPTE) MIDI files are supported.");

        var events = new List<MidiTrackEvent>();
        var tempoChanges = new List<TempoChange>();
        long totalTicks = 0;

        foreach (var timedEvent in midiFile.GetTimedEvents())
        {
            long tick = timedEvent.Time;
            if (tick > totalTicks)
                totalTicks = tick;

            switch (timedEvent.Event)
            {
                case NoteOnEvent noteOn:
                    events.Add(new MidiTrackEvent(tick, MidiEventKind.NoteOn, noteOn.Channel, noteOn.NoteNumber, noteOn.Velocity));
                    break;

                case NoteOffEvent noteOff:
                    events.Add(new MidiTrackEvent(tick, MidiEventKind.NoteOff, noteOff.Channel, noteOff.NoteNumber, noteOff.Velocity));
                    break;

                case ProgramChangeEvent programChange:
                    events.Add(new MidiTrackEvent(tick, MidiEventKind.ProgramChange, programChange.Channel, programChange.ProgramNumber, 0));
                    break;

                case SetTempoEvent setTempo:
                    tempoChanges.Add(new TempoChange(tick, (int)setTempo.MicrosecondsPerQuarterNote));
                    break;
            }
        }

        events.Sort((a, b) => a.Tick.CompareTo(b.Tick));
        tempoChanges.Sort((a, b) => a.Tick.CompareTo(b.Tick));

        // Default to 120 BPM if the file never sets a tempo before the first event.
        if (tempoChanges.Count == 0 || tempoChanges[0].Tick != 0)
            tempoChanges.Insert(0, new TempoChange(0, 500000));

        return new MidiSong(timeDivision.TicksPerQuarterNote, events, tempoChanges, totalTicks);
    }
}

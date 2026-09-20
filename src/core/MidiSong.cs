using System.Collections.Generic;

namespace Project_Snowflake;

public enum MidiEventKind
{
    NoteOn,
    NoteOff,
    ProgramChange
}

public readonly struct MidiTrackEvent
{
    public readonly long Tick;
    public readonly MidiEventKind Kind;
    public readonly int Channel;
    public readonly int Data1; // note number, or program number for ProgramChange
    public readonly int Data2; // velocity (unused for ProgramChange)

    public MidiTrackEvent(long tick, MidiEventKind kind, int channel, int data1, int data2)
    {
        Tick = tick;
        Kind = kind;
        Channel = channel;
        Data1 = data1;
        Data2 = data2;
    }
}

public readonly struct TempoChange
{
    public readonly long Tick;
    public readonly int MicrosecondsPerQuarterNote;

    public TempoChange(long tick, int microsecondsPerQuarterNote)
    {
        Tick = tick;
        MicrosecondsPerQuarterNote = microsecondsPerQuarterNote;
    }
}

public class MidiSong
{
    public int TicksPerQuarterNote { get; }
    public IReadOnlyList<MidiTrackEvent> Events { get; }
    public IReadOnlyList<TempoChange> TempoChanges { get; }
    public long TotalTicks { get; }

    public MidiSong(int ticksPerQuarterNote, IReadOnlyList<MidiTrackEvent> events, IReadOnlyList<TempoChange> tempoChanges, long totalTicks)
    {
        TicksPerQuarterNote = ticksPerQuarterNote;
        Events = events;
        TempoChanges = tempoChanges;
        TotalTicks = totalTicks;
    }
}

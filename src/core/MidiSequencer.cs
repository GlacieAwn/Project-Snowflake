using System.Diagnostics;
using System;
namespace Project_Snowflake;


public class MidiSequencer
{
    private readonly MidiSynth _synth;

    private MidiSong _song;
    private bool _isPlaying;
    private bool _loop;

    private double _currentTick;
    private int _eventIndex;
    private int _tempoIndex;
    private double _microsecondsPerQuarterNote = 500000; // 120 BPM default
    private readonly Stopwatch _stopwatch = new Stopwatch();
    private double _lastElapsedSeconds;

    

    public MidiSequencer(MidiSynth synth)
    {
        _synth = synth;
    }

    public bool IsPlaying => _isPlaying;

    public void Play(MidiSong song, bool loop = false)
    {
        _song = song;
        _loop = loop;
        _currentTick = 0;
        _eventIndex = 0;
        _tempoIndex = 0;
        _microsecondsPerQuarterNote = song.TempoChanges[0].MicrosecondsPerQuarterNote;

        _lastElapsedSeconds = 0;
        _stopwatch.Restart();

        _isPlaying = true;
    }

    public void Stop()
    {
        _isPlaying = false;
        _synth.AllNotesOff();
    }

    public void Update()
    {
        if (!_isPlaying || _song == null)
            return;

        double elapsedSeconds = _stopwatch.Elapsed.TotalSeconds;
        double deltaSeconds = elapsedSeconds - _lastElapsedSeconds;
        _lastElapsedSeconds = elapsedSeconds;

        double ticksPerSecond = _song.TicksPerQuarterNote * 1_000_000.0 / _microsecondsPerQuarterNote;
        double targetTick = _currentTick + deltaSeconds * ticksPerSecond;

        while (_eventIndex < _song.Events.Count && _song.Events[_eventIndex].Tick <= targetTick)
        {
            var midiEvent = _song.Events[_eventIndex];
            AdvanceTempo(midiEvent.Tick);
            Dispatch(midiEvent);
            _eventIndex++;
        }

        AdvanceTempo(targetTick);
        _currentTick = targetTick;

        if (_eventIndex >= _song.Events.Count && _currentTick >= _song.TotalTicks)
        {
            if (_loop)
                Play(_song, loop: true);
            else
                Stop();
        }
    }

    private void AdvanceTempo(double tick)
    {
        var tempoChanges = _song.TempoChanges;
        while (_tempoIndex + 1 < tempoChanges.Count && tempoChanges[_tempoIndex + 1].Tick <= tick)
        {
            _tempoIndex++;
            _microsecondsPerQuarterNote = tempoChanges[_tempoIndex].MicrosecondsPerQuarterNote;
        }
    }

    private void Dispatch(MidiTrackEvent midiEvent)
    {
        double currentTime = _stopwatch.Elapsed.TotalSeconds;
        Console.WriteLine($"[{currentTime:F6}]" + $"[{midiEvent.Kind}]" + $"[Tick={midiEvent.Tick}]");
        switch (midiEvent.Kind)
        {
            case MidiEventKind.NoteOn:
                if (midiEvent.Data2 > 0)
                    _synth.NoteOn(midiEvent.Channel, midiEvent.Data1, midiEvent.Data2);
                else
                    _synth.NoteOff(midiEvent.Channel, midiEvent.Data1);
                break;

            case MidiEventKind.NoteOff:
                _synth.NoteOff(midiEvent.Channel, midiEvent.Data1);
                break;

            case MidiEventKind.ProgramChange:
                _synth.ProgramChange(midiEvent.Channel, midiEvent.Data1);
                break;
        }
    }
}

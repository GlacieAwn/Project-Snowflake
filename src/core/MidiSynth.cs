using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Audio;
using MeltySynth;

namespace Project_Snowflake;

public class MidiSynth : IDisposable
{
    private const int SampleRate = 44100;
    private const int BlockSizeInSamples = SampleRate / 33; // ~30ms of audio per buffer


    private readonly Synthesizer _synthesizer;
    private readonly DynamicSoundEffectInstance _output;
    private readonly byte[] _renderBuffer;

    private readonly object _lock = new();

    public MidiSynth(string soundFontPath)
    {
        _synthesizer = new Synthesizer(soundFontPath, SampleRate);
        _output = new DynamicSoundEffectInstance(SampleRate, AudioChannels.Stereo);
        _renderBuffer = new byte[BlockSizeInSamples * 4]; // stereo, 16-bit samples = 4 bytes/frame

        _output.BufferNeeded += (_, _) => SubmitBuffer();
        _output.Play();


        Console.WriteLine(_synthesizer.MaximumPolyphony);
    }

    public void NoteOn(int channel, int note, int velocity)
    {
        lock (_lock)
            _synthesizer.NoteOn(channel, note, velocity);
    }

    public void NoteOff(int channel, int note)
    {
        lock (_lock)
            _synthesizer.NoteOff(channel, note);
    }

    public void ProgramChange(int channel, int program)
    {
        lock (_lock)
            _synthesizer.ProcessMidiMessage(channel, 0xC0, program, 0);
    }

    public void AllNotesOff()
    {
        lock (_lock)
            _synthesizer.NoteOffAll(immediate: true);
    }

    private void SubmitBuffer()
    {
        lock (_lock)
            _synthesizer.RenderInterleavedInt16(MemoryMarshal.Cast<byte, short>(_renderBuffer));

        _output.SubmitBuffer(_renderBuffer, 0, _renderBuffer.Length);
    }

    public void Dispose()
    {
        _output.Dispose();
    }
}

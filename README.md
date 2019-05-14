# MidiSharp

## Introduction
MidiSharp is a simple C# library written in the .NET framework which allows for easy communication with the MIDI synthesizer built into Windows, or other MIDI devices. It is simple to use, easy to implement and designed for object-orientated programming.

## Usage
To use MidiSharp, simply create a MIDI object using `MIDI midi = new MIDI();`. If you would like to use a different MIDI device, simply enter its ID as a paremeter when constructing the object (e.g. `MIDI midi = new MIDI(3);`).

To get the currently available MIDI devices, use `int deviceCount = MIDI.DeviceCount()`. Any MIDI devices within this range can be used as the MIDI device.

Commands can be sent to MIDI using the `midi.Send()` command. The first parameter is the command, which is an integer with a maximum value of 255. Three parameters after this can be used (refer to the MIDI documentation for what these can be used for). These parameters are optional, so if a command doesn't use any parameters, these are automatically set to be 0.

## Example
Here is an example using MidiSharp to play note C# at octave 3 repeatedly at 120BPM:
```
MIDI midi = new MIDI();

int bpm = 120;      // Beats per minute

int command = 144;  // Note on command
int note = 1;       // Note C#
int octave = 3;     // Octave 3
int velocity = 127  // 100% velocity
int pitch = (12 * (octave + 1)) + note;

while (true) {
  midi.Send(command, pitch, velocity);
  Thread.Sleep((60D / bpm) * 1000);
}
```

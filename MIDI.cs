using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MidiSharp
{
    public class MIDI
    {
        #region Interoperability (winmm.dll)
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string command,
        StringBuilder returnValue, int returnLength, IntPtr winHandle);

        [DllImport("winmm.dll")]
        private static extern int midiOutGetNumDevs();

        [DllImport("winmm.dll")]
        private static extern int midiOutGetDevCaps(Int32 uDeviceID, ref MidiOutCaps lpMidiOutCaps, UInt32 cbMidiOutCaps);

        [DllImport("winmm.dll")]
        private static extern int midiOutOpen(ref int handle, int deviceID, MidiCallBack proc, int instance, int flags);
        private delegate void MidiCallBack(int handle, int msg, int instance, int param1, int param2);

        [DllImport("winmm.dll")]
        protected static extern int midiOutShortMsg(int handle, int message);

        [DllImport("winmm.dll")]
        protected static extern int midiOutClose(int handle);
        #endregion

        private int device;
        private int handle;
        private MidiOutCaps details;

        public MIDI(int _device = 0)
        {
            if (_device > midiOutGetNumDevs())
                throw new Exception("MIDI device does not exist.");

            device = _device;
            midiOutOpen(ref handle, _device, null, 0, 0);

            details = new MidiOutCaps();

            if (midiOutGetDevCaps(0, ref details, (UInt32)Marshal.SizeOf(details)) != 0)
                throw new Exception("Failed to initialise MIDI details struct.");
        }

        public void Send(int _cmd, int _par1 = 0, int _par2 = 0, int _par3 = 0)
        {
            int msg = ((byte)_par3 << 24) + ((byte)_par2 << 16) + ((byte)_par1 << 8) + (byte)_cmd;

            if (midiOutShortMsg(handle, msg) != 0)
                throw new Exception("Command failed to send to MIDI.");
        }

        public MidiOutCaps Details
        {
            get { return details; }
        }

        public int DeviceNumber()
        {
            return device;
        }

        public int Handle()
        {
            return handle;
        }

        public static string Command(string _cmd)
        {
            int returnLength = 256;
            StringBuilder reply = new StringBuilder(returnLength);
            mciSendString(_cmd, reply, returnLength, IntPtr.Zero);
            return reply.ToString();
        }

        public static int DeviceCount()
        {
            return midiOutGetNumDevs();
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MidiOutCaps
        {
            public UInt16 wMid;
            public UInt16 wPid;
            public UInt32 vDriverVersion;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String szPname;

            public UInt16 wTechnology;
            public UInt16 wVoices;
            public UInt16 wNotes;
            public UInt16 wChannelMask;
            public UInt32 dwSupport;
        }
    }
}

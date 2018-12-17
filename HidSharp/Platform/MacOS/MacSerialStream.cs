#region License
/* Copyright 2017 James F. Bellinger <http://www.zer7.com/software/hidsharp>

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

      http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing,
   software distributed under the License is distributed on an
   "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
   KIND, either express or implied.  See the License for the
   specific language governing permissions and limitations
   under the License. */
#endregion

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace HidSharp.Platform.MacOS
{
    sealed class MacSerialStream : SerialStream
    {
        object _lock = new object();
        NativeMethods.termios _oldSettings;
        NativeMethods.termios _newSettings;
        int _baudRate; bool _baudRateDirty = true;
        int _handle;

        internal MacSerialStream(MacSerialDevice device)
            : base(device)
        {
            string fileSystemName = device.GetFileSystemName();

            int ret;
            int handle = NativeMethods.retry(() => NativeMethods.open(fileSystemName, NativeMethods.oflag.RDWR | NativeMethods.oflag.NOCTTY | NativeMethods.oflag.NONBLOCK));
            if (handle < 0)
            {
                var error = (NativeMethods.error)Marshal.GetLastWin32Error();
                if (error == NativeMethods.error.EACCES)
                {
                    throw DeviceException.CreateUnauthorizedAccessException(device, "Not permitted to open serial device at " + fileSystemName + ".");
                }
                else
                {
                    throw DeviceException.CreateIOException(device, "Unable to open serial device (" + error.ToString() + ").");
                }
            }

            ret = NativeMethods.retry(() => NativeMethods.ioctl(handle, NativeMethods.TIOCEXCL));
            if (ret < 0)
            {
                NativeMethods.retry(() => NativeMethods.close(handle));
                throw new IOException("Unable to open serial device exclusively.");
            }

            /*
            ret = NativeMethods.retry(() => NativeMethods.fcntl(handle, NativeMethods.F_SETFL, 0));
            if (ret < 0)
            {
                NativeMethods.retry(() => NativeMethods.ioctl(handle, NativeMethods.TIOCNXCL));
                NativeMethods.retry(() => NativeMethods.close(handle));
                throw new IOException("Unable to remove blocking from port.");
            }
            */

            ret = NativeMethods.retry(() => NativeMethods.tcgetattr(handle, out _oldSettings));
            if (ret < 0)
            {
                NativeMethods.retry(() => NativeMethods.ioctl(handle, NativeMethods.TIOCNXCL));
                NativeMethods.retry(() => NativeMethods.close(handle));
                throw new IOException("Unable to get serial port settings.");
            }

            _newSettings = _oldSettings;
            NativeMethods.cfmakeraw(ref _newSettings);
            _handle = handle;
            InitSettings();
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                lock (_lock)
                {
                    int handle = Interlocked.Exchange(ref _handle, -1);
                    if (handle >= 0)
                    {
                        NativeMethods.retry(() => NativeMethods.tcsetattr(handle, NativeMethods.TCSANOW, ref _oldSettings));
                        NativeMethods.retry(() => NativeMethods.ioctl(handle, NativeMethods.TIOCNXCL));
                        NativeMethods.retry(() => NativeMethods.close(handle));
                    }
                }
            }
            catch
            {

            }

            base.Dispose(disposing);
        }

        public override void Flush()
        {
            int handle = _handle;
            if (handle >= 0)
            {
                NativeMethods.retry(() => NativeMethods.tcdrain(handle));
            }
        }
        
        // Make these timeouts not infinite, so that threads blocking on poll will exit.
        static int GetTimeout(int startTime, int rwTimeout)
        {
            int timeout;

            if (rwTimeout < 0)
            {
                timeout = 1000;
            }
            else
            {
                timeout = Math.Min(1000, (startTime + rwTimeout) - Environment.TickCount);
                if (timeout < 0)
                {
                    throw new TimeoutException("Read timed out.");
                }
            }

            return timeout;
        }

        // TODO: Has close() race condition.
        public unsafe override int Read(byte[] buffer, int offset, int count)
        {
            Throw.If.OutOfRange(buffer, offset, count);
            UpdateSettings();
            if (count == 0) { return 0; }

            fixed (byte* buffer0 = buffer)
            {
                int startTime = Environment.TickCount, readTimeout = ReadTimeout;

                while (true)
                {
                    int handle = _handle;
                    if (handle < 0) { throw new IOException("Closed."); }

                    var bufferPtr = (IntPtr)(buffer0 + offset);
                    int bytesToRead = count;

                    var fd = new NativeMethods.pollfd() { fd = handle, events = NativeMethods.pollev.IN };
                    int ret = NativeMethods.retry(() => NativeMethods.poll(ref fd, 1, GetTimeout(startTime, readTimeout)));
                    if (ret < 0) { throw new IOException("Read failed (poll)."); }
                    if (ret == 1)
                    {
                        if (fd.revents != NativeMethods.pollev.IN) { throw new IOException(string.Format("Closed during read ({0}).", fd.revents)); }

                        int readCount = checked((int)NativeMethods.retry(() => NativeMethods.read(handle, bufferPtr, (UIntPtr)bytesToRead)));
                        if (readCount <= 0 || readCount > bytesToRead) { throw new IOException("Read failed."); }
                        return readCount;
                    }
                }
            }
        }

        // TODO: Has close() race condition.
        public unsafe override void Write(byte[] buffer, int offset, int count)
        {
            Throw.If.OutOfRange(buffer, offset, count);
            UpdateSettings();
            if (count == 0) { return; }

            fixed (byte* buffer0 = buffer)
            {
                int startTime = Environment.TickCount, writeTimeout = WriteTimeout;

                for (int bytesWritten = 0; bytesWritten < count; )
                {
                    int handle = _handle;
                    if (handle < 0) { throw new IOException("Closed."); }

                    var bufferPtr = (IntPtr)(buffer0 + offset + bytesWritten);
                    int bytesToWrite = count - bytesWritten;

                    var fd = new NativeMethods.pollfd() { fd = handle, events = NativeMethods.pollev.OUT };
                    int ret = NativeMethods.retry(() => NativeMethods.poll(ref fd, 1, GetTimeout(startTime, writeTimeout)));
                    if (ret < 0) { throw new IOException("Write failed (poll)."); }
                    if (ret == 1)
                    {
                        if (fd.revents != NativeMethods.pollev.OUT) { throw new IOException(string.Format("Closed during write ({0}).", fd.revents)); }

                        int writeCount = checked((int)NativeMethods.retry(() => NativeMethods.write(handle, bufferPtr, (UIntPtr)bytesToWrite)));
                        if (writeCount <= 0 || writeCount > bytesToWrite) { throw new IOException("Write failed."); }
                        bytesWritten += writeCount;
                    }
                }
            }
        }

        unsafe void InitSettings()
        {
            long iflag = (long)_newSettings.c_iflag;
            iflag = 0; // TODO: Be more specific. I don't want anything listed in the header files, though.
            _newSettings.c_iflag = (UIntPtr)iflag;

            long cflag = (long)_newSettings.c_cflag;
            cflag &= ~NativeMethods.CSTOPB;
            cflag &= ~NativeMethods.CSIZE;
            cflag &= ~NativeMethods.PARENB;
            //cflag |= NativeMethods.HUPCL; ?
            cflag |= NativeMethods.CS8;
            cflag |= NativeMethods.CREAD;
            cflag |= NativeMethods.CLOCAL;
            cflag &= ~NativeMethods.CRTSCTS;
            _newSettings.c_cflag = (UIntPtr)cflag;

            long oflag = (long)_newSettings.c_oflag;
            oflag &= ~NativeMethods.OPOST;
            _newSettings.c_oflag = (UIntPtr)oflag;

            fixed (byte* cc = _newSettings.c_cc) { cc[NativeMethods.VMIN] = 1; cc[NativeMethods.VTIME] = 0; }
        }

        unsafe void UpdateSettings()
        {
            lock (_lock)
            {
                int ret;
                int handle = _handle;
                if (handle >= 0)
                {
                    if (_baudRateDirty)
                    {
                        int baudRate = _baudRate;

                        ret = NativeMethods.retry(() => NativeMethods.cfsetspeed(ref _newSettings, (UIntPtr)Math.Max(1, baudRate)));
                        if (ret < 0) { throw new IOException("cfsetspeed failed."); }

                        ret = NativeMethods.retry(() => NativeMethods.tcsetattr(handle, NativeMethods.TCSANOW, ref _newSettings));
                        if (ret < 0) { throw new IOException("tcsetattr failed."); }

                        ret = NativeMethods.retry(() => NativeMethods.tcflush(handle, NativeMethods.TCIFLUSH));
                        if (ret < 0) { throw new IOException("tcflush failed."); }
                    }

                    _baudRateDirty = false;
                }
            }
        }

        public sealed override int BaudRate
        {
            get { return _baudRate; }
            set { lock (_lock) { _baudRate = value; _baudRateDirty = true; } }
        }

        public sealed override int ReadTimeout
        {
            get;
            set;
        }

        public sealed override int WriteTimeout
        {
            get;
            set;
        }
    }
}

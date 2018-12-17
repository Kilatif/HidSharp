﻿#region License
/* Copyright 2018 James F. Bellinger <http://www.zer7.com/software/hidsharp>

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

namespace HidSharp.Reports
{
    /// <summary>
    /// See http://www.usb.org/developers/hidpage for additional HID Usage Tables.
    /// </summary>
    public enum Usage : uint
    {
        GenericDesktopPointer = 0x00010001,
        GenericDesktopMouse = 0x00010002,
        GenericDesktopJoystick = 0x00010004,
        GenericDesktopGamepad = 0x00010005,
        GenericDesktopKeyboard = 0x00010006,
        GenericDesktopKeypad = 0x00010007,
        GenericDesktopMultiaxisController = 0x00010008,
        GenericDesktopX = 0x00010030,
        GenericDesktopY = 0x00010031,
        GenericDesktopZ = 0x00010032,
        GenericDesktopRx = 0x00010033,
        GenericDesktopRy = 0x00010034,
        GenericDesktopRz = 0x00010035,
        GenericDesktopSlider = 0x00010036,
        GenericDesktopDial = 0x00010037,
        GenericDesktopWheel = 0x00010038,
        GenericDesktopHatSwitch = 0x00010039,
        GenericDesktopStart = 0x0001003d,
        GenericDesktopSelect = 0x0001003e,
        GenericDesktopSystemControl = 0x00010080,
        GenericDesktopDPadUp = 0x00010090,
        GenericDesktopDPadDown = 0x00010091,
        GenericDesktopDPadLeft = 0x00010092,
        GenericDesktopDPadRight = 0x00010093,
        KeyboardA = 0x00070004,
        KeyboardB = 0x00070005,
        KeyboardC = 0x00070006,
        KeyboardD = 0x00070007,
        KeyboardE = 0x00070008,
        KeyboardF = 0x00070009,
        KeyboardG = 0x0007000a,
        KeyboardH = 0x0007000b,
        KeyboardI = 0x0007000c,
        KeyboardJ = 0x0007000d,
        KeyboardK = 0x0007000e,
        KeyboardL = 0x0007000f,
        KeyboardM = 0x00070010,
        KeyboardN = 0x00070011,
        KeyboardO = 0x00070012,
        KeyboardP = 0x00070013,
        KeyboardQ = 0x00070014,
        KeyboardR = 0x00070015,
        KeyboardS = 0x00070016,
        KeyboardT = 0x00070017,
        KeyboardU = 0x00070018,
        KeyboardV = 0x00070019,
        KeyboardW = 0x0007001a,
        KeyboardX = 0x0007001b,
        KeyboardY = 0x0007001c,
        KeyboardZ = 0x0007001d,
        Keyboard1 = 0x0007001e,
        Keyboard2 = 0x0007001f,
        Keyboard3 = 0x00070020,
        Keyboard4 = 0x00070021,
        Keyboard5 = 0x00070022,
        Keyboard6 = 0x00070023,
        Keyboard7 = 0x00070024,
        Keyboard8 = 0x00070025,
        Keyboard9 = 0x00070026,
        Keyboard0 = 0x00070027,
        KeyboardEnter = 0x00070028,
        KeyboardEscape = 0x00070029,
        KeyboardBackspace = 0x0007002a,
        KeyboardTab = 0x0007002b,
        KeyboardSpace = 0x0007002c,
        KeyboardF1 = 0x0007003a,
        KeyboardF2 = 0x0007003b,
        KeyboardF3 = 0x0007003c,
        KeyboardF4 = 0x0007003d,
        KeyboardF5 = 0x0007003e,
        KeyboardF6 = 0x0007003f,
        KeyboardF7 = 0x00070040,
        KeyboardF8 = 0x00070041,
        KeyboardF9 = 0x00070042,
        KeyboardF10 = 0x00070043,
        KeyboardF11 = 0x00070044,
        KeyboardF12 = 0x00070045,
        KeyboardDelete = 0x0007004c,
        KeypadEnter = 0x00070058,
        Keypad1 = 0x00070059,
        Keypad2 = 0x0007005a,
        Keypad3 = 0x0007005b,
        Keypad4 = 0x0007005c,
        Keypad5 = 0x0007005d,
        Keypad6 = 0x0007005e,
        Keypad7 = 0x0007005f,
        Keypad8 = 0x00070060,
        Keypad9 = 0x00070061,
        Keypad0 = 0x00070062,
        KeyboardLeftControl = 0x000700e0,
        KeyboardLeftShift = 0x000700e1,
        KeyboardLeftAlt = 0x000700e2,
        KeyboardLeftGUI = 0x000700e3,
        KeyboardRightControl = 0x000700e4,
        KeyboardRightShift = 0x000700e5,
        KeyboardRightAlt = 0x000700e6,
        KeyboardRightGUI = 0x000700e7,
        LedNumLock = 0x00080001,
        LedCapsLock = 0x00080002,
        LedScrollLock = 0x00080003,
        LedCompose = 0x00080004,
        LedKana = 0x00080005,
        Button1 = 0x00090001,
        Button2 = 0x00090002,
        Button3 = 0x00090003,
        Button4 = 0x00090004,
        Button5 = 0x00090005,
        Button6 = 0x00090006,
        Button7 = 0x00090007,
        Button8 = 0x00090008,
        Button9 = 0x00090009,
        Button10 = 0x0009000a,
        Button11 = 0x0009000b,
        Button12 = 0x0009000c,
        Button13 = 0x0009000d,
        Button14 = 0x0009000e,
        Button15 = 0x0009000f,
        Button16 = 0x00090010,
        Button17 = 0x00090011,
        Button18 = 0x00090012,
        Button19 = 0x00090013,
        Button20 = 0x00090014,
        Button21 = 0x00090015,
        Button22 = 0x00090016,
        Button23 = 0x00090017,
        Button24 = 0x00090018,
        Button25 = 0x00090019,
        Button26 = 0x0009001a,
        Button27 = 0x0009001b,
        Button28 = 0x0009001c,
        Button29 = 0x0009001d,
        Button30 = 0x0009001e,
        Button31 = 0x0009001f,
        ConsumerConsumerControl = 0x000c0001,
        ConsumerNumericKeypad = 0x000c0002,
        ConsumerProgrammableButtons = 0x000c0003,
        ConsumerMicrophone = 0x000c0004,
        ConsumerFunctionButtons = 0x000c0036,
        ConsumerScanNextTrack = 0x000c00b5,
        ConsumerScanPreviousTrack = 0x000c00b6,
        ConsumerPlayPause = 0x000c00cd,
        ConsumerMute = 0x000c00e2,
        ConsumerVolumeIncrement = 0x000c00e9,
        ConsumerVolumeDecrement = 0x000c00ea,
        ConsumerALCalculator = 0x000c0192,
        WeighingDevicesWeighingDevice = 0x008d0001,
        WeighingDevicesDataWeight = 0x008d0040,
        WeighingDevicesDataScaling = 0x008d0041,
        WeighingDevicesWeightUnit = 0x008d0050,
        WeighingDevicesWeightUnitMilligram = 0x008d0051,
        WeighingDevicesWeightUnitGram = 0x008d0052,
        WeighingDevicesWeightUnitKilogram = 0x008d0053,
        WeighingDevicesWeightUnitOunce = 0x008d005b,
        WeighingDevicesWeightUnitPound = 0x008d005c,
        WeighingDevicesScaleStatus = 0x008d0070,
        WeighingDevicesScaleStatusFault = 0x008d0071,
        WeighingDevicesScaleStatusStableAtCenterOfZero = 0x008d0072,
        WeighingDevicesScaleStatusInMotion = 0x008d0073,
        WeighingDevicesScaleStatusWeightStable = 0x008d0074,
        WeighingDevicesScaleStatusUnderZero = 0x008d0075,
        WeighingDevicesScaleStatusOverWeightLimit = 0x008d0076,
        WeighingDevicesScaleStatusRequiresCalibration = 0x008d0077,
        WeighingDevicesScaleStatusRequiresRezeroing = 0x008d0078,
        WeighingDevicesZeroScale = 0x008d0080
    }
}

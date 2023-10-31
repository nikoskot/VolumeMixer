// See https://aka.ms/new-console-template for more information
using System;
using System.Diagnostics;
using CSCore.CoreAudioAPI;
using System.IO.Ports;
using Notifications2;
using System.Collections;

namespace Program;

class Program {

    public static ArrayList arr = new ArrayList();

    public static void Main() {

        Console.WriteLine("Hello, World!");

        Notifications2.Notifications2 notifier = new Notifications2.Notifications2();

        notifier.SetupMediaSessionCallbacks();

        MMDeviceEnumerator enumerator = new MMDeviceEnumerator();

        MMDevice defaultDev = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
        Console.WriteLine(defaultDev.ToString());

        foreach (MMDevice dev in enumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active)) {

            Console.WriteLine(dev.ToString());
        }

        while(true) {

            // Console.WriteLine(arr.Count);
        }
    }
}

// Comunication with Arduino
// SerialPort _serialPort;
// _serialPort = new SerialPort();
// _serialPort.PortName = "COM4";//Set your board COM
// _serialPort.BaudRate = 9600;
// _serialPort.Open();
// while (true)
// {
//     string a = _serialPort.ReadExisting();
//     Console.WriteLine(a);
//     Thread.Sleep(200);
// }

// String[] processesToKeep = ["Discord.exe", "steam.exe", "firefox.exe"];
// System.Diagnostics.Process[] p =  Process.GetProcesses();

// //Change the volume of each application. These applications are displayed in the windows volume mixer
// using (var sessionManager = GetDefaultAudioSessionManager2(CSCore.CoreAudioAPI.DataFlow.Render))
// using (var sessionEnumerator = sessionManager.GetSessionEnumerator())
// {
//     foreach (var session in sessionEnumerator)
//     {
//         var session2 = session.QueryInterface<AudioSessionControl2>();
//         var volume = session.QueryInterface<CSCore.CoreAudioAPI.SimpleAudioVolume>();
        
//         //each session will be assigned to any process
//         //implement your logic to select preferred
//         // Console.WriteLine(session.DisplayName);
//         // Console.WriteLine(session2.SessionInstanceIdentifier);
//         Process proc = session2.Process;
//         String executable = "";
//         try {
//             executable = proc.MainModule.ModuleName;
//         }
//         catch (Exception e) {
//             if (proc.ProcessName == "Idle") {
//                 // Systems sounds (this we will ignore)
//                 executable = "system.exe";
//             }
//         }
//         Console.WriteLine(proc.ProcessName);
//         Console.WriteLine(proc.Id);
//         Console.WriteLine(volume.MasterVolume);
//         Console.WriteLine(executable);
//         // if (proc.ProcessName == "steam") {
//         //     Console.WriteLine(proc.MainModule.ModuleName);
//         // }
//         continue;
//     }
// }

// static AudioSessionManager2 GetDefaultAudioSessionManager2(CSCore.CoreAudioAPI.DataFlow dataFlow)
//     {
//         // Change the audio device volume, set it to 75%
//         CSCore.CoreAudioAPI.MMDeviceEnumerator enumerator = new CSCore.CoreAudioAPI.MMDeviceEnumerator();
//         CSCore.CoreAudioAPI.MMDevice device = enumerator.GetDefaultAudioEndpoint(dataFlow, CSCore.CoreAudioAPI.Role.Multimedia);
//         nint devPtr = device.BasePtr;
//         CSCore.CoreAudioAPI.AudioEndpointVolume endpointVol = CSCore.CoreAudioAPI.AudioEndpointVolume.FromDevice(device);
//         float masterVolume = endpointVol.GetMasterVolumeLevelScalar();
//         endpointVol.MasterVolumeLevelScalar = (float) 0.75;
        
//         var sessionManager = AudioSessionManager2.FromMMDevice(device);
//         return sessionManager; 
//     }



// void SetupMediaSessionCallbacks()
//     {
//         // Foreach output endpoint
//         foreach (var md in new NAudio.CoreAudioApi.MMDeviceEnumerator().EnumerateAudioEndPoints(NAudio.CoreAudioApi.DataFlow.Render, NAudio.CoreAudioApi.DeviceState.Active))
//         {
//             md.AudioSessionManager.OnSessionCreated += OnSessionCreated;
//         }
//     }

// void OnSessionCreated(object sender, NAudio.CoreAudioApi.Interfaces.IAudioSessionControl newSession) {

//     NAudio.CoreAudioApi.AudioSessionControl audioSession = new NAudio.CoreAudioApi.AudioSessionControl(newSession);
//     NAudioEventCallbacks callbacks = new NAudioEventCallbacks();
//     AudioSessionEventsCallback notifications = new AudioSessionEventsCallback(callbacks);
//     audioSession.RegisterEventClient(callbacks);

//     Console.WriteLine("New Session Created");

// }

// public class NAudioEventCallbacks : NAudio.CoreAudioApi.Interfaces.IAudioSessionEventsHandler
// {
//     public void OnChannelVolumeChanged(uint channelCount, IntPtr newVolumes, uint channelIndex) { }

//     public void OnDisplayNameChanged(string displayName) { }

//     public void OnGroupingParamChanged(ref Guid groupingId) { }

//     public void OnIconPathChanged(string iconPath) { }

//     public void OnSessionDisconnected(NAudio.CoreAudioApi.Interfaces.AudioSessionDisconnectReason disconnectReason) { }

//     public void OnStateChanged(NAudio.CoreAudioApi.Interfaces.AudioSessionState state) { }

//     public void OnVolumeChanged(float volume, bool isMuted) { }
// }


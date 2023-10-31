using CSCore.CoreAudioAPI;
using System.Collections;
using Program;

namespace Notifications2;
class Notifications2 {

    public void SetupMediaSessionCallbacks()
    {
        // Foreach output endpoint
        // foreach (var md in new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active))
        // {
        //     md.AudioSessionManager.OnSessionCreated += OnSessionCreated;
        // }

        MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
        MMDevice device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

        AudioSessionManager2 sessionManager2 = AudioSessionManager2.FromMMDevice(device);

        AudioSessionNotifications notif = new AudioSessionNotifications();

        Thread t = new Thread(new ThreadStart(() =>
        {
            Thread.CurrentThread.IsBackground = true;
            /* run your code here */
            Console.WriteLine("Thread running");
            sessionManager2.RegisterSessionNotification(notif);

            AudioSessionEnumerator sessionEnumerator = sessionManager2.GetSessionEnumerator();
        }));
        t.Start();
        t.Join();

        // sessionManager2.RegisterSessionNotification(notif);
    }

    public Int32  OnSessionCreated(IntPtr newSession) {
            
            Console.WriteLine("New session created");

            return 0;
        }

    // public class NAudioEventCallbacks : IAudioSessionEventsHandler
    // {
    //     public void OnChannelVolumeChanged(uint channelCount, IntPtr newVolumes, uint channelIndex) { }

    //     public void OnDisplayNameChanged(string displayName) { }

    //     public void OnGroupingParamChanged(ref Guid groupingId) { }

    //     public void OnIconPathChanged(string iconPath) { }

    //     public void OnSessionDisconnected(AudioSessionDisconnectReason disconnectReason) { }

    //     public void OnStateChanged(AudioSessionState state) { }

    //     public void OnVolumeChanged(float volume, bool isMuted) { }
    // }

    public class AudioSessionNotifications : IAudioSessionNotification {

        public Int32 OnSessionCreated(IntPtr newSession) {
            
            Console.WriteLine("New session created");

            return 0;
        }
    }
}
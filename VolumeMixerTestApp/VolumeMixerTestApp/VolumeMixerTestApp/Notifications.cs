using CSCore.CoreAudioAPI;
using System;
using System.Diagnostics;
using System.Threading;

namespace VolumeMixerTestApp
{
    internal class Notifications {
        // This class handles the notifications for the creation and termination of audio sessions

        public void SetupAudioSessionNotificationCallbacks()
        {
            // This method sets up the notification system.

            // Get the default audio endpoint device
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            MMDevice device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

            // Create an audio session notification handler object
            AudioSessionNotifications notif = new AudioSessionNotifications();

            // Register the notification handler in a MTA background thread
            Thread t = new Thread(new ThreadStart(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                AudioSessionManager2 sessionManager2 = AudioSessionManager2.FromMMDevice(device);
                sessionManager2.RegisterSessionNotification(notif);
                AudioSessionEnumerator sessionEnumerator = sessionManager2.GetSessionEnumerator();
            }));
            t.Start();
            t.Join();
        }

        public class AudioSessionNotifications : IAudioSessionNotification
        {
            // This class corresponds to the audio session notifications handler

            public Int32 OnSessionCreated(IntPtr newSession)
            {
                // This method is called when a new audio session is created

                Console.WriteLine("New session created");

                // Get the available audio sessions and audio session properties and save them to the corresponding list of the main application
                VolumeMixerTestApp.availableAudioSessionsProperties = VolumeMixerTestApp.CollectAvailableAudioSessionsProperties();
                return 0;
            }
        }
    }
}

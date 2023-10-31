using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;
using System.Collections;
using Program;

namespace Notifications;
class Notifications {

    public void SetupMediaSessionCallbacks()
    {
        // Foreach output endpoint
        foreach (var md in new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active))
        {
            md.AudioSessionManager.OnSessionCreated += OnSessionCreated;
        }
    }

    void OnSessionCreated(object sender, IAudioSessionControl newSession) {

        AudioSessionControl audioSession = new AudioSessionControl(newSession);
        NAudioEventCallbacks callbacks = new NAudioEventCallbacks();
        AudioSessionEventsCallback notifications = new AudioSessionEventsCallback(callbacks);
        audioSession.RegisterEventClient(callbacks);

        Console.WriteLine("New Session Created");
        Program.Program.arr.Add(1);

    }

    public class NAudioEventCallbacks : IAudioSessionEventsHandler
    {
        public void OnChannelVolumeChanged(uint channelCount, IntPtr newVolumes, uint channelIndex) { }

        public void OnDisplayNameChanged(string displayName) { }

        public void OnGroupingParamChanged(ref Guid groupingId) { }

        public void OnIconPathChanged(string iconPath) { }

        public void OnSessionDisconnected(AudioSessionDisconnectReason disconnectReason) { }

        public void OnStateChanged(AudioSessionState state) { }

        public void OnVolumeChanged(float volume, bool isMuted) { }
    }
}
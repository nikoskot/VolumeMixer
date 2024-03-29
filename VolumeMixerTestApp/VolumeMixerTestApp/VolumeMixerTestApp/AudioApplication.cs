﻿using CSCore.CoreAudioAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace VolumeMixerTestApp
{
    /// <summary>
    /// This class implements an application that uses audio in the system.
    /// </summary>
    internal class AudioApplication {

        AudioSessionControl session;
        AudioSessionControl2 session2;
        SimpleAudioVolume volume;
        AudioEndpointVolume masterVolume;
        Process process;
        String processName;
        AudioChannel channel;

        // Getters.
        public AudioSessionControl getSession() { return session; }

        public AudioSessionControl2 getSession2() { return session2; }

        public SimpleAudioVolume getVolume() { return volume; }

        public AudioEndpointVolume getMasterVolume() { return masterVolume; }

        public float getVolumeValue() {

            float volume = 0;

            if (this.volume != null && this.masterVolume == null) { volume = this.volume.MasterVolume; }
            else if (this.volume == null && this.masterVolume != null) { volume = masterVolume.MasterVolumeLevelScalar; }

            return volume;
        }

        public Process getProcess() { return process; }

        public String getProcessName() { return processName; }

        public AudioChannel getChannel() { return channel; }

        // Setters.
        public void setSession(AudioSessionControl session) {

            if (session != null) {

                this.session = session;
            }
        }

        public void setSession2(AudioSessionControl2 session2) {

            if (session2 != null) {

                this.session2 = session2;
            }
        }

        public void setVolume(SimpleAudioVolume volume)
        {

            if (volume != null)
            {

                this.volume = volume;
            }
        }

        public void setVolume(AudioEndpointVolume masterVolume)
        {

            if (masterVolume != null)
            {

                this.masterVolume = masterVolume;
            }
        }

        public void setVolume(float volume)
        {

            if (volume >= 0 && volume <= 1f)
            {

                if (this.volume != null && this.masterVolume == null) { this.volume.MasterVolume = volume; }
                else if (this.volume == null && this.masterVolume != null) { masterVolume.MasterVolumeLevelScalar = volume; }
            }
        }

        public void setProcess(Process process) {

            if (process != null) {

                this.process = process;
            }
        } 

        public void setProcessName(String processName) {

            if (processName != null) {

                this.processName = processName;
            }
        }

        public void setChannel(AudioChannel channel) {

            if (channel != null) {

                this.channel = channel;
            }
        }
        
        // Constructor
        public AudioApplication(AudioSessionControl session) {

            setSession(session);
            setSession2(session.QueryInterface<AudioSessionControl2>());
            setVolume(session.QueryInterface<SimpleAudioVolume>());
            setProcess(this.session2.Process);
            setProcessName(this.process.ProcessName);
        }

        // Empty constructor
        public AudioApplication() {

            this.session = null;
            this.session2 = null;
            this.process = null;
            this.processName = null;
            this.channel = null;
        }
    }
}

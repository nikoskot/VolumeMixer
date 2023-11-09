﻿using CSCore.CoreAudioAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        Process process;
        String executable;
        AudioChannel channel;

        // Getters.
        public AudioSessionControl getSession() { return session; }

        public AudioSessionControl2 getSession2() { return session2; }

        public Process getProcess() { return process; }

        public String getExecutable() { return executable; }

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

        public void setProcess(Process process) {

            if (process != null) {

                this.process = process;
            }
        } 

        public void setExecutable(String executable) {

            if (executable != null) {

                this.executable = executable;
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
            setProcess(this.session2.Process);

            try {

                // Set up the handler for when the process will be terminated
                this.process.EnableRaisingEvents = true;
                this.process.Exited += (sender, e) => {
                    Console.WriteLine("Session Disconnected");
                    VolumeMixerTestApp.availableAudioSessionsProperties = VolumeMixerTestApp.CollectAvailableAudioSessionsProperties();
                };
            }

            catch (Exception ex) { }

            try {

                setExecutable(this.process.MainModule.ModuleName);
            }

            catch (Exception ex) {

                if (this.process.ProcessName == "Idle") {

                    // Systems sounds (this we will ignore)
                    setExecutable("system.exe");
                }
            }
        }

        // Empty constructor
        public AudioApplication() {

            this.session = null;
            this.session2 = null;
            this.process = null;
            this.executable = null;
            this.channel = null;
        }
    }
}

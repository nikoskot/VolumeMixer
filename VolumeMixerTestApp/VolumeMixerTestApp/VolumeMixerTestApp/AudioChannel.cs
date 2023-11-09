using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolumeMixerTestApp
{
    /// <summary>
    /// This class implements an audio channel object.
    /// </summary>
    internal class AudioChannel
    {
        // The audio application that the channels is connected to.
        AudioApplication audioApplication;

        // The volume of the channel.
        float volume;


        // Getters.
        public AudioApplication getAudioApplication() { return this.audioApplication; }

        public float getVolume() { return this.volume; }

        // Setters.
        public void setAudioApplication(AudioApplication audioApplication) {

            if (audioApplication != null) {

                this.audioApplication = audioApplication;
            }
        }

        public void setVolume(float volume) {

            if (volume >= 0 && volume <= 1.0f) { 
                
                this.volume = volume;
            }
        }

        // Constructor.
        public AudioChannel(AudioApplication audioApplication, float volume) {

            setAudioApplication(audioApplication);
            setVolume(volume);
        }

        // Empty Constructor.
        public AudioChannel() {

            setAudioApplication(new AudioApplication());
            setVolume(0f);
        }
    }
}

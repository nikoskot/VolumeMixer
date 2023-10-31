using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using CSCore.CoreAudioAPI;
using System.Collections;
using System.Threading;
using System.IO.Ports;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics.Tracing;

namespace VolumeMixerTestApp
{

    public partial class VolumeMixerTestApp : Form
    {
        // The path of the current directory
        String CURRENT_FOLDER_PATH = Path.GetDirectoryName(Application.ExecutablePath);
        // The name of the json file where the configuration is saved when closing the app
        String SAVED_CONFIG_FILE_NAME = "saved_config.json";
        // The labels of the channels
        String[] CHANNELS = { "CH1", "CH2", "CH3", "CH4" };
        String[] CHANNEL_IDS = { "1", "2", "3", "4" };

        static SerialPort arduinoPort;

        // The available audio sessions, nore precisely their audio session properties objects
        static public ArrayList availableAudioSessionsProperties = new ArrayList();

        // The audio sessions that are currently controlled
        static ArrayList activatedAudioSessions = new ArrayList();

        static String[] channelsToAudioSessionsMappings = {null, null, null, null};

        public VolumeMixerTestApp()
        {
            InitializeComponent();

            Console.WriteLine("Initialized");

            this.FormClosing += new FormClosingEventHandler(VolumeMixerTestApp_FormClosing);
        }

        /// <summary>
        /// This method is called when the application window is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VolumeMixerTestApp_Load(object sender, EventArgs e)
        {
            Console.WriteLine("Loaded");

            // Arduino setup
            // Set up the SerialPort for the Arduino
            arduinoPort = new SerialPort("COM4", 9600); // Use the correct COM port and baud rate
            // Subscribe to DataReceived event to listen to Arduino input
            arduinoPort.DataReceived += ArduinoDataReceived;
            // Open the SerialPort
            arduinoPort.Open();
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();

            Notifications notifier = new Notifications();

            notifier.SetupAudioSessionNotificationCallbacks();

            // Get available audio sessions

            // Upon initialization get the available audio sessions (properties)
            Thread t = new Thread(new ThreadStart(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                Console.WriteLine("Thread running");
                availableAudioSessionsProperties = CollectAvailableAudioSessionsProperties();
            }));
            t.Start();
            t.Join();
            Console.WriteLine("Thread finished");


            // Load saved configuration from file
            //Dictionary<String, ChannelProperties> loaded_config = loadSavedConfig(CURRENT_FOLDER_PATH + SAVED_CONFIG_FILE_NAME);
            //// If loaded configuration is not empty, activate the configuration
            //if (loaded_config.Count >= 1) {

            //    // Get the executables of the available audio sessions
            //    ArrayList availableAudioSessionsExecutables = new ArrayList();
            //    foreach (AudioSessionControl session in availableAudioSessions) {

            //        AudioSessionControl2 session2 = session.QueryInterface<AudioSessionControl2>();
            //        Process proc = session2.Process;
            //        String executable = proc.MainModule.ModuleName;

            //        availableAudioSessionsExecutables.Add(executable);
            //    }

            //    foreach (String channel in CHANNELS) {

            //        String executable = loaded_config[channel].executable;
            //        float volume = loaded_config[channel].volume;

            //        ///////

            //    }
            //}
        }

        /// <summary>
        /// This method is called when the application window is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VolumeMixerTestApp_FormClosing(object sender, FormClosingEventArgs e)
        {

            Console.WriteLine("Application closed");
        }

        private void channel1DropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            channelsToAudioSessionsMappings[0] = channel1DropDown.SelectedItem.ToString();
        }

        private void channel1DropDown_DropDown(object sender, EventArgs e)
        {
            // Clear the current item list
            channel1DropDown.Items.Clear();

            // Add the available audio sessions executables to the dropdown lists
            ArrayList availableAudioSessionsExecutables = new ArrayList();

            availableAudioSessionsExecutables.Add("Master Volume");

            // For each audio session property entity available
            foreach (AudioSessionProperties audioSessionProp in availableAudioSessionsProperties)
            {

                // Get only the executable and save it to the list
                availableAudioSessionsExecutables.Add(audioSessionProp.executable);
            }
            
            // Add the executables list to the drop down list
            channel1DropDown.Items.AddRange(availableAudioSessionsExecutables.ToArray());
        }

        private void channel2DropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            channelsToAudioSessionsMappings[1] = channel2DropDown.SelectedItem.ToString();
        }

        private void channel2DropDown_DropDown(object sender, EventArgs e)
        {
            // Clear the current item list
            channel2DropDown.Items.Clear();

            // Add the available audio sessions executables to the dropdown lists
            ArrayList availableAudioSessionsExecutables = new ArrayList();

            availableAudioSessionsExecutables.Add("Master Volume");

            // For each audio session property entity available
            foreach (AudioSessionProperties audioSessionProp in availableAudioSessionsProperties)
            {

                // Get only the executable and save it to the list
                availableAudioSessionsExecutables.Add(audioSessionProp.executable);
            }

            // Add the executables list to the drop down list
            channel2DropDown.Items.AddRange(availableAudioSessionsExecutables.ToArray());
        }

        private void channel3DropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            channelsToAudioSessionsMappings[2] = channel3DropDown.SelectedItem.ToString();
        }

        private void channel3DropDown_DropDown(object sender, EventArgs e)
        {
            // Clear the current item list
            channel3DropDown.Items.Clear();

            // Add the available audio sessions executables to the dropdown lists
            ArrayList availableAudioSessionsExecutables = new ArrayList();

            availableAudioSessionsExecutables.Add("Master Volume");

            // For each audio session property entity available
            foreach (AudioSessionProperties audioSessionProp in availableAudioSessionsProperties)
            {

                // Get only the executable and save it to the list
                availableAudioSessionsExecutables.Add(audioSessionProp.executable);
            }

            // Add the executables list to the drop down list
            channel3DropDown.Items.AddRange(availableAudioSessionsExecutables.ToArray());
        }

        private void channel4DropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            channelsToAudioSessionsMappings[3] = channel4DropDown.SelectedItem.ToString();
        }

        private void channel4DropDown_DropDown(object sender, EventArgs e)
        {
            // Clear the current item list
            channel4DropDown.Items.Clear();

            // Add the available audio sessions executables to the dropdown lists
            ArrayList availableAudioSessionsExecutables = new ArrayList();

            availableAudioSessionsExecutables.Add("Master Volume");

            // For each audio session property entity available
            foreach (AudioSessionProperties audioSessionProp in availableAudioSessionsProperties)
            {

                // Get only the executable and save it to the list
                availableAudioSessionsExecutables.Add(audioSessionProp.executable);
            }

            // Add the executables list to the drop down list
            channel4DropDown.Items.AddRange(availableAudioSessionsExecutables.ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Clicked apply");
        }

        static public ArrayList CollectAvailableAudioSessionsProperties() {

            // Get the device enumerator, the audio device and the session manager of the device
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            MMDevice device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            AudioSessionManager2 sessionManager = AudioSessionManager2.FromMMDevice(device);

            // Get the audio session enumerator
            AudioSessionEnumerator sessionEnumerator = sessionManager.GetSessionEnumerator();

            // Initialize the list of the audio sessions properties objects
            ArrayList audioSessionsProperties = new ArrayList();

            // For each audio session in the audio session enumerator
            foreach (AudioSessionControl session in sessionEnumerator) {

                // Create the corresponding audio session properties object
                AudioSessionProperties audioSessionProp = new AudioSessionProperties(session);

                // If it is not the system.exe and has a valid executable name
                if (audioSessionProp.executable != null & audioSessionProp.executable != "system.exe")
                {

                    // Add it to the list
                    audioSessionsProperties.Add(audioSessionProp);
                }    
            }

            return audioSessionsProperties;
        }

        static private AudioSessionProperties GetAudioSessionPropertyFromExecutable(string executable) {

            AudioSessionProperties prop = null;

            foreach (AudioSessionProperties audioSessionProperties in availableAudioSessionsProperties) {

                if (audioSessionProperties.executable == executable) {

                    prop = audioSessionProperties;
                }
            }

            return prop;
        }

        static void ArduinoDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Read data from the Arduino
            string arduinoData = arduinoPort.ReadLine();

            // Process Arduino input and call a specific function
            Console.WriteLine(arduinoData);

            String[] data = arduinoData.Split('_');

            int channelNumber = (int) Char.GetNumericValue(data[0].Last());

            float newVolume = float.Parse(data[1]);

            AudioSessionProperties audioSessionPropertiesToControll;

            //// Change the audio device volume, set it to 75%
            //MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            //MMDevice device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

            //AudioEndpointVolume endpointVol = AudioEndpointVolume.FromDevice(device);
            ////float masterVolume = endpointVol.GetMasterVolumeLevelScalar();
            //endpointVol.MasterVolumeLevelScalar = newVolume;

            String executableToControll = channelsToAudioSessionsMappings[channelNumber - 1];

            if (executableToControll == "Master Volume") {

                audioSessionPropertiesToControll = new AudioSessionProperties();

                audioSessionPropertiesToControll.executable = "Master Volume";

                MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
                MMDevice device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
                AudioEndpointVolume endpointVol = AudioEndpointVolume.FromDevice(device);

                endpointVol.MasterVolumeLevelScalar = newVolume;
            }
            else if (executableToControll != null) {

                audioSessionPropertiesToControll = GetAudioSessionPropertyFromExecutable(executableToControll);

                if (audioSessionPropertiesToControll != null)
                {
                    audioSessionPropertiesToControll.volume.MasterVolume = newVolume;
                }
            }
        }

        private Dictionary<String, ChannelProperties> loadSavedConfig(String filePath)
        {
            Dictionary<String, ChannelProperties> loaded_config = new Dictionary<string, ChannelProperties>();

            // Check if saved config file exists
            if (File.Exists(filePath))
            {
                // Read the JSON data from the file
                string json = File.ReadAllText(filePath);

                // Deserialize the data to a dictionary
                loaded_config = JsonConvert.DeserializeObject<Dictionary<String, ChannelProperties>>(json);
            }

            return loaded_config;
        }
    }

    public class AudioSessionProperties
    {

        public AudioSessionControl session { get; set; }
        public AudioSessionControl2 session2 { get; set; }
        public SimpleAudioVolume volume { get; set; }
        public Process process { get; set; }
        public String executable { get; set; }

        public AudioSessionProperties(AudioSessionControl session)
        {
            this.session = session;
            this.session2 = session.QueryInterface<AudioSessionControl2>();
            this.volume = session.QueryInterface<SimpleAudioVolume>();
            this.process = this.session2.Process;
            
            try
            {
                // Set up the handler for when the process will be terminated
                this.process.EnableRaisingEvents = true;
                this.process.Exited += (sender, e) => { Console.WriteLine("Session Disconnected");
                                                        VolumeMixerTestApp.availableAudioSessionsProperties = VolumeMixerTestApp.CollectAvailableAudioSessionsProperties();
                                                      };
            }
            catch (Exception ex)
            {

            }

            try
            {
                this.executable = this.process.MainModule.ModuleName;
            }

            catch (Exception ex)
            {
                if (this.process.ProcessName == "Idle")
                {
                    // Systems sounds (this we will ignore)
                    executable = "system.exe";
                }
            }
        }

        public AudioSessionProperties()
        {
            this.session = null;
            this.session2 = null;
            this.volume = null;
            this.process = null;
            this.executable = null;
        }
    }

    public class ChannelProperties
    {

        public string executable { get; set; }
        public float volume { get; set; }
    }
}

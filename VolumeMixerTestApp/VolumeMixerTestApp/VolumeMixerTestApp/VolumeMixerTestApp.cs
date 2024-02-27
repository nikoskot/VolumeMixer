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
using System.Diagnostics.Tracing;
using System.Text.Json;
using static System.Collections.Specialized.BitVector32;

namespace VolumeMixerTestApp
{

    public partial class VolumeMixerTestApp : Form
    {
        // The path of the current directory
        String CURRENT_FOLDER_PATH = Path.GetDirectoryName(Application.ExecutablePath);

        // The name of the json files where the configuration is saved when closing the app
        String SAVED_APPS_FILE_NAME = "saved_apps.json";
        String SAVED_VOLUMES_FILE_NAME = "saved_volumes.json";

        // The labels of the channels
        String[] CHANNELS = { "CH1", "CH2", "CH3", "CH4" };
        const int CHANNELS_NUM = 4;

        static SerialPort arduinoPort;

        // This list holds the currently available running applications/sessions that use audio.
        static List<AudioApplication> availableAudioApplications = new List<AudioApplication>();

        // This array holds the available audio channels.
        static AudioChannel[] audioChannels = new AudioChannel[CHANNELS_NUM];

        // This array holds the objects of the UI Comboboxes
        static ComboBox[] channelDropDownComboBoxes = new ComboBox[CHANNELS_NUM];

        public VolumeMixerTestApp()
        {
            InitializeComponent();

            Console.WriteLine("Initialized");

            // Set a callback for when the user closes the application
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

            //////////////////////////////////////////////////////////////////////////////////////////
            // Arduino setup
            // Set up the SerialPort for the Arduino
            arduinoPort = new SerialPort("COM4", 9600); // Use the correct COM port and baud rate

            // Subscribe to DataReceived event to listen to Arduino input
            arduinoPort.DataReceived += ArduinoDataReceived;

            // Open the SerialPort
            arduinoPort.Open();
            //////////////////////////////////////////////////////////////////////////////////////////

            // Initialize the array that holds the audio channels.
            for (int i = 0; i < CHANNELS_NUM; i++) {
                audioChannels[i] = new AudioChannel();
            }

            // Initialize the array that holds the UI Comboboxes
            channelDropDownComboBoxes[0] = channel1DropDown;
            channelDropDownComboBoxes[1] = channel2DropDown;
            channelDropDownComboBoxes[2] = channel3DropDown;
            channelDropDownComboBoxes[3] = channel4DropDown;

            /////////////////////////////////////////////////////////////////////////////////////////
            // Setup notifications-callbacks for when a new audio session is created.
            //Notifications notifier = new Notifications();
            //notifier.SetupAudioSessionNotificationCallbacks();
            /////////////////////////////////////////////////////////////////////////////////////////

            // Upon initialization get the available audio applications/sessions
            GetAvailableAudioApplications();

            /////////////////////////////////////////////////////////////////////////////////////////
            // Load saved configuration from file
            //String read = File.ReadAllText(CURRENT_FOLDER_PATH + SAVED_APPS_FILE_NAME);
            //Dictionary<String, String> channelToApp = JsonSerializer.Deserialize<Dictionary<String, String>>(read);

            //read = File.ReadAllText(CURRENT_FOLDER_PATH + SAVED_VOLUMES_FILE_NAME);
            //Dictionary<String, float> channelToVolume = JsonSerializer.Deserialize<Dictionary<String, float>>(read);

            //// If loaded configuration is not empty, activate the configuration
            //if (channelToApp.Count >= 1 && channelToVolume.Count >= 1 && channelToVolume.Count == channelToApp.Count)
            //{

            //    // Get the executables of the available audio sessions
            //    //ArrayList availableAudioSessionsExecutables = new ArrayList();
            //    //foreach (AudioSessionControl session in availableAudioSessions)
            //    //{

            //    //    AudioSessionControl2 session2 = session.QueryInterface<AudioSessionControl2>();
            //    //    Process proc = session2.Process;
            //    //    String executable = proc.MainModule.ModuleName;

            //    //    availableAudioSessionsExecutables.Add(executable);
            //    //}

            //    for (int i = 0; i < CHANNELS_NUM; i++) {

            //        String loadedExecutable = channelToApp[CHANNELS[i]];

            //        float loadedVolume = channelToVolume[CHANNELS[i]];

            //        foreach (AudioApplication audioApplication in availableAudioApplications) {

            //            if (audioApplication.getExecutable() == loadedExecutable) {

            //                // set the combobox to show that executable, attach the audio app to the channel, update the volume
            //                // 
            //                audioChannels[i].setAudioApplication(audioApplication);
            //                audioApplication.setVolume(loadedVolume);
            //            }
            //        }

            //    }
            //}
            /////////////////////////////////////////////////////////////////////////////////////////
        }

        /// <summary>
        /// This method is called when the application window is closed. It saves the channel-audioApp-volume configuration to two json files.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VolumeMixerTestApp_FormClosing(object sender, FormClosingEventArgs e)
        {

            Console.WriteLine("Application closed");

            //// Create two dictionaries that will be  saved in two json files
            //Dictionary<String, String> channelToApp = new Dictionary<String, String>();
            //Dictionary<String, float> channelToVolume = new Dictionary<String, float>();

            //// For each aduio channel
            //for (int i = 0; i < CHANNELS_NUM; i++) {

            //    if (audioChannels[i] != null) {

            //        // Add the channels-app-volume pairs to the dictionaries
            //        channelToApp.Add(CHANNELS[i], audioChannels[i].getAudioApplication().getExecutable());

            //        channelToVolume.Add(CHANNELS[i], audioChannels[i].getAudioApplication().getVolumeValue());
            //    }
            //}

            //// Save the channel-app dictionary
            //String json = JsonSerializer.Serialize(channelToApp);

            //File.WriteAllText(CURRENT_FOLDER_PATH + SAVED_APPS_FILE_NAME, json);

            //// Save the channel-volume dictionary
            //json = JsonSerializer.Serialize(channelToVolume);

            //File.WriteAllText(CURRENT_FOLDER_PATH + SAVED_VOLUMES_FILE_NAME, json);
        }

        //private void channel1DropDown_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    audioChannels[0].setAudioApplication(GetAudioApplicationFromExecutable(channel1DropDown.SelectedItem.ToString()));
        //}

        private void channel1DropDown_DropDown(object sender, EventArgs e)
        {
            // Clear the current item list
            channel1DropDown.Items.Clear();

            // Add the available audio application executables to the dropdown lists
            // Create an empty list.
            ArrayList availableAudioSessionsExecutables = new ArrayList();

            // For each audio application available
            foreach (AudioApplication audioApp in availableAudioApplications)
            {

                // Get only the executable and save it to the list
                availableAudioSessionsExecutables.Add(audioApp.getExecutable());
            }
            
            // Add the executables list to the drop down list
            channel1DropDown.Items.AddRange(availableAudioSessionsExecutables.ToArray());
        }

        //private void channel2DropDown_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    audioChannels[1].setAudioApplication(GetAudioApplicationFromExecutable(channel1DropDown.SelectedItem.ToString()));
        //}

        private void channel2DropDown_DropDown(object sender, EventArgs e)
        {
            // Clear the current item list
            channel2DropDown.Items.Clear();

            // Add the available audio application executables to the dropdown lists
            // Create an empty list.
            ArrayList availableAudioSessionsExecutables = new ArrayList();

            // For each audio application available
            foreach (AudioApplication audioApp in availableAudioApplications)
            {

                // Get only the executable and save it to the list
                availableAudioSessionsExecutables.Add(audioApp.getExecutable());
            }

            // Add the executables list to the drop down list
            channel2DropDown.Items.AddRange(availableAudioSessionsExecutables.ToArray());
        }

        //private void channel3DropDown_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    audioChannels[2].setAudioApplication(GetAudioApplicationFromExecutable(channel1DropDown.SelectedItem.ToString()));
        //}

        private void channel3DropDown_DropDown(object sender, EventArgs e)
        {
            // Clear the current item list
            channel3DropDown.Items.Clear();

            // Add the available audio application executables to the dropdown lists
            // Create an empty list.
            ArrayList availableAudioSessionsExecutables = new ArrayList();

            // For each audio application available
            foreach (AudioApplication audioApp in availableAudioApplications)
            {

                // Get only the executable and save it to the list
                availableAudioSessionsExecutables.Add(audioApp.getExecutable());
            }

            // Add the executables list to the drop down list
            channel3DropDown.Items.AddRange(availableAudioSessionsExecutables.ToArray());
        }

        //private void channel4DropDown_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    audioChannels[3].setAudioApplication(GetAudioApplicationFromExecutable(channel1DropDown.SelectedItem.ToString()));
        //}

        private void channel4DropDown_DropDown(object sender, EventArgs e)
        {
            // Clear the current item list
            channel4DropDown.Items.Clear();

            // Add the available audio application executables to the dropdown lists
            // Create an empty list.
            ArrayList availableAudioSessionsExecutables = new ArrayList();

            // For each audio application available
            foreach (AudioApplication audioApp in availableAudioApplications)
            {

                // Get only the executable and save it to the list
                availableAudioSessionsExecutables.Add(audioApp.getExecutable());
            }

            // Add the executables list to the drop down list
            channel4DropDown.Items.AddRange(availableAudioSessionsExecutables.ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Clicked apply");
            for (int i = 0; i < CHANNELS_NUM; i++) {

                if (channelDropDownComboBoxes[i].SelectedIndex >= 0)
                {

                    audioChannels[i].setAudioApplication(GetAudioApplicationFromExecutable(channelDropDownComboBoxes[i].SelectedItem.ToString()));
                }
                else
                {
                    audioChannels[i].setAudioApplication(new AudioApplication());
                }
            }
        }

        /// <summary>
        /// This method is called when we want to get all the available audio applications/sessions that are currently running is the system.
        /// </summary>
        static public void GetAvailableAudioApplications() {

            Thread t = new Thread(new ThreadStart(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                Console.WriteLine("Thread running");
                CollectAvailableAudioApplications();
            }));
            t.Start();
            t.Join();
            Console.WriteLine("Thread finished");
        }

        /// <summary>
        /// This method finds and collects all the available audio applications/sessions that are currently running is the system. 
        /// They are saved in the corresponding list.
        /// </summary>
        static public void CollectAvailableAudioApplications() {

            // Get the device enumerator, the audio device and the session manager of the device
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            MMDevice device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            AudioSessionManager2 sessionManager = AudioSessionManager2.FromMMDevice(device);

            // Get the audio session enumerator
            AudioSessionEnumerator sessionEnumerator = sessionManager.GetSessionEnumerator();

            // Clear the list with the currently available audio applications/sessions
            availableAudioApplications.Clear();

            // Add manualy the System Master Volume.
            // Create an Audio Apllication object for the System Master Volume
            AudioApplication masterVolumeApplication = new AudioApplication();
            masterVolumeApplication.setExecutable("Master Volume");
            masterVolumeApplication.setVolume(AudioEndpointVolume.FromDevice(device));

            // Add it to the list
            availableAudioApplications.Add(masterVolumeApplication);

            // For each audio session in the audio session enumerator
            foreach (AudioSessionControl session in sessionEnumerator) {

                // Create the corresponding audio application object
                AudioApplication audioApplication = new AudioApplication(session);

                // If it is not the system.exe and has a valid executable name
                if (audioApplication.getExecutable() != null & audioApplication.getExecutable() != "system.exe")
                {

                    // Add it to the list
                    availableAudioApplications.Add(audioApplication);
                }    
            }
        }

        static private AudioApplication GetAudioApplicationFromExecutable(string executable) {
            AudioApplication audioApp = null;

            foreach (AudioApplication audioApplication in availableAudioApplications)
            {

                if (audioApplication.getExecutable() == executable)
                {

                    audioApp = audioApplication;
                }
            }

            return audioApp;
        }

        /// <summary>
        /// This method is called when the app receives a new message from the mixer board.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void ArduinoDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Read data from the Arduino
            string arduinoData = arduinoPort.ReadLine();

            // Decompose the message and get the channel id and the new volume number
            Console.WriteLine(arduinoData);

            String[] data = arduinoData.Split('_');

            int channelNumber = (int) Char.GetNumericValue(data[0].Last());

            float newVolume = float.Parse(data[1]);

            // Change the channel volume
            audioChannels[channelNumber - 1].getAudioApplication().setVolume(newVolume);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GetAvailableAudioApplications();
        }

        //private Dictionary<String, ChannelProperties> loadSavedConfig(String filePath)
        //{
        //    Dictionary<String, ChannelProperties> loaded_config = new Dictionary<string, ChannelProperties>();

        //    // Check if saved config file exists
        //    if (File.Exists(filePath))
        //    {
        //        // Read the JSON data from the file
        //        string json = File.ReadAllText(filePath);

        //        // Deserialize the data to a dictionary
        //        loaded_config = JsonConvert.DeserializeObject<Dictionary<String, ChannelProperties>>(json);
        //    }

        //    return loaded_config;
        //}
    }
}

using Microsoft.Win32;
using NAudio.CoreAudioApi;
using System.Diagnostics;
using System.Xml.Linq;

namespace SoundSwitch
{
    public partial class FormSoundSwitch : Form
    {
        private MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
        private MMDeviceCollection devices;

        private bool isDragging = false;

        private VolumeController vc = new VolumeController();

        private Dictionary<string, string> data = new Dictionary<string, string>();

        private const string AutorunKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private static string AppName => Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName);


        int clientWidth = 0;
        int clientHeight = 0;
        int totalWidth = 0;
        int totalHeight = 0;
        int leftPos = 0;
        int topPos = 0;

        class AudioDeviceItem
        {
            public string Name { get; set; }
            public string Id { get; set; }
            public override string ToString() => Name;
        }

        // INIT
        public FormSoundSwitch()
        {
            InitializeComponent();
            LoadAudioDevices();

            instantProgressBar1.Value = vc.GetVolume();

            LoadData();

            if (data.ContainsKey("TopMost"))
            {
                this.TopMost = data["TopMost"] == "1";
            }

            if (data.ContainsKey("Left"))
            {
                this.Left = Int32.Parse(data["Left"]);
            }

            if (data.ContainsKey("Top"))
            {
                this.Top = Int32.Parse(data["Top"]);
            }


            // HIDE TITLE BAR
            clientWidth = this.ClientSize.Width;
            clientHeight = this.ClientSize.Height;
            totalWidth = this.Width;
            totalHeight = this.Height;
            leftPos = this.Left;
            topPos = this.Top;
        }



        // EVENT ACTIVATE FORM
        private void FormSoundSwitch_Activated(object sender, EventArgs e)
        {
            LoadAudioDevices();
            instantProgressBar1.Value = vc.GetVolume();

            if (this.FormBorderStyle == FormBorderStyle.None) {
                this.Left = leftPos;
                this.Top = topPos;
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                clientWidth = this.ClientSize.Width;
                clientHeight = this.ClientSize.Height;
                totalWidth = this.Width;
                totalHeight = this.Height;
            }
        }


        // EVENT DEACTIVATE FORM
        private void FormSoundSwitch_Deactivate(object sender, EventArgs e)
        {
            if (this.FormBorderStyle == FormBorderStyle.FixedSingle)
            {
                leftPos = this.Left;
                topPos = this.Top;
                this.FormBorderStyle = FormBorderStyle.None;
                this.Left += (totalWidth - clientWidth) / 2;
                int borderWidth = (totalWidth - clientWidth) / 2;
                this.Top += (totalHeight - clientHeight) - borderWidth;
            }
        }

        // EVENT FORM CLOSING
        private void FormSoundSwitch_FormClosing(object sender, FormClosingEventArgs e)
        {
            data["Left"] = this.Left.ToString();
            data["Top"] = this.Top.ToString();
            data["TopMost"] = this.TopMost ? "1" : "0";

            SaveData();
        }


        // OPTIONS LOAD XML
        void LoadData()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SoundSwitch", "options.xml");

            data.Clear();

            if (!File.Exists(path))
                return;

            var root = XElement.Load(path);

            foreach (var entry in root.Elements("item"))
            {
                var key = entry.Attribute("Key")?.Value;
                var value = entry.Attribute("Value")?.Value;

                if (key != null)
                    data[key] = value;
            }


            return;
        }

        // OPTIONS SAVE XML 
        void SaveData()
        {
            string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SoundSwitch");
            Directory.CreateDirectory(folder);
            string path = Path.Combine(folder, "options.xml");

            var root = new XElement("root");

            foreach (var pair in data)
            {
                var entry = new XElement("item",
                    new XAttribute("Key", pair.Key),
                    new XAttribute("Value", pair.Value));
                root.Add(entry);
            }

            root.Save(path);
        }

        // AUDIO DEVICES LIST
        private void LoadAudioDevices()
        {
            devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            var defaultDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

            comboBox1.Items.Clear();

            for (int i = 0; i < devices.Count; i++)
            {
                var device = devices[i];
                comboBox1.Items.Add(new AudioDeviceItem
                {
                    Name = device.FriendlyName,
                    Id = device.ID
                });

                if (device.ID == defaultDevice.ID)
                    comboBox1.SelectedIndex = i;
            }

            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
        }

        // COMBOBX AUDIO DEVICES LIST
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem is AudioDeviceItem selected)
            {
                AudioDeviceSwitcher.SetDefaultDevice(selected.Id);
            }
        }

        //EVENT LOAD FORM
        private void FormSoundSwitch_Load(object sender, EventArgs e)
        {

        }

        // VOLUME PROGRESSBAR UPDATE
        private void UpdateProgressBarValue(int mouseX)
        {
            int width = instantProgressBar1.Width;
            int value = mouseX * (instantProgressBar1.Maximum - instantProgressBar1.Minimum) / width;

            if (value < instantProgressBar1.Minimum) value = instantProgressBar1.Minimum;
            if (value > instantProgressBar1.Maximum) value = instantProgressBar1.Maximum;

            instantProgressBar1.Value = value;

            vc.SetVolume(value);
        }

        // VOLUME PROGRESSBAR UPDATE
        private void instantProgressBar1_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            UpdateProgressBarValue(e.X);
        }

        // VOLUME PROGRESSBAR UPDATE
        private void instantProgressBar1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                UpdateProgressBarValue(e.X);
            }
        }

        // VOLUME PROGRESSBAR UPDATE
        private void instantProgressBar1_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        // CONTEXTMENU OPEN
        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mostTopToolStripMenuItem.Checked = this.TopMost;
            autorunToolStripMenuItem.Checked = this.IsInAutorun();
        }

        // CONTEXTMENU Close
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // CONTEXTMENU Most Top
        private void mostTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TopMost = !this.TopMost;
            mostTopToolStripMenuItem.Checked = this.TopMost;
        }
        
        // AUTORUN
        public bool IsInAutorun()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(AutorunKeyPath, false))
            {
                return key?.GetValue(AppName) != null;
            }
        }

        // AUTORUN
        public void AddToAutorun()
        {
            string exePath = Process.GetCurrentProcess().MainModule.FileName;
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(AutorunKeyPath, true))
            {
                key.SetValue(AppName, $"\"{exePath}\"");
            }
        }

        // AUTORUN
        public void RemoveFromAutorun()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(AutorunKeyPath, true))
            {
                key.DeleteValue(AppName, false);
            }
        }

        // AUTORUN
        public string GetCurrentExecutablePath()
        {
            return Process.GetCurrentProcess().MainModule.FileName;
        }


        // CONTEXTMENU AUTORUN OPTION
        private void autorunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.IsInAutorun())
            {
                this.AddToAutorun();
            }
            else
            {
                this.RemoveFromAutorun();
            }
        }


    }
}

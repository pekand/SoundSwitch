using NAudio.CoreAudioApi;
using System.Xml.Linq;

namespace SoundSwitch
{
    public partial class FormSoundSwitch : Form
    {
        private MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
        private MMDeviceCollection devices;

        private bool isDragging = false;

        VolumeController vc = new VolumeController();

        Dictionary<string, string> data = new Dictionary<string, string>();

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
        }

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

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem is AudioDeviceItem selected)
            {
                AudioDeviceSwitcher.SetDefaultDevice(selected.Id);
            }
        }

        class AudioDeviceItem
        {
            public string Name { get; set; }
            public string Id { get; set; }
            public override string ToString() => Name;
        }

        private void FormSoundSwitch_Load(object sender, EventArgs e)
        {

        }

        private void UpdateProgressBarValue(int mouseX)
        {
            int width = instantProgressBar1.Width;
            int value = mouseX * (instantProgressBar1.Maximum - instantProgressBar1.Minimum) / width;

            if (value < instantProgressBar1.Minimum) value = instantProgressBar1.Minimum;
            if (value > instantProgressBar1.Maximum) value = instantProgressBar1.Maximum;

            instantProgressBar1.Value = value;

            vc.SetVolume(value);
        }

        private void SetVolume(int volumePercent)
        {
            // TODO: Your volume setting logic here
            // Example: call your AudioSwitcher or CoreAudio library with volumePercent
        }

        private void instantProgressBar1_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            UpdateProgressBarValue(e.X);
        }

        private void instantProgressBar1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                UpdateProgressBarValue(e.X);
            }
        }

        private void instantProgressBar1_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void FormSoundSwitch_Activated(object sender, EventArgs e)
        {
            LoadAudioDevices();
            instantProgressBar1.Value = vc.GetVolume();
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mostTopToolStripMenuItem.Checked = this.TopMost;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mostTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TopMost = !this.TopMost;
            mostTopToolStripMenuItem.Checked = this.TopMost;
        }

        private void FormSoundSwitch_FormClosing(object sender, FormClosingEventArgs e)
        {
            data["Left"] = this.Left.ToString();
            data["Top"] = this.Top.ToString();
            data["TopMost"] = this.TopMost ? "1" : "0";

            SaveData();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}

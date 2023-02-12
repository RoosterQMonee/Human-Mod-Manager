using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO.Compression;
using System.IO;
using System.ComponentModel;

namespace Human_Mod_Manager
{
    public partial class Form1 : Form
    {
        int active_menu = 0;
        string path = "";

        string ModdedLobbies = "https://github.com/RoosterQMonee/Human-Touch-Mods/raw/main/HumanModdedLobbies.zip";

        string UnityExplorer = "https://github.com/RoosterQMonee/Human-Touch-Mods/blob/main/UnityExplorer.zip?raw=true";
        string ScriptLoader = "https://github.com/RoosterQMonee/Human-Touch-Mods/blob/main/ScriptLoader.zip?raw=true";
        string HumanGrav = "https://github.com/RoosterQMonee/Human-Touch-Mods/raw/main/HumanGrav.zip";

        #region Mods

        void update_mod_list()
        {
            listBox1.Items.Clear();

            listBox1.Items.Add("[ DEV ] Unity Explorer");
            listBox1.Items.Add("[ DEV ] Script Loader");
            listBox1.Items.Add("[ Fun ] Human Grav");

            listBox2.Items.Add("Coming Soon! ( hopefully )");

            listBox3.Items.Add("[ Start ] Updated mod list.");
        }

        #endregion

        #region Form Setup

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );
        public Form1()
        {
            InitializeComponent();

            listBox3.Items.Add("[ Start ] Loaded Menu.");

            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            update_mod_list();

            CosmeticPage.SendToBack();
            LogPage.SendToBack();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        #endregion

        #region Manager Functions

        private void get_game_file_dir()
        {
            listBox3.Items.Add("[ Files ] Loaded folder prompt.");
            MessageBox.Show("I was unable to find the game files, please select the game's folder");

            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    path = fbd.SelectedPath;
                    listBox3.Items.Add("[ Files ] Game folder path set to: " + path);
            }

            textBox1.Text = path;
        }

        private void download_plugin(string URL)
        {
            if (path == "")
                get_game_file_dir();

            using (WebClient wc = new WebClient())
            {
                listBox3.Items.Add("[ Plugin ] Loaded Web Client.");
                wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                wc.DownloadFile(
                    new System.Uri(URL),
                    "./Plugin.zip"
                );

                listBox3.Items.Add("[ Plugin ] Downloaded Plugin.");

                string zipPath = "./Plugin.zip";
                string extractPath = path;

                try
                {
                    ZipFile.ExtractToDirectory(zipPath, extractPath);
                    listBox3.Items.Add("[ Plugin ] Finished Plugin setup.");
                }
                catch
                {
                    listBox3.Items.Add("[ Plugin ] Plugin already exists.");
                }
            }
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        #endregion

        #region Cosmetic Functions
        
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            panel3.BackColor = Color.FromArgb(250, 50, 50, 50);
        }

        #endregion

        #region Empty Functions

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #endregion


        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox3.Items.Add("[ Files ] Loaded folder prompt.");

            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    path = fbd.SelectedPath;
                    listBox3.Items.Add("[ Files ] Game folder path set to: " + path);
            }

            textBox1.Text = path;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (path == "")
                get_game_file_dir();

            using (WebClient wc = new WebClient())
            {
                listBox3.Items.Add("[ BepInEx ] Loaded Web Client.");

                wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                wc.DownloadFile(
                    new System.Uri("https://github.com/BepInEx/BepInEx/releases/download/v5.4.21/BepInEx_x64_5.4.21.0.zip"),
                    "./BepInEx.zip"
                );

                listBox3.Items.Add("[ BepInEx ] Downloaded BepInEx system.");

                string zipPath = "./BepInEx.zip";
                string extractPath = path;

                try
                {
                    ZipFile.ExtractToDirectory(zipPath, extractPath);
                    listBox3.Items.Add("[ BepInEx ] Finished BepInEx build.");
                }
                catch
                {
                    listBox3.Items.Add("[ BepInEx ] BepInEx is already installed.");
                }

                download_plugin(ModdedLobbies);

                if (checkBox1.Checked)
                {
                    listBox3.Items.Add("[ BepInEx ] Starting HumanTouchVR.");

                    Process.Start(path + "\\HumanTouchVR.exe");
                    listBox3.Items.Add("[ BepInEx ] Finished BepInEx setup.");
                }
                else
                {
                    MessageBox.Show("Finished! run the game to finish setup.");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (path == "")
                get_game_file_dir();

            try
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(path + "\\BepInEx\\Plugins");

                foreach (FileInfo file in di.GetFiles())
                    file.Delete();

                foreach (DirectoryInfo dir in di.GetDirectories())
                    dir.Delete(true);

                listBox3.Items.Add("[ Files ] Deleted all Mods.");
            }
            catch
            {
                MessageBox.Show("Cannot find mod folder, please run the game or select the game folder.");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (path == "")
                get_game_file_dir();

            try
            {
                Process.Start(path + "\\BepInEx\\plugins");
                listBox3.Items.Add("[ Files ] Loaded mod folder.");
            }
            catch
            {
                MessageBox.Show("Cannot find mod folder, please run the game or select the game folder.");
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void button5_Click_2(object sender, EventArgs e)
        {
            if (active_menu == 0) // Mods
            {
                string selected = listBox1.Text;

                switch (selected)
                {
                    case "[ DEV ] Unity Explorer":
                        listBox3.Items.Add("[ Plugin ] Downloading Unity Explorer");

                        download_plugin(UnityExplorer);
                        break;

                    case "[ DEV ] Script Loader":
                        listBox3.Items.Add("[ Plugin ] Downloading ScriptLoader");

                        download_plugin(ScriptLoader);
                        break;

                    case "[ Fun ] Human Grav":
                        listBox3.Items.Add("[ Plugin ] Downloading Human Grav");

                        download_plugin(HumanGrav);
                        break;
                }
            }
            if (active_menu == 1) // Cosmetics
            {
                string selected = listBox2.Text;
            }
        }

        void move_all_back()
        {
            ModPage.SendToBack();
            LogPage.SendToBack();
            CosmeticPage.SendToBack();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            listBox3.Items.Add("[ Menu ] Loaded Mod Menu.");

            move_all_back();

            active_menu = 0;
            ModPage.BringToFront();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            listBox3.Items.Add("[ Menu ] Loaded Cosmetic Menu.");

            move_all_back();

            active_menu = 1;
            CosmeticPage.BringToFront();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            listBox3.Items.Add("[ Menu ] Loaded Log Menu.");

            move_all_back();

            active_menu = 2;
            LogPage.BringToFront();
        }
    }
}
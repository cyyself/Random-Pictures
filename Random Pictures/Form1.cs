using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
public struct pic {
    public Image Img;
    public string path;
    public pic(Image _img, String _path) {
        Img = _img;
        path = _path;
    }
    public pic(String _path) {
        Img = Image.FromFile(_path);
        path = _path;
    }
}
namespace Random_Pictures {
    public partial class Form1 : Form {
        List<pic> imgs = new List<pic>();
        bool enable = true;
        bool running = true;
        Thread t;
        public Form1() {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e) {
            string[] jpgs = Directory.GetFiles(Application.StartupPath, "*.jpg", SearchOption.TopDirectoryOnly);
            string[] bmps = Directory.GetFiles(Application.StartupPath, "*.bmp", SearchOption.TopDirectoryOnly);
            string[] gifs = Directory.GetFiles(Application.StartupPath, "*.gif", SearchOption.TopDirectoryOnly);
            string[] pngs = Directory.GetFiles(Application.StartupPath, "*.png", SearchOption.TopDirectoryOnly);
            int tot = jpgs.Length + bmps.Length + gifs.Length + pngs.Length;
            if (tot == 0) {
                MessageBox.Show("No pictures found!","Random Pictures",MessageBoxButtons.OK,MessageBoxIcon.Error);
                Environment.Exit(0);
            }
            foreach (string path in jpgs) imgs.Add(new pic(path));
            foreach (string path in bmps) imgs.Add(new pic(path));
            foreach (string path in gifs) imgs.Add(new pic(path));
            foreach (string path in pngs) imgs.Add(new pic(path));
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            t = new Thread(change);
            t.IsBackground = true;
            t.Start();
        }

        void change() {
            while (running) {
                Thread.Sleep(100);
                if (!enable) continue;
                Random r = new Random();
                int ri = r.Next(imgs.Count);
                this.BackgroundImage = imgs[ri].Img;
                this.Text = Path.GetFileName(imgs[ri].path);
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == 'f') {
                if (this.BackgroundImageLayout == ImageLayout.Stretch) this.BackgroundImageLayout = ImageLayout.Zoom;
                else this.BackgroundImageLayout = ImageLayout.Stretch;
            }
            else enable = !enable;
        }

        private void Form1_Click(object sender, EventArgs e) {
            enable = !enable;
        }
    }
}

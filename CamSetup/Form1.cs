using System;
using OpenCvSharp;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.IO;

namespace CamSetup
{
    public partial class Form1 : Form
    {
        private VideoCapture video;
        private Thread thread;
        private Mat frame;
        private VideoWriter writer;
        private bool isRecording = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            video = new VideoCapture(0);
            thread = new Thread(new ThreadStart(videoThred));
            thread.Start();
        }

        private void videoThred()
        {
            Mat mat = new Mat();
            while (true)
            {
                video.Read(mat);
                pictureBox1.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(mat);
                if (isRecording)
                {
                    writer.Write(mat);
                }
                Application.DoEvents();
                if (!thread.IsAlive) break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap bmp = (Bitmap)pictureBox1.Image;
                string folderPath = @"C:\capture\picture";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string fileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
                string filePath = Path.Combine(folderPath, fileName);
                bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                MessageBox.Show("Image saved as " + fileName);
                System.Diagnostics.Process.Start("explorer.exe", folderPath);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (!isRecording)
            {
                string folderPath = @"C:\capture\record";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string fileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".avi";
                string filePath = Path.Combine(folderPath, fileName);

                writer = new VideoWriter(filePath, FourCC.DIVX, 20, new OpenCvSharp.Size(video.FrameWidth, video.FrameHeight), true);
                isRecording = true;
                button2.Text = "Stop Recording";


            }
            else
            {
                isRecording = false;
                writer.Release();
                button2.Text = "Start Recording";
                string folderPath = @"C:\capture\record";
                string fileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".avi";
                string filePath = Path.Combine(folderPath, fileName);
                MessageBox.Show("VIdeo saved as " + fileName);
                System.Diagnostics.Process.Start("explorer.exe", folderPath);

            }

        }



    }
}
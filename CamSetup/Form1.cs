using System;
using OpenCvSharp;
using System.Threading;
using System.Windows.Forms;

namespace CamSetup
{
    public partial class Form1 : Form
    {
        private VideoCapture video;
        private Thread thread;

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


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
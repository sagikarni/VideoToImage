using AxWMPLib;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VideoToImage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

       
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int index = 0;
            var files = Directory.GetFiles(txtPath.Text, "*.mp4");

            foreach (string file in files)
            {
                var video = (AxWindowsMediaPlayer)pnlVideos.Controls[8 - index];
                video.URL = (file);
                video.PlayStateChange += pauseOnFirstFrame;
                video.Tag = file;
                video.Ctlcontrols.play();
                index++;

            }
            while (index < 9)
            {
                pnlVideos.Controls.RemoveAt(0);
                index++;
            }
            pnlVideos.Visible = true;
        }


        void pauseOnFirstFrame(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 3) // playing
            {
                (sender as AxWindowsMediaPlayer).Ctlcontrols.pause();
                (sender as AxWindowsMediaPlayer).Ctlcontrols.currentPosition = 0;
                (sender as AxWindowsMediaPlayer).PlayStateChange -= pauseOnFirstFrame;
            };
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int index = 0;
            foreach (Control ctl in pnlVideos.Controls)
            {
                var video = (AxWindowsMediaPlayer)ctl;
                
                using (var engine = new Engine(@"C:\Users\sagik\source\repos\VideoSnapShot\VideoSnapShot\bin\Debug\ffmpeg.exe"))
                {
                    var mp4 = new MediaFile { Filename = ctl.Tag.ToString() };
                    var options = new ConversionOptions { Seek = TimeSpan.FromSeconds(video.Ctlcontrols.currentPosition) };
                    var outputFile = new MediaFile { Filename = string.Format("{0}\\image-{1}.jpeg", @"C:\Users\sagik\Downloads\output", index) };

                    engine.GetMetadata(mp4);
                    engine.GetThumbnail(mp4, outputFile, options);
                }
                index++;

            }

        }
    }
}

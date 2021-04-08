using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;




namespace PreviewDemo
{
    
  public   class SaveRtsp2Avi
    {
        private   Capture currentDevice;
        private  VideoWriter videoWriter;
        private  bool recording;
        private  int videoWidth;
        private  int videoHeight;

        static void main(String[] args)
        {

          new  SaveRtsp2Avi().show();

        }



        public  void show() {

          currentDevice = new Capture(@"rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mov");
            CvInvoke.UseOpenCL = false;   //不使用OpneCL
            recording = false;
            videoWidth = currentDevice.Width;
            videoHeight = currentDevice.Height;
            StartRecording_Click();
            currentDevice.ImageGrabbed += CurrentDevice_ImageGrabbed;
            currentDevice.Start();
       

        }
        public void CurrentDevice_ImageGrabbed(object sender, EventArgs e)
        {
            try
            {
                Mat m = new Mat();
                currentDevice.Retrieve(m, 0);
                if (recording && videoWriter != null)
                {
                    videoWriter.Write(m);
                }
                if (m.IsEmpty) {
                    recording = false;
                    currentDevice.Stop();
                    videoWriter.Dispose();

                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public  void StartRecording_Click()
        {
            recording = true;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".avi";
            dialog.AddExtension = true;
            dialog.FileName = DateTime.Now.ToString("yyyyMMddHHmmss");
            DialogResult dialogResult = dialog.ShowDialog();
            if (dialogResult != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            videoWriter = new VideoWriter(dialog.FileName, VideoWriter.Fourcc('M', 'J', 'P', 'G'), 30, new System.Drawing.Size(videoWidth, videoHeight), true);
           
            
        }


        public  void StopRecording_Click()
        {
            recording = false;
            if (videoWriter != null)
            {
                currentDevice.Stop();
                videoWriter.Dispose();
            }
        }


        }


    }


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PreviewDemo
{
    public partial class VlcPlayerForm : Form
    {
        private VlcPlayer vlc_player_;
        private bool is_playinig_;
        private PictureBox pb;

        public VlcPlayerForm()
        {
            InitializeComponent();


            panel1.Controls.Clear();

            FlowLayoutPanel layout = new FlowLayoutPanel();
            layout.Dock = DockStyle.Fill;

            pb = new PictureBox();
            pb.Width = panel1.Width - 8;
            pb.Height = panel1.Height - 8;
            pb.Name = string.Format("PictureBox{0}", 1);
            pb.BackColor = System.Drawing.Color.Red;

            layout.Controls.Add(pb);
            this.panel1.Controls.Add(layout);

            string pluginPath = System.Environment.CurrentDirectory + "\\vlc\\plugins\\";
            vlc_player_ = new VlcPlayer(pluginPath);
            IntPtr render_wnd = pb.Handle;
            vlc_player_.SetRenderWindow((int)render_wnd);



           vlc_player_.PlayUrl("rtsp://184.72.239.149/vod/mp4://BigBuckBunny_175k.mov");
         //   vlc_player_.PlayUrl("rtsp://rtsp-v3-spbtv.msk.spbtv.com/spbtv_v3_1/214_110.sdp");
            is_playinig_ = true;


            //System.Threading.Thread.Sleep(20000);

         //   vlc_player_.PlayUrl("rtsp://rtsp-v3-spbtv.msk.spbtv.com/spbtv_v3_1/214_110.sdp");
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

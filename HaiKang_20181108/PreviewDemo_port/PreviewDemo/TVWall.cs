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
    public partial class TVWall : Form
    {

        private Int32 m_lUserID = -1;
        private uint iLastErr = 0;
        private string str;
        private Int32 m_lRealHandle = -1;

        public TVWall()
        {
            InitializeComponent();
        }

        private void TVWall_Load(object sender, EventArgs e)
        {
            CreatePictureBox();
        }


        /// <summary>
        /// 创建 视频播放的面板（视频窗口的载体）
        /// </summary>
        private void CreatePictureBox() {


            panel1.Controls.Clear();

            FlowLayoutPanel layout = new FlowLayoutPanel();
            layout.Dock = DockStyle.Fill;
            
                PictureBox pb = new PictureBox();
                pb.Width = panel1.Width  - 8;
                pb.Height = panel1.Height  - 8;
                pb.Name = string.Format("PictureBox{0}", 1);
                pb.BackColor = System.Drawing.Color.Black;
                //pb.Paint += new PaintEventHandler(RealPlayWnd_Paint);
                
                layout.Controls.Add(pb);

            this.panel1.Controls.Add(layout);

            loginDevice("192.168.1.64", 8000, "admin", "Transcend");

            cameraPlay(pb);

        }

 

        /// <summary>
        ///   登陆摄像头   
        /// </summary>
        /// <param name="DVRIPAddress">  摄像头的Ip地址</param>
        /// <param name="DVRPortNumber"> 摄像头端口号</param>
        /// <param name="DVRUserName"> 摄像头的登陆名</param>
        /// <param name="DVRPassword"> 摄像头登录密码</param>
        private void loginDevice(string DVRIPAddress, int DVRPortNumber, string DVRUserName, string DVRPassword)
        {

            CHCNetSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();

            //登录设备 Login the device
            m_lUserID = CHCNetSDK.NET_DVR_Login_V30(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, ref DeviceInfo);
            if (m_lUserID < 0)
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                str = "NET_DVR_Login_V30 failed, error code= " + iLastErr; //登录失败，输出错误号
                MessageBox.Show(str);
                return;
            }
            else
            {
                //登录成功
                MessageBox.Show("Login Success!");
              //  btnLogin.Text = "Logout";
            }
        }

        /// <summary>
        /// 摄像头在制定的pictureBox上进行播放
        /// </summary>
        private void cameraPlay( PictureBox pb) {
            if (m_lUserID < 0)
            {
                MessageBox.Show("Please login the device firstly");
                return;
            }

            if (m_lRealHandle < 0)
            {
                CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO();
                lpPreviewInfo.hPlayWnd = pb.Handle;//预览窗口
                lpPreviewInfo.lChannel = Int16.Parse("1");//预te览的设备通道
                lpPreviewInfo.dwStreamType = 0;//码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
                lpPreviewInfo.dwLinkMode = 0;//连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
                lpPreviewInfo.bBlocked = true; //0- 非阻塞取流，1- 阻塞取流
                lpPreviewInfo.dwDisplayBufNum = 15; //播放库播放缓冲区最大缓冲帧数

                CHCNetSDK.REALDATACALLBACK RealData = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack);//预览实时流回调函数
                IntPtr pUser = new IntPtr();//用户数据

                //打开预览 Start live view 
                m_lRealHandle = CHCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, null/*RealData*/, pUser);
                if (m_lRealHandle < 0)
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_RealPlay_V40 failed, error code= " + iLastErr; //预览失败，输出错误号
                    MessageBox.Show(str);
                    return;
                }
                else
                {
                    //预览成功
                //    btnPreview.Text = "Stop Live View";
                }
            }

        
        }

        public void RealDataCallBack(Int32 lRealHandle, UInt32 dwDataType, ref byte pBuffer, UInt32 dwBufSize, IntPtr pUser)
        {
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

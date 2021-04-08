using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;

using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;

//using System.Runtime.InteropServices;
namespace PreviewDemo
{   
    
    /// <summary>
    /// Form1 的摘要说明。
    /// </summary>
    public class Preview : System.Windows.Forms.Form
    {
        static Socket Server;
        private static int f4 = 0;
        private static int f9 = 0;
        private string sbl;
        private uint dwPTZPresetCmd;
        static System.Threading.Timer timerforico;
        static System.Threading.Timer timerforfp;
        static System.Threading.Timer timerfor9;
        private uint dwPresetIndex;
        private uint SET_PRESET = 8;//设置预置点
        private uint CLE_PRESET = 9;
        private uint GOTO_PRESET = 39;//转到预置点
        private Int32 msgCount = 0;
        private uint iLastErr = 0;
        private Int32 m_lUserID = -1;
        private bool m_bInitSDK = false;
        private bool m_bRecord = false;
        private int lunxunINT = 1;//轮询次数
        private int sleeptime = 1;
       static  int IPCCOUNT;
        List<VlcPlayer> listlxrtspforfp = new List<VlcPlayer>();
        List<int> liststrforfp = new List<int>();
        List<int> liststr4 = new List<int>();
        Screen[] sc;
        private m_lRealHandleClass m_lRealHandleModel = new m_lRealHandleClass();
        System.Windows.Forms.Form frm2 = new Form();
        
       // frm2.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      
        DateTime fiveM = new DateTime();

        private class m_lRealHandleClass
        {
            public Int32 m_lRealHandle1 = -1;
            public Int32 m_lRealHandle2 = -1;
            public Int32 m_lRealHandle3 = -1;
            public Int32 m_lRealHandle4 = -1;
            public Int32 m_lRealHandle5 = -1;
            public Int32 m_lRealHandle6 = -1;
            public Int32 m_lRealHandle7 = -1;
            public Int32 m_lRealHandle8 = -1;
            public Int32 m_lRealHandle9 = -1;
            public Int32 m_lRealHandle10 = -1;
            public Int32 m_lRealHandle11 = -1;
            public Int32 m_lRealHandle12 = -1;
            public Int32 m_lRealHandle13 = -1;
            public Int32 m_lRealHandle14 = -1;
            public Int32 m_lRealHandle15 = -1;
            public Int32 m_lRealHandle16 = -1;
            public Int32 m_lRealHandle17 = -1;
            public Int32 m_lRealHandle18 = -1;
            public Int32 m_lRealHandle19 = -1;
            public Int32 m_lRealHandle20 = -1;
            public Int32 m_lRealHandle21 = -1;
            public Int32 m_lRealHandle22= -1;
            public Int32 m_lRealHandle23 = -1;
            public Int32 m_lRealHandle24 = -1;
            public Int32 m_lRealHandle25 = -1;
            public Int32 m_lRealHandle26 = -1;
            public Int32 m_lRealHandle27 = -1;
            public Int32 m_lRealHandle28 = -1;
        }
        private Int32 m_lRealHandle = -1;

        private string str;
        private TreeView treeView1;
        private IContainer components;
        private Size m_szInit;
        public DataGridView dataGridView1;
        private DataGridViewTextBoxColumn No;
        private DataGridViewTextBoxColumn Msg;
        private Button button14;
        private Panel panel1;
        private System.Windows.Forms.Timer timer1;


        private Dictionary<Control, Rectangle> m_dicSize
            = new Dictionary<Control, Rectangle>();


        /// <summary>
        /// 使用vlc 播放用到的 参数
        /// </summary>
        private VlcPlayer vlc_player_;
        private bool is_playinig_;
        private ImageList imageList1;
        private System.Windows.Forms.Timer timer4;


        private vlcplayerClass m_vlcplayerModel = new vlcplayerClass();
        private class vlcplayerClass
        {
            static string pluginPath = System.Environment.CurrentDirectory + "\\vlc\\plugins\\";
            
            public VlcPlayer vlc_player_1 = new VlcPlayer(pluginPath);
            public VlcPlayer vlc_player_2 = new VlcPlayer(pluginPath);
            public VlcPlayer vlc_player_3 = new VlcPlayer(pluginPath);
            public VlcPlayer vlc_player_4 = new VlcPlayer(pluginPath);
            public VlcPlayer vlc_player_5 = new VlcPlayer(pluginPath);
            public VlcPlayer vlc_player_6 = new VlcPlayer(pluginPath);
            public VlcPlayer vlc_player_7 = new VlcPlayer(pluginPath);
            public VlcPlayer vlc_player_8 = new VlcPlayer(pluginPath);
            public VlcPlayer vlc_player_9 = new VlcPlayer(pluginPath);
            public VlcPlayer vlc_player_10 = new VlcPlayer(pluginPath);
            public VlcPlayer vlc_player_11 = new VlcPlayer(pluginPath);
            public VlcPlayer vlc_player_12 = new VlcPlayer(pluginPath);
            public VlcPlayer vlc_player_13 = new VlcPlayer(pluginPath);
            public VlcPlayer vlc_player_14 = new VlcPlayer(pluginPath);
            public VlcPlayer vlc_player_15 = new VlcPlayer(pluginPath);

        }

        protected override void OnLoad(EventArgs e)
        {
            m_szInit = this.Size;//获取初始大小
            this.GetInitSize(this);
            base.OnLoad(e);
        }

        private void GetInitSize(Control ctrl)
        {
            foreach (Control c in ctrl.Controls)
            {
                m_dicSize.Add(c, new Rectangle(c.Location, c.Size));
                this.GetInitSize(c);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            //计算当前大小和初始大小的比例
            float fx = (float)this.Width / m_szInit.Width;
            float fy = (float)this.Height / m_szInit.Height;
            foreach (var v in m_dicSize)
            {
                v.Key.Left = (int)(v.Value.Left * fx);
                v.Key.Top = (int)(v.Value.Top * fy);
                v.Key.Width = (int)(v.Value.Width * fx);
                v.Key.Height = (int)(v.Value.Height * fy);
            }
            base.OnResize(e);
        }
        public Preview()
        {
            //
            // Windows 窗体设计器支持所必需的
            //
            InitializeComponent();

            m_bInitSDK = CHCNetSDK.NET_DVR_Init();
            if (m_bInitSDK == false)
            {
                MessageBox.Show("NET_DVR_Init error!");
                return;
            }
            else
            {

                //保存SDK日志 To save the SDK log
                CHCNetSDK.NET_DVR_SetLogToFile(3, "C:\\SdkLog\\", true);
            }

            //
            // TODO: 在 InitializeComponent 调用后添加任何构造函数代码
            //

        }


        /// <summary>
        /// 用于检测当前pc有几块显示器  软件显示在那个显示器上  -lcq
        /// </summary>
        /// <param name="showOnMonitor"> 软件显示在第几块显示器上  从0开始 </param>
        private void showOnMonitor(int showOnMonitor)
        {
            Screen[] sc;
            sc = Screen.AllScreens;
            if (showOnMonitor >= sc.Length)
            {
                showOnMonitor = 0;
            }
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(sc[showOnMonitor].Bounds.Left, sc[showOnMonitor].Bounds.Top);
        }
        /// <summary>
        /// 用于检测当前pc是否有第二块显示器  软件显示在那个显示器上  -qhz
        /// </summary>
        /// <param name="showOnMonitorfor2"> 用来绘制第二块显示器上的窗口 </param>
      

        private void showonmonitorfor2()
        {

            sc = Screen.AllScreens;
            
            if (Screen.AllScreens.Length ==2)
            {
                frm2.Left = ((Screen.AllScreens[1].Bounds.Width - this.Width) / 2);
                frm2.Top = ((Screen.AllScreens[1].Bounds.Height - this.Height) / 2);
                frm2.Size = new System.Drawing.Size(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height);
                frm2.StartPosition = FormStartPosition.Manual;
                frm2.Location = new Point(sc[0].Bounds.Left, sc[0].Bounds.Top);
                frm2.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                frm2.FormClosing+=frm2_FormClosing;
                //frm2.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                frm2.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                createPicBox(4);
                
                frm2.Show();//弹出f
                // frm2.ShowDialog();

            }
        }
        //禁止分屏窗口关闭
        private void frm2_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            
        }
        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (m_lRealHandle >= 0)
            {
                CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle);
            }
            if (m_lUserID >= 0)
            {
                CHCNetSDK.NET_DVR_Logout(m_lUserID);
            }
            if (m_bInitSDK == true)
            {
                CHCNetSDK.NET_DVR_Cleanup();
            }

            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码
        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Preview));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Msg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button14 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.timer4 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(12, 12);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(159, 641);
            this.treeView1.TabIndex = 24;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.No,
            this.Msg});
            this.dataGridView1.Location = new System.Drawing.Point(177, 541);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(830, 112);
            this.dataGridView1.TabIndex = 28;
            // 
            // No
            // 
            this.No.FillWeight = 111.6751F;
            this.No.HeaderText = "No.";
            this.No.Name = "No";
            this.No.ReadOnly = true;
            this.No.Width = 110;
            // 
            // Msg
            // 
            this.Msg.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Msg.FillWeight = 88.32487F;
            this.Msg.HeaderText = "Msg";
            this.Msg.Name = "Msg";
            this.Msg.ReadOnly = true;
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(7, 45);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(144, 24);
            this.button14.TabIndex = 3;
            this.button14.Text = "回放";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(177, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(830, 523);
            this.panel1.TabIndex = 25;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "DevOffline.ico");
            this.imageList1.Images.SetKeyName(1, "DevOnline.ico");
            this.imageList1.Images.SetKeyName(2, "home.ico");
            this.imageList1.Images.SetKeyName(3, "alarm.ico");
            this.imageList1.Images.SetKeyName(4, "Train.ico");
            // 
            // timer4
            // 
            this.timer4.Tick += new System.EventHandler(this.timer4_Tick);
            // 
            // Preview
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1012, 668);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.treeView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Preview";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CCTV";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.form_closing);
            this.Load += new System.EventHandler(this.Preview_Load);
            this.Shown += new System.EventHandler(this.preview_show);
            this.SizeChanged += new System.EventHandler(this.Previer_reload);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
           Application.Run(new Preview());

        }
        //关于分屏轮询
        void Timerfp() {
          
            //TimerCallback是指向一个object类型参数的无返回值方法的委托，供定时器回调使用
            TimerCallback timerCallback = new TimerCallback(Method5);//把Method5的方法加入委托中
            timerCallback += o => { Console.WriteLine("定时调用方法：Lambda表达式方法"); };//把lambda表达式加入委托中
            timerCallback += delegate(object o)//把匿名方法加入到委托中
            {
                Console.WriteLine("定时调用方法：匿名方法");
            };
           
            if (frm2.Controls[0].Controls.Count == 4)
            {
                  // if(ll.Count==34){
                   timerforfp = new System.Threading.Timer(timerCallback,//参数定义定时时处理的方法或方法列表（以委托实现）
                    null,                             //此参数定义传递给方法的object类型参数，用null表明不传递参数
                   10000,                                //此参数表明定时器开始的时间，0标示立即开始
                    (int.Parse("30")-4) * 1000);                            //此参数标示定时处理的事件间隔，以毫秒为单位
               // }
            }
        
        
        }
        CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfofp;
        private static int fp = 0;
        void Method5(object o) {
            sxtClearforfp();
            vlcClearforfp();
            Thread.Sleep(100);
            cbb = 0;
            for (int i = 0; i < frm2.Controls[0].Controls.Count; i++)
            {
                PictureBox pic = frm2.Controls[0].Controls[i] as PictureBox;
                pic.Invalidate();
            }
            Console.WriteLine(" ：Method5；执行时间：" + DateTime.Now.ToLongTimeString());
            Console.WriteLine("托管线程ID为：" + Thread.CurrentThread.ManagedThreadId);
            for (; fp < treeView1.Nodes[0].Nodes.Count; )
            {
                Dictionary<string, string> d = treeView1.Nodes[0].Nodes[fp].Tag as Dictionary<string, string>;
                foreach (var l in ll)
                {
                    if (d["IPC_NAME"].ToString() == l[4])
                    {
                        String IPC_IP = l[0];
                        String IPC_USER = l[2];
                        String IPC_PWD = l[3];
                        String IPC_PORT=l[1];
                        int i = int.Parse(l[5]);
                        String IPC_NAME = l[4];
                        PictureBox pb = frm2.Controls[0].Controls[cbb++] as PictureBox;
                     //   if (d["IPC_NAME"].ToString() == "行车摄像机1-1" || d["IPC_NAME"].ToString() == "客室1-1" || d["IPC_NAME"].ToString() == "客室1-2" || d["IPC_NAME"].ToString() == "客室1-3" || d["IPC_NAME"].ToString() == "客室1-4" || d["IPC_NAME"].ToString() == "客室2-1" || d["IPC_NAME"].ToString() == "客室2-2" || d["IPC_NAME"].ToString() == "客室2-3" || d["IPC_NAME"].ToString() == "客室2-4" || d["IPC_NAME"].ToString() == "客室3-1" || d["IPC_NAME"].ToString() == "客室3-2" || d["IPC_NAME"].ToString() == "客室3-3" || d["IPC_NAME"].ToString() == "客室3-4" || d["IPC_NAME"].ToString() == "客室4-1" || d["IPC_NAME"].ToString() == "客室4-2" || d["IPC_NAME"].ToString() == "客室4-3" || d["IPC_NAME"].ToString() == "客室4-4" || d["IPC_NAME"].ToString() == "客室5-1" || d["IPC_NAME"].ToString() == "客室5-2" || d["IPC_NAME"].ToString() == "客室5-3" || d["IPC_NAME"].ToString() == "客室5-4" || d["IPC_NAME"].ToString() == "客室6-1" || d["IPC_NAME"].ToString() == "客室6-2" || d["IPC_NAME"].ToString() == "客室6-3" || d["IPC_NAME"].ToString() == "客室6-4" || d["IPC_NAME"].ToString() == "行车摄像机2-1")
                           // if (d["IPC_IP"].ToString() == "192.168.1.1" || d["IPC_IP"].ToString() == "192.168.1.2" || d["IPC_IP"].ToString() == "192.168.1.3" || d["IPC_IP"].ToString() == "192.168.1.4" || d["IPC_IP"].ToString() == "192.168.1.5" || d["IPC_IP"].ToString() == "192.168.1.6" || d["IPC_IP"].ToString() == "192.168.1.7" || d["IPC_IP"].ToString() == "192.168.1.8" || d["IPC_IP"].ToString() == "192.168.1.9" || d["IPC_IP"].ToString() == "192.168.1.10" || d["IPC_IP"].ToString() == "192.168.1.11" || d["IPC_IP"].ToString() == "192.168.1.12" || d["IPC_IP"].ToString() == "192.168.1.13" || d["IPC_IP"].ToString() == "192.168.1.14" || d["IPC_IP"].ToString() == "192.168.1.15" || d["IPC_IP"].ToString() == "192.168.1.16" || d["IPC_IP"].ToString() == "192.168.1.17" || d["IPC_IP"].ToString() == "192.168.1.18" || d["IPC_IP"].ToString() == "192.168.1.19" || d["IPC_IP"].ToString() == "192.168.1.20" || d["IPC_IP"].ToString() == "192.168.1.21" || d["IPC_IP"].ToString() == "192.168.1.22" || d["IPC_IP"].ToString() == "192.168.1.23" || d["IPC_IP"].ToString() == "192.168.1.24" || d["IPC_IP"].ToString() == "192.168.1.25" || d["IPC_IP"].ToString() == "192.168.1.26" || d["IPC_IP"].ToString() == "192.168.101.47" || d["IPC_IP"].ToString() == "192.168.101.48")
                            if(d["IPC_RTSP"].ToString()=="1")
                        {
                                try
                            {
                                get_vlcplayer = (VlcPlayer)m_vlcplayerModel.GetType().GetField(string.Format("vlc_player_{0}", pb.TabIndex + 10)).GetValue(m_vlcplayerModel);
                                if (get_vlcplayer != null)
                                {
                                    pb.Invoke(new Action(() => {
                                        listlxrtspforfp.Add(get_vlcplayer);
                                        IntPtr render_wnd = pb.Handle;
                                        get_vlcplayer.SetRenderWindow((int)render_wnd);
                                    
                                    }));
                                    get_vlcplayer.PlayFromURL("rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mov");
                                   // get_vlcplayer.PlayFromURL("rtsp://"+IPC_USER+":"+IPC_PWD+"@" + IPC_IP + ":"+IPC_PORT+ "/h264/ch1/main/av_stream --rtsp-tcp");
                                    //设置分屏播放时rtsp视频流视频画面的比例，使其填充完整个窗口
                                    
                                    get_vlcplayer.SetRatio(sbl2);
                                    String str = String.Format("分屏播放的车载摄像头名称：{0},IP地址为：{1},窗口索引为{2}", IPC_NAME, IPC_IP,cbb);
                                    addMsg(str);
                                    fp++;
                                    break;

                                }
                            }
                            catch
                            {
                                //return false;
                            }
                        }
                        else
                        {
                            try
                            {
                                //alter by qhz
                                pb.Invoke(new Action(() => { lpPreviewInfofp = CreatePreview(pb.Handle); }));
                                CHCNetSDK.REALDATACALLBACK RealData = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack);//预览实时流回调函数
                                IntPtr pUser = new IntPtr();//用户数据
                                int error = -1;
                                if (i < 0)
                                {
                                    pb.Invoke(new Action(() => {
                                        error = CHCNetSDK.NET_DVR_RealPlay_V40(i, ref lpPreviewInfofp, null/*RealData*/, pUser);
                                        liststrforfp.Add(error);
                                        m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", pb.TabIndex + 10)).SetValue(m_lRealHandleModel, error);
                                    }));
                                   
                                  
                                    if (error < 0)
                                    {
                                        iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                                        str = string.Format("综合实验平台_{0}:摄像机索引{1} 分屏播放窗口索引{2},错误码{3}", IPC_NAME, i, cbb, iLastErr); //登录失败，输出错误号
                                        addMsg(str);
                                        fp++;
                                        break;
                                    }
                                    else
                                    {
                                        str = string.Format("综合实验平台_{0}:摄像机索引{1} 分屏播放窗口索引{2}", IPC_NAME, error, cbb ); //登录失败，输出错误号
                                        addMsg(str);
                                        fp++;
                                        break;
                                    }
                                }
                                else
                                {
                                    pb.Invoke(new Action(() =>
                                    {
                                        error = CHCNetSDK.NET_DVR_RealPlay_V40(i, ref lpPreviewInfofp, null/*RealData*/, pUser);
                                        liststrforfp.Add(error);
                                        m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", pb.TabIndex + 10)).SetValue(m_lRealHandleModel, error);
                                    }));
                                   
                                    if (error < 0)
                                    {
                                        iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                                        str = string.Format("综合实验平台:摄像机索引{0} 分屏播放窗口索引{2},错误码{1}", IPC_NAME, /*i*/iLastErr, cbb);
                                        fp++;
                                        break;
                                    }
                                    else
                                    {
                                        str = string.Format("综合实验平台:摄像机索引{0} 分屏播放窗口索引{2}", IPC_NAME, /*i*/error, cbb); //登录失败，输出错误号
                                        addMsg(str);
                                        Thread.Sleep(100);
                                        fp++;
                                        break;
                                    }

                                }
                            }
                            catch (Exception error)
                            {
                                WriteLog.Write(GetExceptionMsg(error, string.Empty));
                                throw error;
                            }
                        }
                    }

                }
               // if (d["IPC_NAME"].ToString() == "席位区-枪型摄像机1" || d["IPC_NAME"].ToString() == "席位区-枪型摄像机5" || d["IPC_IP"].ToString() == "192.168.1.3" || d["IPC_IP"].ToString() == "192.168.1.7" || d["IPC_IP"].ToString() == "192.168.1.11" || d["IPC_IP"].ToString() == "192.168.1.15" || d["IPC_IP"].ToString() == "192.168.1.19" || d["IPC_IP"].ToString() == "192.168.1.23")
               if(d["IPC_V4"].ToString()=="1")
                {
                    cbb = 0;
                    break;
                }
                //else if (d["IPC_PORT"].ToString() == "11048")
                else if(d["IPC_V4_END"].ToString()=="1")
                {
                    cbb = 0;
                    fp = 0;
                    break;
                }
               
            }
        
        }
        //一画面轮询
        //void Timer1()
        //{
        //    try {
        //        timerfor9.Change(Timeout.Infinite, 1000);
        //        timerfor9.Dispose();
        //    }
        //    catch {
            
        //    }
        //    //TimerCallback是指向一个object类型参数的无返回值方法的委托，供定时器回调使用
        //    TimerCallback timerCallback = new TimerCallback(Method4);//把Method1的方法加入委托中
        //    timerCallback += o => { Console.WriteLine("定时调用方法：Lambda表达式方法"); };//把lambda表达式加入委托中
        //    timerCallback += delegate(object o)//把匿名方法加入到委托中
        //    {
        //        Console.WriteLine("定时调用方法：匿名方法");
        //    };
        //    if (panel1.Controls[0].Controls.Count == 1)
        //    {
                
        //                timerfor9 = new System.Threading.Timer(timerCallback,//参数定义定时时处理的方法或方法列表（以委托实现）
        //                    null,                             //此参数定义传递给方法的object类型参数，用null表明不传递参数
        //                    0,                                //此参数表明定时器开始的时间，0标示立即开始
        //                    (int.Parse(txtTime.Text.ToString())) * 1000);                            //此参数标示定时处理的事件间隔，以毫秒为单位
        //    }

        //}

        CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo;
        private static int f1 = 0;
        void Method4(object o)
        {
          
            sxtClearfor1change();
            vlcClear();
            Thread.Sleep(100);
            for (int i = 0; i < panel1.Controls[0].Controls.Count; i++)
            {
                PictureBox pic = panel1.Controls[0].Controls[i].Controls[0] as PictureBox;
                pic.Invalidate();
            }
            Console.WriteLine("定时调用方法：Method3；执行时间：" + DateTime.Now.ToLongTimeString());
            Console.WriteLine("托管线程ID为：" + Thread.CurrentThread.ManagedThreadId);
            for (; f1 < treeView1.Nodes[0].Nodes.Count; )
            {
                Dictionary<string, string> d = treeView1.Nodes[0].Nodes[f1].Tag as Dictionary<string, string>;
                foreach (var l in ll)
                {
                    if (d["IPC_NAME"].ToString() == l[4])
                    {
                        String IPC_IP = l[0];
                        String IPC_USER = l[2];
                        String IPC_PWD=l[3];
                        String IPC_PORT=l[1];
                        int i = int.Parse(l[5]);
                        String IPC_NAME = l[4];
                        String IPC_V1_END = l[11];
                        PictureBox pb = panel1.Controls[0].Controls[0].Controls[0] as PictureBox;
                        // if (d["IPC_NAME"].ToString() == "行车摄像机1-1" || d["IPC_NAME"].ToString() == "客室1-1" || d["IPC_NAME"].ToString() == "客室1-2" || d["IPC_NAME"].ToString() == "客室1-3" || d["IPC_NAME"].ToString() == "客室1-4" || d["IPC_NAME"].ToString() == "客室2-1" || d["IPC_NAME"].ToString() == "客室2-2" || d["IPC_NAME"].ToString() == "客室2-3" || d["IPC_NAME"].ToString() == "客室2-4" || d["IPC_NAME"].ToString() == "客室3-1" || d["IPC_NAME"].ToString() == "客室3-2" || d["IPC_NAME"].ToString() == "客室3-3" || d["IPC_NAME"].ToString() == "客室3-4" || d["IPC_NAME"].ToString() == "客室4-1" || d["IPC_NAME"].ToString() == "客室4-2" || d["IPC_NAME"].ToString() == "客室4-3" || d["IPC_NAME"].ToString() == "客室4-4" || d["IPC_NAME"].ToString() == "客室5-1" || d["IPC_NAME"].ToString() == "客室5-2" || d["IPC_NAME"].ToString() == "客室5-3" || d["IPC_NAME"].ToString() == "客室5-4" || d["IPC_NAME"].ToString() == "客室6-1" || d["IPC_NAME"].ToString() == "客室6-2" || d["IPC_NAME"].ToString() == "客室6-3" || d["IPC_NAME"].ToString() == "客室6-4" || d["IPC_NAME"].ToString() == "行车摄像机2-1")
                        //  if (d["IPC_IP"].ToString() == "192.168.1.1" || d["IPC_IP"].ToString() == "192.168.1.2" || d["IPC_IP"].ToString() == "192.168.1.3" || d["IPC_IP"].ToString() == "192.168.1.4" || d["IPC_IP"].ToString() == "192.168.1.5" || d["IPC_IP"].ToString() == "192.168.1.6" || d["IPC_IP"].ToString() == "192.168.1.7" || d["IPC_IP"].ToString() == "192.168.1.8" || d["IPC_IP"].ToString() == "192.168.1.9" || d["IPC_IP"].ToString() == "192.168.1.10" || d["IPC_IP"].ToString() == "192.168.1.11" || d["IPC_IP"].ToString() == "192.168.1.12" || d["IPC_IP"].ToString() == "192.168.1.13" || d["IPC_IP"].ToString() == "192.168.1.14" || d["IPC_IP"].ToString() == "192.168.1.15" || d["IPC_IP"].ToString() == "192.168.1.16" || d["IPC_IP"].ToString() == "192.168.1.17" || d["IPC_IP"].ToString() == "192.168.1.18" || d["IPC_IP"].ToString() == "192.168.1.19" || d["IPC_IP"].ToString() == "192.168.1.20" || d["IPC_IP"].ToString() == "192.168.1.21" || d["IPC_IP"].ToString() == "192.168.1.22" || d["IPC_IP"].ToString() == "192.168.1.23" || d["IPC_IP"].ToString() == "192.168.1.24" || d["IPC_IP"].ToString() == "192.168.1.25" || d["IPC_IP"].ToString() == "192.168.1.26" || d["IPC_IP"].ToString() == "192.168.101.47" || d["IPC_IP"].ToString() == "192.168.101.48")
                        if(d["IPC_RTSP"].ToString()=="1")
                        {
                            try
                            {
                                get_vlcplayer = (VlcPlayer)m_vlcplayerModel.GetType().GetField(string.Format("vlc_player_{0}", pb.Parent.TabIndex + 1)).GetValue(m_vlcplayerModel);
                                if (get_vlcplayer != null)
                                {
                                    pb.Invoke(new Action(() => {
                                        IntPtr render_wnd = pb.Handle;
                                        get_vlcplayer.SetRenderWindow((int)render_wnd);
                                        get_vlcplayer.PlayFromURL("rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mov");
                                      //  get_vlcplayer.PlayFromURL("rtsp://" + IPC_USER + ":" + IPC_PWD + "@" + IPC_IP + ":" + IPC_PORT + "/h264/ch1/main/av_stream --rtsp-tcp");
                                        listforVlc.Add(get_vlcplayer);
                                        get_vlcplayer.SetRatio(sbl);
                                        EnableWindow(render_wnd, false);
                                    }));
                                    String str = String.Format("播放的车载摄像头名称：{0},IP ：{1},窗口索引为{2}", IPC_NAME, IPC_IP, 0);
                                    addMsg(str);
                                    pb.Parent.MouseClick += new MouseEventHandler(RealPlayWnd_Click);
                                    pb.Parent.MouseDoubleClick += new MouseEventHandler(PictureBox_MouseDoubleClick);
                                    f1++;
                                    break;
                                }
                            }
                            catch(Exception e)
                            {
                                throw e;
                            }
                        }
                        else
                        {
                            
                            try
                            {
                                //CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = CreatePreview(pb.Handle);
                                pb.Invoke(new Action(() => { lpPreviewInfo = CreatePreview(pb.Handle); }));
                                CHCNetSDK.REALDATACALLBACK RealData = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack);//预览实时流回调函数
                                IntPtr pUser = new IntPtr();//用户数据
                                int error = -1;
                                if (i < 0)
                                {

                                    pb.Invoke(new Action(() => { error = CHCNetSDK.NET_DVR_RealPlay_V40(i, ref lpPreviewInfo, null/*RealData*/, pUser);
                                    m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", pb.Parent.TabIndex + 1)).SetValue(m_lRealHandleModel, error);
                                    }));
                                    liststr4.Add(error);
                                    
                                    if (error < 0)
                                    {
                                        iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                                        str = string.Format("综合实验平台:摄像机：{0} 播放窗口索引{2},错误码：{3}", IPC_NAME, i, pb.Parent.TabIndex + 1, iLastErr); //登录失败，输出错误号
                                        addMsg(str);
                                        f1++;
                                        break;
                                    }
                                    else
                                    {
                                        str = string.Format("综合实验平台:摄像机：{0} 播放窗口索引{2}", IPC_NAME, error, pb.Parent.TabIndex + 1); //登录失败，输出错误号
                                        addMsg(str);
                                        f1++;
                                        break;
                                    }
                                }
                                else
                                {
                                    CHCNetSDK.NET_DVR_StopRealPlay(i);//关闭原视频流 
                                    pb.Invoke(new Action(() => { 
                                    error = CHCNetSDK.NET_DVR_RealPlay_V40(i, ref lpPreviewInfo, null/*RealData*/, pUser);
                                    m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", pb.Parent.TabIndex + 1)).SetValue(m_lRealHandleModel, error);
                                    liststr4.Add(error);
                                    }));
                                    if (error < 0)
                                    {
                                        iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                                        str = string.Format("综合实验平台:摄像机IP:{0} 播放窗口索引{2},{3},设备已离线", IPC_IP, error, pb.Parent.TabIndex + 1, iLastErr); //登录失败，输出错误号
                                        addMsg(str);
                                        f1++;
                                        break;
                                    }
                                    else
                                    {    //alter by qhz
                                        str = string.Format("综合实验平台:摄像机:{0} 播放窗口索引{2}", IPC_NAME, /*i*/error, pb.Parent.TabIndex + 1); //登录失败，输出错误号
                                        addMsg(str);
                                        f1++;
                                        break;
                                    }

                                }
                            }
                            catch (Exception error)
                            {
                                WriteLog.Write(GetExceptionMsg(error, string.Empty));
                                throw error;
                                // return;
                            }
                        }
                    }
                }
                if (d["IPC_V1_END"].ToString() == "0") { break; }
                else{
                    f1 = 0;
                    break;
                }
                //if(f1>=0&&f1<=34){
                //    break;
                //}else if(f1==35){
                //    f1 = 0;
                //    break;
                //}
                
                //if (d["IPC_NAME"].ToString() == "席位区-枪型摄像机5" || d["IPC_NAME"].ToString() == "席位区-枪型摄像机4" || d["IPC_NAME"].ToString() == "席位区-枪型摄像机3" || d["IPC_NAME"].ToString() == "席位区-枪型摄像机2" || d["IPC_NAME"].ToString() == "席位区-枪型摄像机1" || d["IPC_NAME"].ToString() == "车站区-球型摄像机" || d["IPC_NAME"].ToString() == "中心区-枪型摄像机2" || d["IPC_NAME"].ToString() == "中心区-枪型摄像机1" || d["IPC_NAME"].ToString() == "行车摄像机1-1" || d["IPC_NAME"].ToString() == "客室1-1" || d["IPC_NAME"].ToString() == "客室1-2" || d["IPC_NAME"].ToString() == "客室1-3" || d["IPC_NAME"].ToString() == "客室1-4" || d["IPC_NAME"].ToString() == "客室2-1" || d["IPC_NAME"].ToString() == "客室2-2" || d["IPC_NAME"].ToString() == "客室2-3" || d["IPC_NAME"].ToString() == "客室2-4" || d["IPC_NAME"].ToString() == "客室3-1" || d["IPC_NAME"].ToString() == "客室3-2" || d["IPC_NAME"].ToString() == "客室3-3" || d["IPC_NAME"].ToString() == "客室3-4" || d["IPC_NAME"].ToString() == "客室4-1" || d["IPC_NAME"].ToString() == "客室4-2" || d["IPC_NAME"].ToString() == "客室4-3" || d["IPC_NAME"].ToString() == "客室4-4" || d["IPC_NAME"].ToString() == "客室5-1" || d["IPC_NAME"].ToString() == "客室5-2" || d["IPC_NAME"].ToString() == "客室5-3" || d["IPC_NAME"].ToString() == "客室5-4" || d["IPC_NAME"].ToString() == "客室6-1" || d["IPC_NAME"].ToString() == "客室6-2" || d["IPC_NAME"].ToString() == "客室6-3" || d["IPC_NAME"].ToString() == "客室6-4")
                //{
                //    break;
                //}
                //else if (d["IPC_NAME"].ToString() == "行车摄像机2-1")
                //{
                //    f1 = 0;
                //    break;
                //}
               
            }
            return;


        }

       


       //// 关于四画面轮询
        //void Timer4()
        //{
            
        //    try
        //    {
        //        timerfor9.Change(Timeout.Infinite, 1000);
        //        timerfor9.Dispose();
              
        //    }
        //    catch
        //    {

        //    }
        //    //TimerCallback是指向一个object类型参数的无返回值方法的委托，供定时器回调使用
        //    TimerCallback timerCallback = new TimerCallback(Method3);//把Method1的方法加入委托中
        //    timerCallback += o => { Console.WriteLine("定时调用方法：Lambda表达式方法"); };//把lambda表达式加入委托中
        //    timerCallback += delegate(object o)//把匿名方法加入到委托中
        //    {
        //        Console.WriteLine("定时调用方法：匿名方法");
        //    };

        //    //使用定时器定时调用委托中引用的方法
           
        //        if (panel1.Controls[0].Controls.Count == 4)
        //        {
        //            while (true) { 
        //            timerfor9 = new System.Threading.Timer(timerCallback,//参数定义定时时处理的方法或方法列表（以委托实现）
        //                null,                             //此参数定义传递给方法的object类型参数，用null表明不传递参数
        //                0,                                //此参数表明定时器开始的时间，0标示理解开始
        //                (int.Parse(txtTime.Text.ToString())) * 1000);                            //此参数标示定时处理的事件间隔，以毫秒为单位
        //            break;
        //            }

        //        }

        //}

        void Method3(object o)
        {
          
            sxtClearfro49change();
            vlcClear();
          //  Thread.Sleep(100);
            layout.Dock = DockStyle.Fill;
            layout.BackColor = Color.White;
            for (int i = 0; i < panel1.Controls[0].Controls.Count; i++) {
                PictureBox pic = panel1.Controls[0].Controls[i].Controls[0] as PictureBox;
                pic.Invalidate();
            }
                Console.WriteLine("定时调用方法：Method3；执行时间：" + DateTime.Now.ToLongTimeString());
                Console.WriteLine("托管线程ID为：" + Thread.CurrentThread.ManagedThreadId);
                for (; f4 < treeView1.Nodes[0].Nodes.Count; )
                {
                    Dictionary<string, string> d = treeView1.Nodes[0].Nodes[f4].Tag as Dictionary<string, string>;
                    foreach (var l in ll)
                    {
                        if (d["IPC_NAME"].ToString() == l[4])
                        {
                            String IPC_IP = l[0];
                            String IPC_USER = l[2];
                            String IPC_PWD=l[3];
                            String IPC_PORT=l[1];
                            int i = int.Parse(l[5]);
                            String IPC_NAME = l[4];
                            PictureBox pb = panel1.Controls[0].Controls[cb++].Controls[0] as PictureBox;
                        //  if (d["IPC_NAME"].ToString() == "行车摄像机1-1" || d["IPC_NAME"].ToString() == "客室1-1" || d["IPC_NAME"].ToString() == "客室1-2" || d["IPC_NAME"].ToString() == "客室1-3" || d["IPC_NAME"].ToString() == "客室1-4" || d["IPC_NAME"].ToString() == "客室2-1" || d["IPC_NAME"].ToString() == "客室2-2" || d["IPC_NAME"].ToString() == "客室2-3" || d["IPC_NAME"].ToString() == "客室2-4" || d["IPC_NAME"].ToString() == "客室3-1" || d["IPC_NAME"].ToString() == "客室3-2" || d["IPC_NAME"].ToString() == "客室3-3" || d["IPC_NAME"].ToString() == "客室3-4" || d["IPC_NAME"].ToString() == "客室4-1" || d["IPC_NAME"].ToString() == "客室4-2" || d["IPC_NAME"].ToString() == "客室4-3" || d["IPC_NAME"].ToString() == "客室4-4" || d["IPC_NAME"].ToString() == "客室5-1" || d["IPC_NAME"].ToString() == "客室5-2" || d["IPC_NAME"].ToString() == "客室5-3" || d["IPC_NAME"].ToString() == "客室5-4" || d["IPC_NAME"].ToString() == "客室6-1" || d["IPC_NAME"].ToString() == "客室6-2" || d["IPC_NAME"].ToString() == "客室6-3" || d["IPC_NAME"].ToString() == "客室6-4" || d["IPC_NAME"].ToString() == "行车摄像机2-1")
                        // if (d["IPC_IP"].ToString() == "192.168.1.1" || d["IPC_IP"].ToString() == "192.168.1.2" || d["IPC_IP"].ToString() == "192.168.1.3" || d["IPC_IP"].ToString() == "192.168.1.4" || d["IPC_IP"].ToString() == "192.168.1.5" || d["IPC_IP"].ToString() == "192.168.1.6" || d["IPC_IP"].ToString() == "192.168.1.7" || d["IPC_IP"].ToString() == "192.168.1.8" || d["IPC_IP"].ToString() == "192.168.1.9" || d["IPC_IP"].ToString() == "192.168.1.10" || d["IPC_IP"].ToString() == "192.168.1.11" || d["IPC_IP"].ToString() == "192.168.1.12" || d["IPC_IP"].ToString() == "192.168.1.13" || d["IPC_IP"].ToString() == "192.168.1.14" || d["IPC_IP"].ToString() == "192.168.1.15" || d["IPC_IP"].ToString() == "192.168.1.16" || d["IPC_IP"].ToString() == "192.168.1.17" || d["IPC_IP"].ToString() == "192.168.1.18" || d["IPC_IP"].ToString() == "192.168.1.19" || d["IPC_IP"].ToString() == "192.168.1.20" || d["IPC_IP"].ToString() == "192.168.1.21" || d["IPC_IP"].ToString() == "192.168.1.22" || d["IPC_IP"].ToString() == "192.168.1.23" || d["IPC_IP"].ToString() == "192.168.1.24" || d["IPC_IP"].ToString() == "192.168.1.25" || d["IPC_IP"].ToString() == "192.168.1.26" || d["IPC_IP"].ToString() == "192.168.101.47" || d["IPC_IP"].ToString() == "192.168.101.48")
                        if(d["IPC_RTSP"].ToString()=="1")
                        {
                                try
                                {
                                    get_vlcplayer = (VlcPlayer)m_vlcplayerModel.GetType().GetField(string.Format("vlc_player_{0}", pb.Parent.TabIndex + 1)).GetValue(m_vlcplayerModel);
                                    if (get_vlcplayer != null)
                                    {
                                        pb.Invoke(new Action(() => {
                                            IntPtr render_wnd = pb.Handle;
                                            get_vlcplayer.SetRenderWindow((int)render_wnd);
                                            get_vlcplayer.PlayFromURL("rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mov");

                                           // get_vlcplayer.PlayFromURL("rtsp://" + IPC_USER + ":" + IPC_PWD + "@" + IPC_IP + ":" + IPC_PORT + "/h264/ch1/main/av_stream --rtsp-tcp");
                                            get_vlcplayer.SetRatio(sbl);
                                            listforVlc.Add(get_vlcplayer);
                                            EnableWindow(render_wnd, false);   
                                        }));

                                        String str = String.Format("播放的车载摄像头名称：{0},IP地址为：{1},窗口索引为{2}", IPC_NAME, IPC_IP, cb);
                                        addMsg(str);
                                        pb.Parent.MouseClick += new MouseEventHandler(RealPlayWnd_Click);
                                        pb.Parent.MouseDoubleClick += new MouseEventHandler(PictureBox_MouseDoubleClick);
                                        f4++;
                                        break;
                                    }
                                }
                                catch(Exception e)
                                {
                                    //return false;
                                    throw e;
                                }
                            }
                            else
                            {
                                try
                                {
                                    //alter by qhz
                                   // CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = CreatePreview(pb.Handle);
                                    pb.Invoke(new Action(() => { lpPreviewInfo = CreatePreview(pb.Handle); }));
                                    IntPtr pUser = new IntPtr();//用户数据
                                   // IntPtr render_wnd = pb.Handle;
                                    int error = -1;
                                    if (i < 0)
                                    {
                                        pb.Invoke(new Action(() => { error = CHCNetSDK.NET_DVR_RealPlay_V40(i, ref lpPreviewInfo, null/*RealData*/, pUser);
                                        liststr3.Add(error);
                                        })); 
                                        
                                        m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", pb.Parent.TabIndex + 1)).SetValue(m_lRealHandleModel, error);
                                        if (error < 0)
                                        {
                                            iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                                            str = string.Format("综合实验平台:摄像机:{0} 播放窗口索引{2},错误码：{3}", IPC_NAME, i, cb, iLastErr); //登录失败，输出错误号
                                            addMsg(str);
                                            Thread.Sleep(100);
                                            f4++;
                                            break;
                                        }
                                        else
                                        {
                                            str = string.Format("综合实验平台:摄像机：{0} 播放窗口索引{2}", IPC_NAME, error, cb); //登录失败，输出错误号
                                            addMsg(str);
                                            Thread.Sleep(100);
                                            f4++;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        pb.Invoke(new Action(() => { error = CHCNetSDK.NET_DVR_RealPlay_V40(i, ref lpPreviewInfo, null/*RealData*/, pUser);
                                        liststr3.Add(error);
                                        })); 
                                        m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", pb.Parent.TabIndex + 1)).SetValue(m_lRealHandleModel, error);
                                        if (error < 0)
                                        {
                                            iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                                            str = string.Format("综合实验平台:摄像机IP:{0} 播放窗口索引{2},错误码：{3}", IPC_NAME, error, cb, iLastErr); //登录失败，输出错误号
                                            addMsg(str);
                                            Thread.Sleep(100);
                                            f4++;
                                            break;
                                        }
                                        else
                                        {    //alter by qhz
                                            str = string.Format("综合实验平台:摄像机索引{0} 播放窗口索引{2}", IPC_NAME, /*i*/error, cb); //登录失败，输出错误号
                                            addMsg(str);
                                            Thread.Sleep(100);
                                            f4++;
                                            break;
                                        }

                                    }


                                }
                                catch (Exception error)
                                {
                                    //addMsg(GetExceptionMsg(error, string.Empty));
                                    WriteLog.Write(GetExceptionMsg(error, string.Empty));
                                    throw error;
                                    // return;
                                }
                            }
                        }

                    }

                //  if (d["IPC_NAME"].ToString() == "席位区-枪型摄像机1" || d["IPC_NAME"].ToString() == "席位区-枪型摄像机5" || d["IPC_NAME"].ToString() == "客室1-3" || d["IPC_NAME"].ToString() == "客室2-3" || d["IPC_NAME"].ToString() == "客室3-3" || d["IPC_NAME"].ToString() == "客室4-3" || d["IPC_NAME"].ToString() == "客室5-3" || d["IPC_NAME"].ToString() == "客室6-3")

                // if (d["IPC_NAME"].ToString() == "席位区-枪型摄像机1" || d["IPC_NAME"].ToString() == "席位区-枪型摄像机5" || d["IPC_IP"].ToString() == "192.168.1.3" || d["IPC_IP"].ToString() == "192.168.1.7" || d["IPC_IP"].ToString() == "192.168.1.11" || d["IPC_IP"].ToString() == "192.168.1.15" || d["IPC_IP"].ToString() == "192.168.1.19" || d["IPC_IP"].ToString() == "192.168.1.23")
                if(d["IPC_V4"].ToString()=="1")
                {
                        cb = 0;
                        break;
                    }
                   // else if (d["IPC_PORT"].ToString() == "11048")
                   else if(d["IPC_V4_END"].ToString()=="1")
                    {
                        cb = 0;
                        f4 = 0;
                        break;
                    }
                }
            }
        

        //九画面轮询
        //void Timer9()
        //{
        //    cb = 0;
        //    try
        //    {
        //        timerfor9.Change(Timeout.Infinite, 1000);
        //        timerfor9.Dispose();
        //    }
        //    catch
        //    {

        //    }
        //    //TimerCallback是指向一个object类型参数的无返回值方法的委托，供定时器回调使用
        //    TimerCallback timerCallback = new TimerCallback(Method2);//把Method1的方法加入委托中
        //    timerCallback += o => { Console.WriteLine("定时调用方法：Lambda表达式方法"); };//把lambda表达式加入委托中
        //    timerCallback += delegate(object o)//把匿名方法加入到委托中
        //    {
        //        Console.WriteLine("定时调用方法：匿名方法");
        //    };

        //    //使用定时器定时调用委托中引用的方法
      
        //    if (panel1.Controls[0].Controls.Count == 9)
        //    {
        //        while (true)
        //        {
        //            if(ll.Count== IPCCOUNT)
        //            {
        //            timerfor9 = new System.Threading.Timer(timerCallback,//参数定义定时时处理的方法或方法列表（以委托实现）
        //                null,                             //此参数定义传递给方法的object类型参数，用null表明不传递参数
        //                0,                                //此参数表明定时器开始的时间，0标示理解开始
        //                (int.Parse(txtTime.Text.ToString())) * 1000);                           //此参数标示定时处理的事件间隔，以毫秒为单位
        //            break;
        //                }

        //        }                          
              
        //    }
        
        //}

        void Method2(object o)
        {
            sxtClearfro49change();
            vlcClear();
            Thread.Sleep(100);
            for (int i = 0; i < panel1.Controls[0].Controls.Count; i++)
            {
                PictureBox pic = panel1.Controls[0].Controls[i].Controls[0] as PictureBox;
                pic.Invalidate();
            }
            Console.WriteLine("定时调用方法：Method2；执行时间：" + DateTime.Now.ToLongTimeString());
            Console.WriteLine("托管线程ID为：" + Thread.CurrentThread.ManagedThreadId);
            for (; f9 < treeView1.Nodes[0].Nodes.Count; )
            {
                Dictionary<string, string> d = treeView1.Nodes[0].Nodes[f9].Tag as Dictionary<string, string>;
                foreach (var l in ll)
                {
                    if (d["IPC_NAME"].ToString() == l[4])
                    {
                        String IPC_IP = l[0];
                        String IPC_USER = l[2];
                        String IPC_PWD=l[3];
                        String IPC_PORT=l[1];
                        int i = int.Parse(l[5]);
                        String IPC_NAME = l[4];
                        PictureBox pb = panel1.Controls[0].Controls[cb++].Controls[0] as PictureBox;
                        // if (d["IPC_NAME"].ToString() == "行车摄像机1-1" || d["IPC_NAME"].ToString() == "客室1-1" || d["IPC_NAME"].ToString() == "客室1-2" || d["IPC_NAME"].ToString() == "客室1-3" || d["IPC_NAME"].ToString() == "客室1-4" || d["IPC_NAME"].ToString() == "客室2-1" || d["IPC_NAME"].ToString() == "客室2-2" || d["IPC_NAME"].ToString() == "客室2-3" || d["IPC_NAME"].ToString() == "客室2-4" || d["IPC_NAME"].ToString() == "客室3-1" || d["IPC_NAME"].ToString() == "客室3-2" || d["IPC_NAME"].ToString() == "客室3-3" || d["IPC_NAME"].ToString() == "客室3-4" || d["IPC_NAME"].ToString() == "客室4-1" || d["IPC_NAME"].ToString() == "客室4-2" || d["IPC_NAME"].ToString() == "客室4-3" || d["IPC_NAME"].ToString() == "客室4-4" || d["IPC_NAME"].ToString() == "客室5-1" || d["IPC_NAME"].ToString() == "客室5-2" || d["IPC_NAME"].ToString() == "客室5-3" || d["IPC_NAME"].ToString() == "客室5-4" || d["IPC_NAME"].ToString() == "客室6-1" || d["IPC_NAME"].ToString() == "客室6-2" || d["IPC_NAME"].ToString() == "客室6-3" || d["IPC_NAME"].ToString() == "客室6-4" || d["IPC_NAME"].ToString() == "行车摄像机2-1")
                       // if (d["IPC_IP"].ToString() == "192.168.1.1" || d["IPC_IP"].ToString() == "192.168.1.2" || d["IPC_IP"].ToString() == "192.168.1.3" || d["IPC_IP"].ToString() == "192.168.1.4" || d["IPC_IP"].ToString() == "192.168.1.5" || d["IPC_IP"].ToString() == "192.168.1.6" || d["IPC_IP"].ToString() == "192.168.1.7" || d["IPC_IP"].ToString() == "192.168.1.8" || d["IPC_IP"].ToString() == "192.168.1.9" || d["IPC_IP"].ToString() == "192.168.1.10" || d["IPC_IP"].ToString() == "192.168.1.11" || d["IPC_IP"].ToString() == "192.168.1.12" || d["IPC_IP"].ToString() == "192.168.1.13" || d["IPC_IP"].ToString() == "192.168.1.14" || d["IPC_IP"].ToString() == "192.168.1.15" || d["IPC_IP"].ToString() == "192.168.1.16" || d["IPC_IP"].ToString() == "192.168.1.17" || d["IPC_IP"].ToString() == "192.168.1.18" || d["IPC_IP"].ToString() == "192.168.1.19" || d["IPC_IP"].ToString() == "192.168.1.20" || d["IPC_IP"].ToString() == "192.168.1.21" || d["IPC_IP"].ToString() == "192.168.1.22" || d["IPC_IP"].ToString() == "192.168.1.23" || d["IPC_IP"].ToString() == "192.168.1.24" || d["IPC_IP"].ToString() == "192.168.1.25" || d["IPC_IP"].ToString() == "192.168.1.26" || d["IPC_IP"].ToString() == "192.168.101.47" || d["IPC_IP"].ToString() == "192.168.101.48")
                       if(d["IPC_RTSP"].ToString()=="1")
                        {
                            try
                            {
                                get_vlcplayer = (VlcPlayer)m_vlcplayerModel.GetType().GetField(string.Format("vlc_player_{0}", pb.Parent.TabIndex + 1)).GetValue(m_vlcplayerModel);
                                if (get_vlcplayer != null)
                                {
                                    pb.Invoke(new Action(() => {

                                        IntPtr render_wnd = pb.Handle;
                                        get_vlcplayer.SetRenderWindow((int)render_wnd);
                                        get_vlcplayer.SetRatio(sbl);
                                        get_vlcplayer.PlayFromURL("rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mov");

                                       // get_vlcplayer.PlayFromURL("rtsp://" + IPC_USER + ":" + IPC_PWD + "@" + IPC_IP + ":" + IPC_PORT + "/h264/ch1/main/av_stream --rtsp-tcp");
                                        listforVlc.Add(get_vlcplayer);
                                        EnableWindow(render_wnd, false);
                                    
                                    }));
                                    String str = String.Format("播放的车载摄像头名称：{0},IP地址为：{1},窗口索引为{2}", IPC_NAME, IPC_IP,cb);
                                    addMsg(str);
                                    pb.Parent.MouseClick += new MouseEventHandler(RealPlayWnd_Click);
                                    pb.Parent.MouseDoubleClick += new MouseEventHandler(PictureBox_MouseDoubleClick);
                                    f9++;
                                    break;
                                }
                              
                            }
                            catch
                            {
                                //return false;
                            }
                        }
                        else
                        {
                            try
                            {
                                //alter by qhz
                              //  CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = CreatePreview(pb.Handle);
                                pb.Invoke(new Action(() => { lpPreviewInfo = CreatePreview(pb.Handle); }));
                                IntPtr pUser = new IntPtr();//用户数据
                              // IntPtr render_wnd = pb.Handle;
                                Dictionary<string, string> dd = pb.Tag as Dictionary<string, string>;
                                int error = -1;
                                //int getm_lRealHandle = (int)m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", 1)).GetValue(m_lRealHandleModel);//根据所选监控窗口，获取对应的返回索引值
                                if (i < 0)
                                {
                                    pb.Invoke(new Action(() => { error = CHCNetSDK.NET_DVR_RealPlay_V40(i, ref lpPreviewInfo, null/*RealData*/, pUser);
                                    liststr3.Add(error);
                                    
                                    }));
                                   
                                    m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", pb.Parent.TabIndex + 1)).SetValue(m_lRealHandleModel, error);
                                    if (error < 0)
                                    {
                                        iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                                        str = string.Format("综合实验平台:摄像机:{0} 播放窗口索引{2},错误码：{3}", IPC_NAME, i, cb, iLastErr); //登录失败，输出错误号
                                        addMsg(str);
                                        Thread.Sleep(100);
                                        f9++;
                                        break;
                                    }
                                    else
                                    {
                                        str = string.Format("综合实验平台:摄像机：{0} 播放窗口索引{2}", IPC_NAME, error, cb); //登录失败，输出错误号
                                        addMsg(str);
                                        Thread.Sleep(100);
                                        f9++;
                                        break;
                                    }
                                }
                                else
                                {
                                    pb.Invoke(new Action(() => { error = CHCNetSDK.NET_DVR_RealPlay_V40(i, ref lpPreviewInfo, null/*RealData*/, pUser);
                                    liststr3.Add(error);
                                    }));
                                   
                                    m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", pb.Parent.TabIndex + 1)).SetValue(m_lRealHandleModel, error);
                                    if (error < 0)
                                    {
                                        iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                                        str = string.Format("综合实验平台:摄像机IP:{0} 播放窗口索引{2},错误码：{3}", IPC_NAME, error, cb, iLastErr); //登录失败，输出错误号
                                        addMsg(str);
                                        Thread.Sleep(100);
                                        f9++;
                                        break;
                                    }
                                    else
                                    {    //alter by qhz
                                        str = string.Format("综合实验平台:摄像机索引{0} 播放窗口索引{2}", IPC_NAME, /*i*/error, cb); //登录失败，输出错误号
                                        addMsg(str);
                                        Thread.Sleep(100);
                                        f9++;
                                        break;
                                    }

                                }


                            }
                            catch (Exception error)
                            {
                                WriteLog.Write(GetExceptionMsg(error, string.Empty));
                                throw error;
                            }
                        }
                    }
                }
              //  if (d["IPC_PORT"].ToString()=="11025"||d["IPC_PORT"].ToString()== "11009" || d["IPC_PORT"].ToString()== "11018")
              if(d["IPC_V9"].ToString()=="1")
                {
                    cb = 0;
                    break;
                }
              //  else if (d["IPC_PORT"].ToString() == "11048")
              else if(d["IPC_V9_END"].ToString()=="1")
                {
                    cb = 0;
                    f9 = 0;
                    break;
                }
            }

            return;
        }
   //摄像头在线状态显示
     void TimerEx()
        {
            //TimerCallback是指向一个object类型参数的无返回值方法的委托，供定时器回调使用
            TimerCallback timerCallback = new TimerCallback(Method1);//把Method1的方法加入委托中
            timerCallback += o => { Console.WriteLine("定时调用方法：Lambda表达式方法"); };//把lambda表达式加入委托中
            timerCallback += delegate(object o)//把匿名方法加入到委托中
            {
                Console.WriteLine("定时调用方法：匿名方法");
            };

            //使用定时器定时调用委托中引用的方法
            timerforico = new System.Threading.Timer(timerCallback,//参数定义定时时处理的方法或方法列表（以委托实现）
                null,                             //此参数定义传递给方法的object类型参数，用null表明不传递参数
                0,                                //此参数表明定时器开始的时间，0标示理解开始
                1000);                            //此参数标示定时处理的事件间隔，以毫秒为单位
            

            
        }
        //定义定时处理的方法
        void Method1(object o)
        {
            Console.WriteLine("定时调用方法：Method1；执行时间：" + DateTime.Now.ToLongTimeString());
            Console.WriteLine("托管线程ID为：" + Thread.CurrentThread.ManagedThreadId);
                for (int i = 0; i < treeView1.Nodes[0].GetNodeCount(false); i++)
                {
                   
                    foreach (TreeNode tn in treeView1.Nodes[0].Nodes[i].Nodes)
                    {
                    ThreadPool.QueueUserWorkItem(
                             new WaitCallback(obj =>
                             {
                                 Dictionary<string, string> d = tn.Tag as Dictionary<string, string>;
                                 String IPC_IP = d["IPC_IP"].ToString();
                                 String IPC_USER = d["IPC_USER"].ToString();
                                 int IPC_PORT = int.Parse(d["IPC_PORT"].ToString());

                                 try
                                 {
                                     Ping objPingSender = new Ping();
                                     PingOptions objPinOptions = new PingOptions();
                                     objPinOptions.DontFragment = true;
                                     string data = "";
                                     byte[] buffer = Encoding.UTF8.GetBytes(data);
                                     int intTimeout = 1000;
                                     PingReply objPinReply = objPingSender.Send(IPC_IP, intTimeout, buffer, objPinOptions);
                                     string strInfo = objPinReply.Status.ToString();
                                     if (strInfo == "Success")
                                     {
                                         treeView1.Invoke(new Action(() =>
                                         {
                                             tn.ImageIndex = 1;
                                             tn.SelectedImageIndex = 1;
                                         }));

                                     }
                                     else
                                     {
                                         treeView1.Invoke(new Action(() =>
                                         {
                                             tn.ImageIndex = 0;
                                             tn.SelectedImageIndex = 0;
                                         }));

                                     }
                                 }
                                 catch (Exception err)
                                 {
                                     throw err;
                                 }
                             }));
                }
            }
        }

       

        private void textBox1_TextChanged(object sender, System.EventArgs e)
        {

        }
        //private void timecallback()
        //{
        //    fiveM = DateTime.Parse("00:00:10");
        //    label1.Text = fiveM.Hour.ToString("00") + ":" + fiveM.Minute.ToString("00") + ":" + fiveM.Second.ToString("00");
        //}
        private void btnLogin_Click(object sender, System.EventArgs e)
        {

            if (m_lUserID < 0)
            {
                string DVRIPAddress = ""; //textBoxIP.Text; //设备IP地址或者域名
                Int16 DVRPortNumber = 8000; //Int16.Parse(textBoxPort.Text);//设备服务端口号
                string DVRUserName = ""; //textBoxUserName.Text;//设备登录用户名
                string DVRPassword = ""; //textBoxPassword.Text;//设备登录密码

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
                    //btnLogin.Text = "Logout";
                }

            }
            else
            {
                //注销登录 Logout the device
                if (m_lRealHandle >= 0)
                {
                    MessageBox.Show("Please stop live view firstly");
                    return;
                }

                if (!CHCNetSDK.NET_DVR_Logout(m_lUserID))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_Logout failed, error code= " + iLastErr;
                    MessageBox.Show(str);
                    return;
                }
                m_lUserID = -1;
                //btnLogin.Text = "Login";
            }
            return;
        }

        private CHCNetSDK.NET_DVR_PREVIEWINFO CreatePreview(System.IntPtr Handle)
        {
            CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO();
            lpPreviewInfo.hPlayWnd = Handle;//RealPlayWnd.Handle;//预览窗口
            lpPreviewInfo.lChannel = 1;//预te览的设备通道
            lpPreviewInfo.dwStreamType = 1;//码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
            lpPreviewInfo.dwLinkMode = 0;//连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
            lpPreviewInfo.bBlocked = true; //0- 非阻塞取流，1- 阻塞取流
            lpPreviewInfo.dwDisplayBufNum = 15; //播放库播放缓冲区最大缓冲帧数
            return lpPreviewInfo;

        }



        public void RealDataCallBack(Int32 lRealHandle, UInt32 dwDataType, ref byte pBuffer, UInt32 dwBufSize, IntPtr pUser)
        {
        }

        private void btnBMP_Click(object sender, EventArgs e)
        {
            string sBmpPicFileName;
            //图片保存路径和文件名 the path and file name to save
            sBmpPicFileName = "BMP_test.bmp";

            //BMP抓图 Capture a BMP picture
            if (!CHCNetSDK.NET_DVR_CapturePicture(m_lRealHandle, sBmpPicFileName))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                str = "NET_DVR_CapturePicture failed, error code= " + iLastErr;
                MessageBox.Show(str);
                return;
            }
            else
            {
                str = "Successful to capture the BMP file and the saved file is " + sBmpPicFileName;
                MessageBox.Show(str);
            }
            return;
        }

        private void btnJPEG_Click(object sender, EventArgs e)
        {
            string sJpegPicFileName;
            //图片保存路径和文件名 the path and file name to save
            sJpegPicFileName = "JPEG_test.jpg";
            int lChannel = 1; //通道号 Channel number
            CHCNetSDK.NET_DVR_JPEGPARA lpJpegPara = new CHCNetSDK.NET_DVR_JPEGPARA();
            lpJpegPara.wPicQuality = 0; //图像质量 Image quality
            lpJpegPara.wPicSize = 0xff; //抓图分辨率 Picture size: 2- 4CIF，0xff- Auto(使用当前码流分辨率)，抓图分辨率需要设备支持，更多取值请参考SDK文档

            //JPEG抓图 Capture a JPEG picture
            if (!CHCNetSDK.NET_DVR_CaptureJPEGPicture(m_lUserID, lChannel, ref lpJpegPara, sJpegPicFileName))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                str = "NET_DVR_CaptureJPEGPicture failed, error code= " + iLastErr;
                MessageBox.Show(str);
                return;
            }
            else
            {
                str = "Successful to capture the JPEG file and the saved file is " + sJpegPicFileName;
                MessageBox.Show(str);
            }
            return;
        }
        private void btn_Exit_Click(object sender, EventArgs e)
        {
            //停止预览 Stop live view 
            if (m_lRealHandle >= 0)
            {
                CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle);
                m_lRealHandle = -1;
            }

            //注销登录 Logout the device
            if (m_lUserID >= 0)
            {
                CHCNetSDK.NET_DVR_Logout(m_lUserID);
                m_lUserID = -1;
            }

            CHCNetSDK.NET_DVR_Cleanup();
            Application.Exit();
        }
        PictureBox now = null;
        /// <summary>
        /// 获取当前窗体   添加边框效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RealPlayWnd_Click(object sender, MouseEventArgs e)
        {

            PictureBox p = sender as PictureBox;
            for (int i = 0; i < panel1.Controls[0].Controls.Count; i++)
            {
                PictureBox ppp = panel1.Controls[0].Controls[i] as PictureBox;
                ppp.BackColor = Color.Black;
            }
            now = p;
            if (p.Parent == panel1.Controls[0])
            {
                p.BackColor = Color.Red;
                foreach (Control c in panel1.Controls[0].Controls)
                {
                    PictureBox pp = c as PictureBox;
                    if (p != pp)
                    {
                        pp.BackColor = System.Drawing.Color.Black;
                    }

                }


            }
            else
            {
                p.Parent.BackColor = System.Drawing.Color.Red;

                foreach (Control c in panel1.Controls[0].Controls)
                {
                    foreach (Control c1 in c.Controls)
                    {
                        PictureBox p1 = c1 as PictureBox;
                        if (p.Parent != p1)
                        {
                            p1.BackColor = System.Drawing.Color.Black;

                        }
                    }
                }


            }

        }

        private CancellationTokenSource cancellationTokenSource
            = new CancellationTokenSource();
        private void Preview_Load(object sender, EventArgs e)
        {  
            //将系统对跨线程调用的安全性检查关掉
           // Control.CheckForIllegalCrossThreadCalls = false;
            String sus = null;
            try {

                 sus = Init();
            }
            catch (Exception error){

                throw error;
            }
           
            treeView1.ImageList = imageList1;
            foreach (TreeNode tn in treeView1.Nodes[0].Nodes)
            {
                tn.ImageIndex = 4;
                tn.SelectedImageIndex = 4;
            }
            treeView1.Nodes[0].ImageIndex = 2;
            treeView1.Nodes[0].SelectedImageIndex = 2;
            while (true)
            {
                if (sus == "success")
                {
                    TimerEx();
                   // timer4.Interval = (int.Parse(txtTime.Text.ToString()) + 1) * 1000;
                   // timer4.Start();
                }
                break;
            }
            //创建UDP接收方 循环监听发送方发来的消息
            //Server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //Server.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6001));//绑定端口号和IP
            //Console.WriteLine("服务端已经开启");
            //Thread t = new Thread(ReciveMsg);//开启接收消息线程
            //t.Start();
            //暂时关闭分屏
             // this.BeginInvoke(new Action(()=>Timerfp()));
        }
        PictureBox pz;
        /// <summary>
        /// 初始化
        /// </summary>
        private String Init()
        {
           // button13.Enabled = false;
            CreatePictureBox(1);
            
           // bindcmbYuzhi();
            getTree();
          //  StopPolling(false); //启动轮询
           // cmbMode.SelectedIndex = 0;
           // cmbSpeed.SelectedIndex = 6;
           showonmonitorfor2(); 
            return "success";

        }


        /// <summary>
        /// 生成PictureBox
        /// </summary>
        /// <param name="PictureBoxs">需要生成的个数</param>
      FlowLayoutPanel layout;
        private void CreatePictureBox(int PictureBoxs)
        {
          
            panel1.Controls.Clear();
            layout = new FlowLayoutPanel();
            layout.Dock = DockStyle.Fill;
            panel1.Controls.Add(layout);
            for (int i = 0; i < PictureBoxs; i++)
            {
                PictureBox pb = new PictureBox();
                pb.BackColor = System.Drawing.Color.Black;
                pb.Width = panel1.Width / (PictureBoxs == 9 ? 3 : (PictureBoxs == 4 ? 2 : 1)) - 7;
                pb.Height = panel1.Height / (PictureBoxs == 9 ? 3 : (PictureBoxs == 4 ? 2 : 1)) - 7;
                
                pb.Name = string.Format("PictureBox{0}", PictureBoxs);
                pb.MouseClick += new MouseEventHandler(RealPlayWnd_Click);
                pb.MouseDoubleClick += new MouseEventHandler(PictureBox_MouseDoubleClick);
                panel1.Controls[0].Controls.Add(pb);
            }
            for (int i = 0; i < PictureBoxs; i++)
            {
                PictureBox pb = new PictureBox();
                pb.BackColor = System.Drawing.Color.Black;
                pb.Width = panel1.Width / (PictureBoxs == 9 ? 3 : (PictureBoxs == 4 ? 2 : 1)) - 9;
                pb.Height = panel1.Height / (PictureBoxs == 9 ? 3 : (PictureBoxs == 4 ? 2 : 1)) - 9;
                pb.Name = string.Format("PictureBox{0}", PictureBoxs);
                sbl = pb.Width + ":" + pb.Height; 
                pb.Top += 1;
                pb.Left += 1;
                pb.MouseClick += new MouseEventHandler(RealPlayWnd_Click);
                pb.MouseDoubleClick += new MouseEventHandler(PictureBox_MouseDoubleClick);
                panel1.Controls[0].Controls[i].Controls.Add(pb);
            }


           

        }

        //分屏
        private string sbl2;

        private void createPicBox(int PictureBoxs)
        {
            frm2.Controls.Clear();
            FlowLayoutPanel  layout = new FlowLayoutPanel();
            layout.Dock = DockStyle.Fill;
            for (int i = 0; i < PictureBoxs; i++)
            {
                PictureBox pb = new PictureBox();
                pb.BackColor = System.Drawing.Color.Black;
                pb.Width = (frm2.Width - 20) / (PictureBoxs == 9 ? 3 : (PictureBoxs == 4 ? 2 : 1)) - 7;
                pb.Height = (frm2.Height - 8) / (PictureBoxs == 9 ? 3 : (PictureBoxs == 4 ? 2 : 1)) - 7;
                sbl2 = pb.Width + ":" + pb.Height; 

                pb.Name = string.Format("PictureBox{0}", PictureBoxs);
                layout.Controls.Add(pb);

            }
            frm2.Controls.Add(layout);
        }
        
       
        /// <summary>
        /// 关闭所有监控连接
        /// </summary>
        private void shutdownAll()
        {
            try
            {
                for (int i = 0; i < 13; i++)
                {
                    int getm_lRealHandle = (int)m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", i + 1)).GetValue(m_lRealHandleModel);//根据所选监控窗口，获取对应的返回索引值
                    if (getm_lRealHandle >= 0)
                        CHCNetSDK.NET_DVR_StopRealPlay(getm_lRealHandle);
                }
            }

            catch (Exception error)
            {
                //addMsg(GetExceptionMsg(error, string.Empty));
                WriteLog.Write(GetExceptionMsg(error, string.Empty));
                //throw error;
                return;
            }
        }
        int yt = -1;
        int y = -1;
        int jj = -1;
        private void PictureBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                timerfor9.Change(Timeout.Infinite, 1000);
                timerfor9.Dispose();
            }
            catch
            {

            }
            PictureBox bb = sender as PictureBox;
           // StopPolling(false);
           // button13.Text = "启动轮询";
            yt = -1;
            y = -1;
            jj = -1;
            switch (panel1.Controls[0].Controls.Count)
            {
                //case 1:
                //    this.Invoke(new Action(() => panel1.Controls.Clear()));
                //    this.Invoke(new Action(() => CreatePictureBox(9)));
                //    int gf = panel1.Controls[0].Controls.Count;
                //    cb = 0;
                //    yt = -1;
                //    if (panel1.Controls[0].Controls.Count == 9)
                //    {
                //        if (yt >= -10)
                //        {
                //            sxtClearfor1change();
                //            sxtClearfro49change();
                //            vlcClear();
                //            //先判断播放第一个m_iuserid>=0的设备，从第一个m_iuserid大于0的设备开始，进行播放。
                //            for (int td = 0; td < treeView1.Nodes[0].Nodes.Count; td++)
                //            {
                //                for (int h = 0; h < ll.Count; h++)
                //                {
                //                    String s = ll[h][4];
                //                    String sip = ll[h][0];
                //                    string RTSP = ll[h][6];
                //                    Dictionary<string, string> d = treeView1.Nodes[0].Nodes[td].Tag as Dictionary<string, string>;
                //                    String IPC_IP = d["IPC_IP"].ToString();
                //                    String IPC_PORT = d["IPC_PORT"].ToString();
                //                    String IPC_USER = d["IPC_USER"].ToString();
                //                    String IPC_PWD = d["IPC_PWD"].ToString();
                //                    String IPC_NAME = d["IPC_NAME"].ToString();
                //                    String IPC_RTSP = d["IPC_RTSP"].ToString();
                //                    if (s == d["IPC_NAME"].ToString())
                //                    {
                //                        int m_iuserid = int.Parse(ll[h][5]);
                //                        if (m_iuserid < 0 && yt == -1)
                //                        {
                //                            break;
                //                        }
                //                        else
                //                        {

                //                            //  if (s == "行车摄像机1-1" || s == "客室1-1" || s == "客室1-2" || s == "客室1-3" || s == "客室1-4" || s == "客室2-1" || s == "客室2-2" || s == "客室2-3" || s == "客室2-4" || s == "客室3-1" || s == "客室3-2" || s == "客室3-3" || s == "客室3-4" || s == "客室4-1" || s == "客室4-2" || s == "客室4-3" || s == "客室4-4" || s == "客室5-1" || s == "客室5-2" || s == "客室5-3" || s == "客室5-4" || s == "客室6-1" || s == "客室6-2" || s == "客室6-3" || s == "客室6-4" || s == "行车摄像机2-1")
                //                           // if (sip == "192.168.1.1" || sip == "192.168.1.2" || sip == "192.168.1.3" || sip == "192.168.1.4" || sip == "192.168.1.5" || sip == "192.168.1.6" || sip == "192.168.1.7" || sip == "192.168.1.8" || sip == "192.168.1.9" || sip == "192.168.1.10" || sip == "192.168.1.11" || sip == "192.168.1.12" || sip == "192.168.1.13" || sip == "192.168.1.14" || sip == "192.168.1.15" || sip == "192.168.1.16" || sip == "192.168.1.17" || sip == "192.168.1.18" || sip == "192.168.1.19" || sip == "192.168.1.20" || sip == "192.168.1.21" || sip == "192.168.1.22" || sip == "192.168.1.23" || sip == "192.168.1.24" || sip == "192.168.1.25" || sip == "192.168.1.26" || sip == "192.168.101.47" || sip == "192.168.101.48")
                //                            if(RTSP=="1")
                //                            {
                //                                lxRtspfor49(ll[h][0], s, ll[h][3], ll[h][1], ll[h][2]);
                //                                String str1 = string.Format("播放的车载摄像头的名称为:{0};窗口索引为{1}", d["IPC_NAME"].ToString(), cb);
                //                                addMsg(str1);
                //                                yt--;
                //                                break;
                //                            }
                //                            else
                //                            {   
                //                               // int miuserid = ConnectCamera(IPC_IP,IPC_PORT,IPC_USER,IPC_PWD);
                //                                lunxunChange(m_iuserid,ll[h][0],ll[h][4]);
                //                                String str = string.Format("播放的摄像头的的名称为:{0},窗口索引为{1}", d["IPC_NAME"].ToString(), cb);
                //                                addMsg(str);
                //                                yt--;
                //                                break;
                //                            }
                //                        }
                //                    }
                //                }
                //                if (yt == -10)
                //                {
                                    
                //                    break;
                //                }
                //            }

                //        }
                //        cb = 0;
                //        yt = -1;
                //        return;
                //    }

                //    break;
                case 4:
                    this.Invoke(new Action(() => CreatePictureBox(1)));
                    cb = 0;
                    jj = -1;
                    if (panel1.Controls[0].Controls.Count == 1)
                    {
                        if (jj == -1)
                        {
                            sxtClearfor1change();
                            sxtClearfro49change();
                            vlcClear();
                            //先判断播放m_iuserid是大于0的设备信息，从第一个m_iuserid大于0的设备开始，进行轮询播放
                            for (int zz = 0; zz < treeView1.Nodes[0].Nodes.Count; zz++)
                            {
                                for (int h = 0; h < ll.Count; h++)
                                {
                                    String s = ll[h][4];
                                    String sip = ll[h][0];
                                    String RTSP = ll[h][6];
                                    Dictionary<string, string> d = treeView1.Nodes[0].Nodes[zz].Tag as Dictionary<string, string>;
                                    String IPC_IP = d["IPC_IP"].ToString();
                                    String IPC_PORT = d["IPC_PORT"].ToString();
                                    String IPC_USER = d["IPC_USER"].ToString();
                                    String IPC_PWD = d["IPC_PWD"].ToString();
                                    String IPC_NAME = d["IPC_NAME"].ToString();
                                    if (s == d["IPC_NAME"].ToString())
                                    {
                                        int m_iuserid = int.Parse(ll[h][5]);
                                        if (m_iuserid < 0 && jj == -1)
                                        {

                                            break;
                                        }
                                        else
                                        {
                                            //  if (s == "行车摄像机1-1" || s == "客室1-1" || s == "客室1-2" || s == "客室1-3" || s == "客室1-4" || s == "客室2-1" || s == "客室2-2" || s == "客室2-3" || s == "客室2-4" || s == "客室3-1" || s == "客室3-2" || s == "客室3-3" || s == "客室3-4" || s == "客室4-1" || s == "客室4-2" || s == "客室4-3" || s == "客室4-4" || s == "客室5-1" || s == "客室5-2" || s == "客室5-3" || s == "客室5-4" || s == "客室6-1" || s == "客室6-2" || s == "客室6-3" || s == "客室6-4" || s == "行车摄像机2-1")
                                            //  if (sip == "192.168.1.1" || sip == "192.168.1.2" || sip == "192.168.1.3" || sip == "192.168.1.4" || sip == "192.168.1.5" || sip == "192.168.1.6" || sip == "192.168.1.7" || sip == "192.168.1.8" || sip == "192.168.1.9" || sip == "192.168.1.10" || sip == "192.168.1.11" || sip == "192.168.1.12" || sip == "192.168.1.13" || sip == "192.168.1.14" || sip == "192.168.1.15" || sip == "192.168.1.16" || sip == "192.168.1.17" || sip == "192.168.1.18" || sip == "192.168.1.19" || sip == "192.168.1.20" || sip == "192.168.1.21" || sip == "192.168.1.22" || sip == "192.168.1.23" || sip == "192.168.1.24" || sip == "192.168.1.25" || sip == "192.168.1.26" || sip == "192.168.101.47" || sip == "192.168.101.48")
                                            if(RTSP=="1")
                                            {
                                                lxRtspfor49(ll[h][0], s, ll[h][3], ll[h][1], ll[h][2]);
                                                String str1 = string.Format("播放的车载摄像头的名称为:{0};窗口索引为{1}", d["IPC_NAME"].ToString(), cb);
                                                addMsg(str1);
                                                jj--;
                                                break;
                                            }
                                            else
                                            {
                                                lunxunChange(m_iuserid, ll[h][0], ll[h][4]);
                                                String str = string.Format("播放的摄像头的的名称为:{0},窗口索引为{1}", d["IPC_NAME"].ToString(), cb);
                                                addMsg(str);
                                                jj--;
                                                break;
                                            }
                                        }

                                    }
                                }
                                if (jj == -2)
                                {

                                    break;
                                }
                            }

                        }

                        jj = -1;
                        cb = 0;
                        return;

                    }
                    break;
                case 1:
                    this.Invoke(new Action(() => CreatePictureBox(4)));
                    cb = 0;
                    y = -1;
                    if (panel1.Controls[0].Controls.Count == 4)
                    {
                        
                        if (y >= -5)
                        {
                            
                            sxtClearfor1change();
                            sxtClearfro49change();
                            vlcClear();
                            //先判断播放第一个m_iuserid>=0的设备，从第一个m_iuserid大于0的设备开始，进行轮询。
                            for (int td = 0; td < treeView1.Nodes[0].Nodes.Count; td++)
                            {
                                for (int h = 0; h < ll.Count; h++)
                                {
                                    String s = ll[h][4];
                                    String sip = ll[h][0];
                                    String RTSP = ll[h][6];
                                    Dictionary<string, string> d = treeView1.Nodes[0].Nodes[td].Tag as Dictionary<string, string>;
                                    String IPC_IP = d["IPC_IP"].ToString();
                                    String IPC_PORT = d["IPC_PORT"].ToString();
                                    String IPC_USER = d["IPC_USER"].ToString();
                                    String IPC_PWD = d["IPC_PWD"].ToString();
                                    String IPC_NAME = d["IPC_NAME"].ToString();
                                    if (s == d["IPC_NAME"].ToString())
                                    {
                                        int m_iuserid = int.Parse(ll[h][5]);
                                        if (m_iuserid < 0 && y == -1)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            //  if (sip == "192.168.1.1" || sip == "192.168.1.2" || sip == "192.168.1.3" || sip == "192.168.1.4" || sip == "192.168.1.5" || sip == "192.168.1.6" || sip == "192.168.1.7" || sip == "192.168.1.8" || sip == "192.168.1.9" || sip == "192.168.1.10" || sip == "192.168.1.11" || sip == "192.168.1.12" || sip == "192.168.1.13" || sip == "192.168.1.14" || sip == "192.168.1.15" || sip == "192.168.1.16" || sip == "192.168.1.17" || sip == "192.168.1.18" || sip == "192.168.1.19" || sip == "192.168.1.20" || sip == "192.168.1.21" || sip == "192.168.1.22" || sip == "192.168.1.23" || sip == "192.168.1.24" || sip == "192.168.1.25" || sip == "192.168.1.26" || sip == "192.168.101.47" || sip == "192.168.101.48")
                                            if(RTSP=="1")
                                            {
                                                lxRtspfor49(ll[h][0], s, ll[h][3], ll[h][1], ll[h][2]);
                                                String str1 = string.Format("播放的车载摄像头的名称为:{0};窗口索引为{1}", d["IPC_NAME"].ToString(), cb);
                                                addMsg(str1);
                                                y--;
                                                break;
                                            }
                                            else
                                            {
                                                lunxunChange(m_iuserid, ll[h][0], ll[h][4]);
                                                String str = string.Format("播放的摄像头的的名称为:{0},窗口索引为{1}", d["IPC_NAME"].ToString(), cb);
                                                addMsg(str);
                                                y--;
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (y == -5)
                                {

                                    break;
                                }
                            }

                        }
                        y = -1;
                        cb = 0;
                        return;
                    }

                    break;
            }

        }
        /// <summary>
        /// 获取设备列表
        /// </summary>
        private void getTree()
        {
            List<Dictionary<string, string>> listDict = new List<Dictionary<string, string>>();
            List<string> sections = IniFile.ReadSections();
            foreach (string section in sections)
            {
                if (section.Contains("IPC"))
                {
                    string IPC_NAME = IniFile.ReadIniData(section, "IPC_NAME", "");
                    string IPC_IP = IniFile.ReadIniData(section, "IPC_IP", "");
                    string IPC_PORT = IniFile.ReadIniData(section, "IPC_PORT", "");
                    string IPC_USER = IniFile.ReadIniData(section, "IPC_USER", "");
                    string IPC_PWD = IniFile.ReadIniData(section, "IPC_PWD", "");
                    string IPC_RTSP = IniFile.ReadIniData(section, "IPC_RTSP", "");
                    string IPC_V4 = IniFile.ReadIniData(section, "IPC_V4", "");
                    string IPC_JS = IniFile.ReadIniData(section, "IPC_JS", "");
                    string IPC_V4_END = IniFile.ReadIniData(section, "IPC_V4_END", "");
                    string IPC_V9_END = IniFile.ReadIniData(section, "IPC_V9_END", "");
                    string IPC_V9 = IniFile.ReadIniData(section, "IPC_V9", "");
                    string IPC_V1_END = IniFile.ReadIniData(section,"IPC_V1_END","");
                    string IPC_NODE = IniFile.ReadIniData(section, "IPC_NODE", "");
                    string IPC_PARENT = IniFile.ReadIniData(section, "IPC_PARENT", "");
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    dict.Add("IPC_NAME", IPC_NAME);
                    dict.Add("IPC_IP", IPC_IP);
                    dict.Add("IPC_PORT", IPC_PORT);
                    dict.Add("IPC_USER", IPC_USER);
                    dict.Add("IPC_PWD", IPC_PWD);
                    dict.Add("IPC_RTSP", IPC_RTSP);
                    dict.Add("IPC_V4", IPC_V4);
                    dict.Add("IPC_JS", IPC_JS);
                    dict.Add("IPC_V4_END", IPC_V4_END);
                    dict.Add("IPC_V9_END", IPC_V9_END);
                    dict.Add("IPC_V9", IPC_V9);
                    dict.Add("IPC_V1_END",IPC_V1_END);
                    dict.Add("IPC_NODE",IPC_NODE);
                    dict.Add("IPC_PARENT", IPC_PARENT);
                    listDict.Add(dict);


                }
                else if(section.Contains("CONFIG")) {

                    IPCCOUNT=int.Parse(IniFile.ReadIniData(section, "IPCCOUNT",""));
                    addMsg(IPCCOUNT+"IPC摄像头的数量");
                }
            }
            TreeNode t1 = new TreeNode("视频监控系统");
          
           
            foreach (Dictionary<string, string> dict in listDict)
            {
            
                string IPC_NAME;
                string IPC_NODE;
                dict.TryGetValue("IPC_NAME", out IPC_NAME);
                dict.TryGetValue("IPC_NODE", out IPC_NODE);
                if (IPC_NODE != null && !IPC_NODE.Equals("0"))
                {
                    TreeNode t2 = new TreeNode();
                    t2.Text = IPC_NAME;
                    t2.Tag = dict;
                    
                    foreach (Dictionary<string, string> dict2 in listDict)
                    {
                        string IPC_NAME2;
                        string IPC_NODE2;
                        string IPC_PARENT2;
                        dict2.TryGetValue("IPC_NAME", out IPC_NAME2);
                        dict2.TryGetValue("IPC_NODE", out IPC_NODE2);
                        dict2.TryGetValue("IPC_PARENT",out IPC_PARENT2);
                        if (IPC_NODE2.Equals("0") && IPC_PARENT2.Equals(IPC_NODE)) {
                         TreeNode t3 =  new TreeNode();
                         t3.Text = IPC_NAME2;
                         t3.Tag = dict2;
                         t2.Nodes.Add(t3);

                        
                        }
                   
                   }
                    t1.Nodes.Add(t2);
                }
                
                
               
            }
            treeView1.Nodes.Add(t1);
            if (treeView1.Nodes.Count > 0)
                treeView1.Nodes[0].Expand();
            treeView1.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(treeView1_NodeMouseDoubleClick);
        }

        /// <summary>
        /// 点击左侧选中的将摄像头 将信息赋给右侧已经选中的PictureBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            //判断当前画面是否正在轮询
         //if(button13.Text.ToString()== "停止轮询"){
         //    addMsg("请先停止轮询");
         //    return;  
         //}
            if (e.Node.Text.ToString() != "综合实验平台")
            {
                if (now != null)
                {
                    now.MouseDoubleClick += new MouseEventHandler(PictureBox_MouseDoubleClick);
                    /**********************************海康摄像头视频播放*******************************************/
                    Dictionary<string, string> dict = e.Node.Tag as Dictionary<string, string>;
                    string IPC_NAME;
                    string IPC_IP;
                    string IPC_PORT;
                    string IPC_USER;
                    string IPC_PWD;
                    string IPC_RTSP;
                    dict.TryGetValue("IPC_NAME", out IPC_NAME);
                    dict.TryGetValue("IPC_IP", out IPC_IP);
                    dict.TryGetValue("IPC_PORT", out IPC_PORT);
                    dict.TryGetValue("IPC_USER", out IPC_USER);
                    dict.TryGetValue("IPC_PWD", out IPC_PWD);
                    dict.TryGetValue("IPC_RTSP",out IPC_RTSP);
                 //   get_vlcplayer = (VlcPlayer)m_vlcplayerModel.GetType().GetField(string.Format("vlc_player_{0}", now.TabIndex + 1)).GetValue(m_vlcplayerModel);
                    // if (IPC_NAME == "中心区-枪型摄像机1" || IPC_NAME == "中心区-枪型摄像机2" || IPC_NAME == "车站区-球形摄像机" || IPC_NAME == "席位区-枪型摄像机1" || IPC_NAME == "席位区-枪型摄像机2" || IPC_NAME == "席位区-枪型摄像机3" || IPC_NAME == "席位区-枪型摄像机4" || IPC_NAME == "席位区-枪型摄像机5") { 
                    if (IPC_RTSP=="0") { 
                    if (IPC_IP != "" && IPC_PORT != "" && IPC_USER != "" & IPC_PWD != "")
                    {

                        if (ConnectCamera(IPC_IP, IPC_PORT, IPC_USER, IPC_PWD) >= 0)
                        {
                            msgCount++; now.Tag = dict;
                            PlayCamera(now);
                               // now = null;
                        }
                        else
                        {
                            addMsg(string.Format("综合实验平台_{0}_播放失败，请检查配置文件是否有错", IPC_NAME));
                        }
                    }
                    }
                    else
                    {

                        // if (IPC_NAME == "行车摄像机1-1" || IPC_NAME == "客室1-1" || IPC_NAME == "客室1-2" || IPC_NAME == "客室1-3" || IPC_NAME == "客室1-4" || IPC_NAME == "客室2-1" || IPC_NAME == "客室2-2" || IPC_NAME == "客室2-3" || IPC_NAME == "客室2-4" || IPC_NAME == "客室3-1" || IPC_NAME == "客室3-2" || IPC_NAME == "客室3-3" || IPC_NAME == "客室3-4" || IPC_NAME == "客室4-1" || IPC_NAME == "客室4-2" || IPC_NAME == "客室4-3" || IPC_NAME == "客室4-4" || IPC_NAME == "客室5-1" || IPC_NAME == "客室5-2" || IPC_NAME == "客室5-3" || IPC_NAME == "客室5-4" || IPC_NAME == "客室6-1" || IPC_NAME == "客室6-2" || IPC_NAME == "客室6-3" || IPC_NAME == "客室6-4" || IPC_NAME == "行车摄像机2-1")
                        // if (IPC_IP == "192.168.1.1" || IPC_IP == "192.168.1.2" || IPC_IP == "192.168.1.3" || IPC_IP == "192.168.1.4" || IPC_IP == "192.168.1.5" || IPC_IP == "192.168.1.6" || IPC_IP == "192.168.1.7" || IPC_IP == "192.168.1.8" || IPC_IP == "192.168.1.9" || IPC_IP == "192.168.1.10" || IPC_IP == "192.168.1.11" || IPC_IP == "192.168.1.12" || IPC_IP == "192.168.1.13" || IPC_IP == "192.168.1.14" || IPC_IP == "192.168.1.15" || IPC_IP == "192.168.1.16" || IPC_IP == "192.168.1.17" || IPC_IP == "192.168.1.18" || IPC_IP == "192.168.1.19" || IPC_IP == "192.168.1.20" || IPC_IP == "192.168.1.21" || IPC_IP == "192.168.1.22" || IPC_IP == "192.168.1.23" || IPC_IP == "192.168.1.24" || IPC_IP == "192.168.1.25" || IPC_IP == "192.168.1.26" || IPC_IP == "192.168.101.47" || IPC_IP == "192.168.101.48")
                        if(IPC_RTSP=="1")
                        {
                          //  PlayRtsp(PictureBox pb, String IPC_NAME,String IPC_PWD,String IPC_USER,String IPC_PORT,String IPC_IP)

                            PlayRtsp(now, IPC_NAME,IPC_PWD,IPC_USER,IPC_PORT,IPC_IP);
                        }
                        else
                        {
                            addMsg(string.Format("[综合实验平台_{0}_配置文件有错，其中IP、端口、用户名、密码不能为空]", IPC_NAME));
                        }
                        /**********************************rtsp视频流播放*******************************************/
                    }
                }
                else
                    MessageBox.Show("请选择播放窗口");
            }

        }

        /// <summary>
        /// 连接摄像头
        /// </summary>
        /// <param name="IP">设备IP地址或者域名</param>
        /// <param name="PORT">设备服务端口号</param>
        /// <param name="USER">设备登录用户名</param>
        /// <param name="PWD">设备登录密码</param>
        /// <returns></returns>
        private int ConnectCamera(string IP, string PORT, string USER, string PWD)
        {
            /*try
            {*/
            CHCNetSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();
            //登录设备 Login the device
            if (IP == "" && PORT == "" && USER == "" && PWD == "")
            {
                string str = string.Format("[{0}]：用户名，密码，端口号不能为空", IP);
                PORT = "8000";
                //addMsg();
            }
            m_lUserID = CHCNetSDK.NET_DVR_Login_V30(IP, int.Parse(PORT), USER, PWD, ref DeviceInfo);

            if (m_lUserID < 0)
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                str = string.Format("[{2}-{0}]连接失败，错误码：{1}", IP, iLastErr, USER); //登录失败，输出错误号
                addMsg(str);
            }
            else
            {
                str = string.Format("[{1}-{0}]连接成功", IP, USER);
                addMsg(str);
            }
            return m_lUserID;
        }

        /// <summary>
        /// 添加提示信息
        /// </summary>
        /// <param name="msg"></param>
        private void addMsg(string msg)
        {
            this.BeginInvoke(new Action(() => { addMsgEvent(msg); }));

        }


        private void addMsgEvent(string msg)
        {
            int index = this.dataGridView1.Rows.Add();
            this.dataGridView1.Rows[index].Cells[0].Value = index + 1;
            this.dataGridView1.Rows[index].Cells[1].Value = msg;
            this.dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows[index].Index;
        }


        /// <summary>
        /// 绑定预置点CombboBox
        /// </summary>
        //private void bindcmbYuzhi()
        //{
        //    Dictionary<int, string> kvDictonary = new Dictionary<int, string>();
        //    for (int i = 0; i < 255; i++)
        //    {
        //        kvDictonary.Add(i + 1, string.Format("{0}", i + 1));
        //    }

        //    BindingSource bs = new BindingSource();
        //    bs.DataSource = kvDictonary;
        //    cmbYuzhi.DataSource = bs;
        //    cmbYuzhi.ValueMember = "Key";
        //    cmbYuzhi.DisplayMember = "Value";
        //}
        String[] str2 = new String[10];


        /// <summary>
        /// 选中的  pictureBox 进行视频的播放 海康摄像头视频的播放 -lcq
        /// </summary>
        /// <param name="pb"></param>
        private void PlayCamera(PictureBox pb)
        {
            int getm_lRealHandle;
            try
            {

                if (pb.Parent == panel1.Controls[0])
                {
                    IntPtr render_wnd = pb.Controls[0].Handle;
                    EnableWindow(render_wnd, true);
                    //getm_lRealHandle = (int)m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", pb.TabIndex + 1)).GetValue(m_lRealHandleModel);//根据所选监控窗口，获取对应的返回索引值 
                    //CHCNetSDK.NET_DVR_StopRealPlay(getm_lRealHandle);//关闭原视频流
                    //pb.Controls[0].Invalidate();

                    get_vlcplayer = (VlcPlayer)m_vlcplayerModel.GetType().GetField(string.Format("vlc_player_{0}", pb.TabIndex + 1)).GetValue(m_vlcplayerModel);
                    get_vlcplayer.Stop();

                    if (m_lRealHandle < 0)
                    {
                        CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = CreatePreview(pb.Controls[0].Handle);  //预览窗口
                        CHCNetSDK.REALDATACALLBACK RealData = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack);//预览实时流回调函数
                        IntPtr pUser = new IntPtr();//用户数据
                        Dictionary<string, string> d = pb.Controls[0].Tag as Dictionary<string, string>;
                        int error = -1;
                        getm_lRealHandle = (int)m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", pb.TabIndex + 1)).GetValue(m_lRealHandleModel);//根据所选监控窗口，获取对应的返回索引值 

                        if (getm_lRealHandle < 0)
                        {
                            pb.Controls[0].Invalidate();
                            error = CHCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, null/*RealData*/, pUser);
                            liststr3.Add(error);
                            liststr4.Add(error);
                            m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", pb.TabIndex + 1)).SetValue(m_lRealHandleModel, error);
                            if (error < 0)
                            {
                                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                                str = string.Format("综合实验平台_{0}:摄像机索引{1} 播放窗口索引{2},{3}", d["IPC_NAME"].ToString(), getm_lRealHandle, pb.TabIndex + 1, iLastErr); //登录失败，输出错误号
                                addMsg(str);
                            }
                            else
                            {
                                str = string.Format("综合实验平台_{0}:摄像机索引{1} 播放窗口索引{2}", d["IPC_NAME"].ToString(), error, pb.TabIndex + 1); //登录失败，输出错误号
                                addMsg(str);
                            }
                        }
                        else
                        {
                            CHCNetSDK.NET_DVR_StopRealPlay(getm_lRealHandle);//关闭原视频流
                            pb.Controls[0].Invalidate();
                            error = CHCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, null/*RealData*/, pUser);
                            liststr3.Add(error);
                            liststr4.Add(error);
                            //   getm_lRealHandle = (int)m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", pb.Parent.TabIndex + 1)).GetValue(m_lRealHandleModel);//根据所选监控窗口，获取对应的返回索引值 
                            m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", pb.TabIndex + 1)).SetValue(m_lRealHandleModel, error);
                            if (error < 0)
                            {
                                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                                str = string.Format("综合实验平台_{0}:摄像机索引{1} 播放窗口索引{2},{3}", d["IPC_NAME"].ToString(), error, pb.TabIndex + 1, iLastErr); //登录失败，输出错误号
                                addMsg(str);
                            }
                            else
                            {
                                str = string.Format("综合实验平台_{0}:摄像机索引{1} 播放窗口索引{2}", d["IPC_NAME"].ToString(), getm_lRealHandle, pb.TabIndex + 1); //登录失败，输出错误号
                                addMsg(str);
                            }
                        }
                    }
                    else
                    {
                        //停止预览 Stop live view 
                        if (!CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle))
                        {
                            iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                            str = "NET_DVR_StopRealPlay failed, error code= " + iLastErr;
                            MessageBox.Show(str);
                            return;
                        }
                        //  m_lRealHandle = -1;

                    }
                    m_lRealHandle = -1;
                }
                else
                {

                    if (m_lRealHandle < 0)
                    {
                        CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = CreatePreview(pb.Handle);  //预览窗口
                        CHCNetSDK.REALDATACALLBACK RealData = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack);//预览实时流回调函数
                        IntPtr pUser = new IntPtr();//用户数据
                        Dictionary<string, string> d = pb.Tag as Dictionary<string, string>;
                        int error = -1;
                        getm_lRealHandle = (int)m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", pb.Parent.TabIndex + 1)).GetValue(m_lRealHandleModel);//根据所选监控窗口，获取对应的返回索引值 

                        if (getm_lRealHandle < 0)
                        {
                            pb.Invalidate();
                            error = CHCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, null/*RealData*/, pUser);
                            liststr3.Add(error);
                            liststr4.Add(error);
                            m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", pb.Parent.TabIndex + 1)).SetValue(m_lRealHandleModel, error);
                            if (error < 0)
                            {
                                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                                str = string.Format("综合实验平台_{0}:摄像机索引{1} 播放窗口索引{2},{3}", d["IPC_NAME"].ToString(), getm_lRealHandle, pb.Parent.TabIndex + 1, iLastErr); //登录失败，输出错误号
                                addMsg(str);
                            }
                            else
                            {
                                str = string.Format("综合实验平台_{0}:摄像机索引{1} 播放窗口索引{2}", d["IPC_NAME"].ToString(), error, pb.Parent.TabIndex + 1); //登录失败，输出错误号
                                addMsg(str);
                            }
                        }
                        else
                        {
                            CHCNetSDK.NET_DVR_StopRealPlay(getm_lRealHandle);//关闭原视频流
                            pb.Invalidate();
                            error = CHCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, null/*RealData*/, pUser);
                            liststr3.Add(error);
                            liststr4.Add(error);
                            //   getm_lRealHandle = (int)m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", pb.Parent.TabIndex + 1)).GetValue(m_lRealHandleModel);//根据所选监控窗口，获取对应的返回索引值 
                            m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", pb.Parent.TabIndex + 1)).SetValue(m_lRealHandleModel, error);
                            if (error < 0)
                            {
                                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                                str = string.Format("综合实验平台_{0}:摄像机索引{1} 播放窗口索引{2},{3}", d["IPC_NAME"].ToString(), error, pb.Parent.TabIndex + 1, iLastErr); //登录失败，输出错误号
                                addMsg(str);
                            }
                            else
                            {
                                str = string.Format("综合实验平台_{0}:摄像机索引{1} 播放窗口索引{2}", d["IPC_NAME"].ToString(), getm_lRealHandle, pb.Parent.TabIndex + 1); //登录失败，输出错误号
                                addMsg(str);
                            }
                        }
                    }
                    else
                    {
                        //停止预览 Stop live view 
                        if (!CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle))
                        {
                            iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                            str = "NET_DVR_StopRealPlay failed, error code= " + iLastErr;
                            MessageBox.Show(str);
                            return;
                        }
                    }
                    m_lRealHandle = -1;
                }
            }
            catch (Exception error)
            {
                WriteLog.Write(GetExceptionMsg(error, string.Empty));
                // throw error;
            }

        }


        /// <summary>
        /// 非海康摄像头的视频播放       使用VLC  进行rtsp视频的播放
        /// </summary>
        /// <param name="pb"></param>
        VlcPlayer get_vlcplayer;
        List<VlcPlayer> listforVlc = new List<VlcPlayer>();
        [DllImport("user32.dll")]
        unsafe public static extern bool EnableWindow(IntPtr hWnd, bool bEnable);//设置Enable属性
        private Boolean PlayRtsp(PictureBox pb, String IPC_NAME, String IPC_PWD, String IPC_USER, String IPC_PORT, String IPC_IP)
        {

            try
            {
                try
                {
                    if (pb.Parent == panel1.Controls[0])
                    {
                        IntPtr render_wnd = pb.Controls[0].Handle;
                        EnableWindow(render_wnd, true);
                        int getm_lRealHandle = (int)m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", pb.TabIndex + 1)).GetValue(m_lRealHandleModel);//根据所选监控窗口，获取对应的返回索引值 
                        CHCNetSDK.NET_DVR_StopRealPlay(getm_lRealHandle);
                       // addMsg(getm_lRealHandle + "SDK");
                        pb.Invalidate();
                        get_vlcplayer = (VlcPlayer)m_vlcplayerModel.GetType().GetField(string.Format("vlc_player_{0}", pb.TabIndex + 1)).GetValue(m_vlcplayerModel);
                        get_vlcplayer.Stop();

                      //  addMsg(pb.Parent.TabIndex + 1 + "串口索引");
                        //IntPtr render_wnd = pb.Handle;
                        get_vlcplayer.SetRenderWindow((int)render_wnd);
                        get_vlcplayer.PlayFromURL("rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mov");
                        
                        //get_vlcplayer.PlayFromURL("rtsp://" + IPC_USER + ":" + IPC_PWD + "@" + IPC_IP + ":" + IPC_PORT + "/h264/ch1/main/av_stream --rtsp-tcp");
                        m_vlcplayerModel.GetType().GetField(string.Format("vlc_player_{0}", pb.TabIndex + 1)).SetValue(m_vlcplayerModel, get_vlcplayer);
                        get_vlcplayer.SetRatio(sbl);
                        this.BeginInvoke(new Action(() => new SaveRtsp2Avi().show())); 
                        
                        listforVlc.Add(get_vlcplayer);
                        EnableWindow(render_wnd, false);
                        addMsg(string.Format("综合实验平台_{0}:摄像机索引{1} 播放窗口索引{2}", IPC_NAME, (int)m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", now.TabIndex + 1)).GetValue(m_lRealHandleModel), now.TabIndex + 1));
                        pb.MouseClick += new MouseEventHandler(RealPlayWnd_Click);
                        pb.MouseDoubleClick += new MouseEventHandler(PictureBox_MouseDoubleClick);
                        return true;

                    }
                    else
                    {


                        int getm_lRealHandle = (int)m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", pb.Parent.TabIndex + 1)).GetValue(m_lRealHandleModel);//根据所选监控窗口，获取对应的返回索引值 
                        CHCNetSDK.NET_DVR_StopRealPlay(getm_lRealHandle);
                      //  addMsg(getm_lRealHandle + "SDK");
                        pb.Invalidate();
                        get_vlcplayer = (VlcPlayer)m_vlcplayerModel.GetType().GetField(string.Format("vlc_player_{0}", pb.Parent.TabIndex + 1)).GetValue(m_vlcplayerModel);
                        get_vlcplayer.Stop();

                      //  addMsg(pb.Parent.TabIndex + 1 + "串口索引");
                        IntPtr render_wnd = pb.Handle;
                        get_vlcplayer.SetRenderWindow((int)render_wnd);
                        get_vlcplayer.PlayFromURL("rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mov");
                       
                        //get_vlcplayer.PlayFromURL("rtsp://" + IPC_USER + ":" + IPC_PWD + "@" + IPC_IP + ":" + IPC_PORT + "/h264/ch1/main/av_stream --rtsp-tcp");
                        m_vlcplayerModel.GetType().GetField(string.Format("vlc_player_{0}", pb.Parent.TabIndex + 1)).SetValue(m_vlcplayerModel, get_vlcplayer);
                        get_vlcplayer.SetRatio(sbl);
                        this.BeginInvoke(new Action(() =>new SaveRtsp2Avi().show()));
                        listforVlc.Add(get_vlcplayer);
                        EnableWindow(render_wnd, false);
                        pb.Parent.BackColor = Color.Black;

                        addMsg(string.Format("综合实验平台_{0}:摄像机索引{1} 播放窗口索引{2}", IPC_NAME, (int)m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", now.Parent.TabIndex + 1)).GetValue(m_lRealHandleModel), now.Parent.TabIndex + 1));
                        pb.Parent.MouseClick += new MouseEventHandler(RealPlayWnd_Click);
                        pb.Parent.MouseDoubleClick += new MouseEventHandler(PictureBox_MouseDoubleClick);
                        return true;
                    }
                }
                catch
                {
                }

            }
            catch
            {
                return false;
            }
            return false;

        }

        //private Boolean PlayRtspForBJ(PictureBox pb, String IPC_NAME, String IPC_PWD, String IPC_USER, String IPC_PORT, String IPC_IP)
        //{

        //    try
        //    {
        //        get_vlcplayer = (VlcPlayer)m_vlcplayerModel.GetType().GetField(string.Format("vlc_player_{0}", pb.Parent.TabIndex + 10)).GetValue(m_vlcplayerModel);
        //        if (get_vlcplayer != null)
        //        {
        //            IntPtr render_wnd = pb.Handle;
        //            get_vlcplayer.SetRenderWindow((int)render_wnd);
        //              get_vlcplayer.PlayFromURL("rtsp://" + IPC_USER + ":" + IPC_PWD + "@" + IPC_IP + ":" + IPC_PORT + "/h264/ch1/main/av_stream");
        //            //get_vlcplayer.PlayFromURL("rtsp://admin:Transcend@192.168.10.186:8000/h264/ch1/main/av_stream");
        //            get_vlcplayer.SetRatio(sbl);
        //            EnableWindow(render_wnd, false);
        //          //  listforVlc.Add(get_vlcplayer);
        //        //    pb.Parent.MouseClick += new MouseEventHandler(RealPlayWnd_Click);
        //          //  pb.Parent.MouseDoubleClick += new MouseEventHandler(PictureBox_MouseDoubleClick);
        //            return true;
        //        }
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    return false;

        //}

        private int btnUp = 0;
        private uint Type;
        private uint dwStop = 0;
        //private void button1_MouseDown(object sender, MouseEventArgs e)
        //{
        //    ControlWithSpeed(CHCNetSDK.TILT_UP);
        //}

        //private void button1_MouseUp(object sender, MouseEventArgs e)
        //{
        //    StopControlWithSpeed();
        //}

        /// <summary>
        /// 停止云台控制
        /// </summary>
        //private void StopControlWithSpeed()
        //{
        //    try
        //    {
        //        dwStop = 1; this.timer1.Enabled = false;
        //        if (!CHCNetSDK.NET_DVR_PTZControlWithSpeed((int)m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", now.Parent.TabIndex + 1)).GetValue(m_lRealHandleModel), Type, dwStop, (uint)cmbSpeed.SelectedIndex + 1))
        //        {
        //            Dictionary<string, string> d = now.Tag as Dictionary<string, string>;
        //            addMsg(string.Format("综合实验平台_{0}:摄像机索引{1} 播放窗口索引{2},{3}", d["IPC_NAME"].ToString(), (int)m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", now.Parent.TabIndex + 1)).GetValue(m_lRealHandleModel), now.Parent.TabIndex + 1, CHCNetSDK.NET_DVR_GetLastError()));
        //        }
        //    }
        //    catch (Exception error)
        //    {
        //        //addMsg(GetExceptionMsg(error, string.Empty));
        //        WriteLog.Write(GetExceptionMsg(error, string.Empty));
        //        // throw error;
        //        return;
        //    }
        //}

        /// <summary>
        /// 开启云台控制
        /// </summary>
        /// <param name="Type">操作类型</param>
        //private void ControlWithSpeed(uint Type)
        //{
        //    if (now != null)
        //    {
        //        int getm_lRealHandle = (int)m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", now.Parent.TabIndex + 1)).GetValue(m_lRealHandleModel);//根据所选监控窗口，获取对应的返回索引值
        //        if (getm_lRealHandle < 0)
        //            addMsg("云台控制：选择的通道未播放，请先播放");
        //        else
        //        {   //add by qhz
        //            dwStop = 0;
        //            this.Type = Type;
        //            this.timer1.Enabled = true;
        //            // add by qhz
        //            CHCNetSDK.NET_DVR_PTZControlWithSpeed((int)m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", now.Parent.TabIndex + 1)).GetValue(m_lRealHandleModel), Type, dwStop, (uint)cmbSpeed.SelectedIndex + 1);
        //        }
        //    }
        //    else
        //        addMsg("云台控制：选择的通道未播放，请先播放");
        //}

        //private void timer1_Tick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //btnUp++; 
        //        if (Type >= 11 && Type <= 16)//云台控制:调焦、聚焦、光圈的
        //        {
        //            if (!CHCNetSDK.NET_DVR_PTZControlWithSpeed((int)m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", now.Parent.TabIndex + 1)).GetValue(m_lRealHandleModel), Type, dwStop, (uint)cmbSpeed.SelectedIndex + 1))
        //            {

        //                Dictionary<string, string> d = now.Tag as Dictionary<string, string>;
        //                addMsg(string.Format("综合实验平台_{0}:摄像机索引{1} 播放窗口索引{2},{3}", d["IPC_NAME"].ToString(), (int)m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", now.Parent.TabIndex + 1)).GetValue(m_lRealHandleModel), now.Parent.TabIndex + 1, CHCNetSDK.NET_DVR_GetLastError()));
        //            }
        //        }
        //        else//云台控制:上、下、左、右
        //        {
        //            if (!CHCNetSDK.NET_DVR_PTZControlWithSpeed((int)m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", now.Parent.TabIndex + 1)).GetValue(m_lRealHandleModel), Type, dwStop, (uint)cmbSpeed.SelectedIndex + 1))
        //            {
        //                Dictionary<string, string> d = now.Tag as Dictionary<string, string>;
        //                addMsg(string.Format("综合实验平台_{0}:摄像机索引{1} 播放窗口索引{2},{3}", d["IPC_NAME"].ToString(), (int)m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", now.Parent.TabIndex + 1)).GetValue(m_lRealHandleModel), now.Parent.TabIndex + 1, CHCNetSDK.NET_DVR_GetLastError()));
        //            }
        //        }
        //    }
        //    catch (Exception error)
        //    {
        //        WriteLog.Write(GetExceptionMsg(error, string.Empty));
        //        addMsg("云台控制：选择的通道未播放，请先播放");
        //        return;
        //    }
        //}
        ////addMsg(string.Format("测试：{0}", btnUp)); 

        //private void button2_MouseDown(object sender, MouseEventArgs e)
        //{
        //    ControlWithSpeed(CHCNetSDK.PAN_LEFT);

        //}

        //private void button2_MouseUp(object sender, MouseEventArgs e)
        //{
        //    StopControlWithSpeed();
        //}

        //private void button3_MouseDown(object sender, MouseEventArgs e)
        //{
        //    ControlWithSpeed(CHCNetSDK.PAN_RIGHT);
        //}

        //private void button3_MouseUp(object sender, MouseEventArgs e)
        //{
        //    StopControlWithSpeed();
        //}

        //private void button4_MouseDown(object sender, MouseEventArgs e)
        //{
        //    ControlWithSpeed(CHCNetSDK.TILT_DOWN);
        //}

        //private void button4_MouseUp(object sender, MouseEventArgs e)
        //{
        //    StopControlWithSpeed();
        //}

        //private void button6_MouseDown(object sender, MouseEventArgs e)
        //{
        //    dwStop = 0;
        //    ControlWithSpeed(CHCNetSDK.ZOOM_IN);
        //}

        //private void button6_MouseUp(object sender, MouseEventArgs e)
        //{
        //    StopControlWithSpeed();
        //}

        //private void button5_MouseDown(object sender, MouseEventArgs e)
        //{
        //    dwStop = 0;
        //    ControlWithSpeed(CHCNetSDK.ZOOM_OUT);
        //}

        //private void button5_MouseUp(object sender, MouseEventArgs e)
        //{
        //    StopControlWithSpeed();
        //}

        //private void button8_MouseDown(object sender, MouseEventArgs e)
        //{
        //    dwStop = 0;
        //    ControlWithSpeed(CHCNetSDK.FOCUS_FAR);
        //}

        //private void button8_MouseUp(object sender, MouseEventArgs e)
        //{
        //    StopControlWithSpeed();
        //}

        //private void button7_MouseDown(object sender, MouseEventArgs e)
        //{
        //    dwStop = 0;
        //    ControlWithSpeed(CHCNetSDK.FOCUS_NEAR);
        //}

        //private void button7_MouseUp(object sender, MouseEventArgs e)
        //{
        //    StopControlWithSpeed();
        //}

        //private void button10_MouseDown(object sender, MouseEventArgs e)
        //{
        //    dwStop = 0;
        //    ControlWithSpeed(CHCNetSDK.IRIS_CLOSE);
        //}

        //private void button10_MouseUp(object sender, MouseEventArgs e)
        //{
        //    StopControlWithSpeed();
        //}

        //private void button9_MouseDown(object sender, MouseEventArgs e)
        //{
        //    dwStop = 0;
        //    ControlWithSpeed(CHCNetSDK.IRIS_OPEN);
        //}

        //private void button9_MouseUp(object sender, MouseEventArgs e)
        //{
        //    StopControlWithSpeed();
        //}

        /// <summary>
        /// 生成自定义异常消息
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <param name="backStr">备用异常消息：当ex为null时有效</param>
        /// <returns>异常字符串文本</returns>
        static string GetExceptionMsg(Exception ex, string backStr)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("****************************异常文本****************************");
            sb.AppendLine("【出现时间】：" + DateTime.Now.ToString());
            if (ex != null)
            {
                sb.AppendLine("【异常类型】：" + ex.GetType().Name);
                sb.AppendLine("【异常信息】：" + ex.Message);
                sb.AppendLine("【堆栈调用】：" + ex.StackTrace);
            }
            else
            {
                sb.AppendLine("【未处理异常】：" + backStr);
            }
            sb.AppendLine("***************************************************************");
            return sb.ToString();
        }
        private delegate void lunxun();

        private void button13_Click(object sender, EventArgs e)
        {   //add by qhz
            //先判断当前画面是否和选择的轮询画面一致，不一致则重新构建
            //if (cmbMode.Text == "4画面" && panel1.Controls[0].Controls.Count != 4)
            //{

            //    panel1.Controls.Clear();
            //    CreatePictureBox(4);

            //}
            //else if (cmbMode.Text == "9画面" && panel1.Controls[0].Controls.Count != 9)
            //{
            //    ;
            //    panel1.Controls.Clear();
            //    CreatePictureBox(9);
            //}
            //else if (cmbMode.Text == "1画面" && panel1.Controls[0].Controls.Count != 1)
            //{

            //    panel1.Controls.Clear();
            //    j = -1;
            //    zh = 0;
            //    CreatePictureBox(1);
            //}
            //if (button13.Text == "启动轮询")
            //{
            //    button13.Text = "停止轮询";
            //    z = 0;
            //    j = -1;
            //    zh = 0;
                
            //    flag = true;
            //    StopPolling(true);
             

            //}
            //else
            //{
            //    // flag = false;
            //    StopPolling(false);
            //    button13.Text = "启动轮询";

            //    //判断当前是几画面轮训
            //    switch (panel1.Controls[0].Controls.Count) {
            //        case 1:
            //            try
            //            {
            //                timerfor9.Change(Timeout.Infinite, 1000);
            //                timerfor9.Dispose();
            //            }
            //            catch
            //            {

            //            }
            //            break;
            //        case 4:
            //            try
            //            {
            //                timerfor9.Change(Timeout.Infinite, 1000);
            //                timerfor9.Dispose();
            //            }
            //            catch
            //            {

            //            }
            //            break;
            //        case 9:
            //            try
            //            {
            //                timerfor9.Change(Timeout.Infinite, 1000);
            //                timerfor9.Dispose();
            //            }
            //            catch
            //            {

            //            }
            //            break;
                
                
            //    }
            //}
        }

        /// <summary>
        /// 轮询
        /// </summary>
        /// <param name="flag">开启、关闭</param>
        //设置bj的目的是只让创建的任务只执行一次，多次执行的话会,timer4定时器失效。
        private int bj = 1;
        private int bjj = 1;
        //private void StopPolling(bool flag)
        //{
        //    if (!flag)
        //    {   // add by qhz
        //        if (bjj == 1)
        //        {
        //           // button13.Text = "启动轮询";
        //           // button13.Enabled = true;
        //            if (!cancellationTokenSource.IsCancellationRequested)
        //            {
        //                l = new List<int>();
        //                cancellationTokenSource.Cancel();
        //                bjj++;
        //            }
        //        }
        //        else
        //        {
        //            //button13.Text = "启动轮询";
        //            try
        //            {
        //                timerfor9.Change(Timeout.Infinite, 1000);
        //                timerfor9.Dispose();
        //            }
        //            catch {
        //                return;
        //            }
                   

        //        }
        //    }
        //    else
        //    {
        //        if (bj == 1)
        //        {
        //            cancellationTokenSource = new CancellationTokenSource();
        //            Task.Factory.StartNew(xx, cancellationTokenSource.Token);
        //                if (!cancellationTokenSource.IsCancellationRequested)
        //                {
        //                    //button13.Text = "停止轮询";
        //                   // button13.Enabled = true;
        //                    z = 0; cb = 0;
        //                 // Timer1();
        //                   new Thread(new ThreadStart(Timer1)).Start();
        //                        bj++;
        //                }
        //        }
        //        else if (bj != 1)
        //        {
            
        //            while (true)
        //            {
                        
        //              //  button13.Text = "停止轮询";
        //               // button13.Enabled = true;
        //                while (true) {
        //                    if (panel1.Controls[0].Controls.Count == 1&&ll.Count==IPCCOUNT) {
        //                     //   Timer1();
        //                    }
        //                    else if (panel1.Controls[0].Controls.Count == 4 && ll.Count == IPCCOUNT)
        //                    {
        //                      //  Timer4();
        //                    }
        //                    else if (panel1.Controls[0].Controls.Count == 9 && ll.Count == IPCCOUNT)
        //                    {
        //                       // Timer9();
        //                    }
        //                    break;
                        
        //                }
        //                break;
        //            }
        //        }
        //    }

        //}
        
        private void button15_Click(object sender, EventArgs e)
        {
            Time t = new Time();
            t.SendEvent += (addMsg);
            t.ShowDialog();
        }
        
        private bool flag = false;
        List<int> l = new List<int>();
      static  List<List<String>> ll = new List<List<String>>();
        // add by qhz
        private int cb = 0;
        private int cbb = 0;
        int treenodess;
        //利用线程池将连接摄像头的信息存放到list集合里
        private void xx()
        {
            shutdownAll();
            ll.Clear();
            int treenodes = -1;
            int bzw = 1;
            foreach (TreeNode n in treeView1.Nodes[0].Nodes)
            {
                ThreadPool.QueueUserWorkItem(
                         new WaitCallback(obj =>
                         {
                             //add by qhz
                             Dictionary<string, string> d = n.Tag as Dictionary<string, string>;
                             String IPC_IP = d["IPC_IP"].ToString();
                             String IPC_PORT = d["IPC_PORT"].ToString();
                             String IPC_USER = d["IPC_USER"].ToString();
                             String IPC_PWD = d["IPC_PWD"].ToString();
                             String IPC_NAME = d["IPC_NAME"].ToString();
                             String RTSP = d["IPC_RTSP"].ToString();
                             String IPC_V4 = d["IPC_V4"].ToString();
                             String IPC_JS = d["IPC_JS"].ToString();
                             String IPC_V4_END = d["IPC_V4_END"].ToString();
                             String IPC_V9_END = d["IPC_V9_END"].ToString();
                             String IPC_V9 = d["IPC_V9"].ToString();
                             String IPC_V1_END = d["IPC_V1_END"].ToString();
                             // if (IPC_IP == "192.168.1.1" || IPC_IP == "192.168.1.2" || IPC_IP == "192.168.1.3" || IPC_IP == "192.168.1.4" || IPC_IP == "192.168.1.5" || IPC_IP == "192.168.1.6" || IPC_IP == "192.168.1.7" || IPC_IP == "192.168.1.8" || IPC_IP == "192.168.1.9" || IPC_IP == "192.168.1.10" || IPC_IP == "192.168.1.11" || IPC_IP == "192.168.1.12" || IPC_IP == "192.168.1.13" || IPC_IP == "192.168.1.14" || IPC_IP == "192.168.1.15" || IPC_IP == "192.168.1.16" || IPC_IP == "192.168.1.17" || IPC_IP == "192.168.1.18" || IPC_IP == "192.168.1.19" || IPC_IP == "192.168.1.20" || IPC_IP == "192.168.1.21" || IPC_IP == "192.168.1.22" || IPC_IP == "192.168.1.23" || IPC_IP == "192.168.1.24" || IPC_IP == "192.168.1.25" || IPC_IP == "192.168.1.26" || IPC_IP == "192.168.101.47"||IPC_IP=="192.168.101.48")
                             if(RTSP=="1")
                             {
                                 treenodess = PingIP(IPC_IP, IPC_USER);
                               
                             }
                             else
                             {
                                 treenodess = ConnectCamera(IPC_IP, IPC_PORT, IPC_USER, IPC_PWD);
                                
                             }
                             String m_iuserid = treenodess.ToString();
                             List<String> l = new List<string>();
                             l.Add(IPC_IP);
                             l.Add(IPC_PORT);
                             l.Add(IPC_USER);
                             l.Add(IPC_PWD);
                             l.Add(IPC_NAME);
                             l.Add(m_iuserid);
                             l.Add(RTSP);
                             l.Add(IPC_V4);
                             l.Add(IPC_V4_END);
                             l.Add(IPC_V9_END);
                             l.Add(IPC_V9);
                             l.Add(IPC_V1_END);
                             ll.Add(l);
                             bzw++;

                         }));
            }
            
        }
        //add by qhz
        //轮询检测该摄像头是否能连通
        private void luntestip()
        {
            if (ll.Count == IPCCOUNT)
            {
                List<string> sections = IniFile.ReadSections();
                foreach (string section in sections)
                {
                    ThreadPool.QueueUserWorkItem(
                             new WaitCallback(obj =>
                             {
                                 if (section.Contains("IPC"))
                                 {
                                     string IPC_NAME = IniFile.ReadIniData(section, "IPC_NAME", "");
                                     string IPC_IP = IniFile.ReadIniData(section, "IPC_IP", "");
                                     string IPC_PORT = IniFile.ReadIniData(section, "IPC_PORT", "");
                                     string IPC_USER = IniFile.ReadIniData(section, "IPC_USER", "");
                                     string IPC_PWD = IniFile.ReadIniData(section, "IPC_PWD", "");

                                     foreach (var l in ll)
                                     {
                                         if (l[1] == IPC_PORT && l[2] == IPC_USER && l[3] == IPC_PWD && l[4] == IPC_NAME)
                                         {
                                         // if (l[0].ToString() == "192.168.1.1" || l[0].ToString() == "192.168.1.2" || l[0].ToString() == "192.168.1.3" || l[0].ToString() == "192.168.1.4" || l[0].ToString() == "192.168.1.5" || l[0].ToString() == "192.168.1.6" || l[0].ToString() == "192.168.1.7" || l[0].ToString() == "192.168.1.8" || l[0].ToString() == "192.168.1.9" || l[0].ToString() == "192.168.1.10" || l[0].ToString() == "192.168.1.11" || l[0].ToString() == "192.168.1.12" || l[0].ToString() == "192.168.1.13" || l[0].ToString() == "192.168.1.14" || l[0].ToString() == "192.168.1.15" || l[0].ToString() == "192.168.1.16" || l[0].ToString() == "192.168.1.17" || l[0].ToString() == "192.168.1.18" || l[0].ToString() == "192.168.1.19" || l[0].ToString() == "192.168.1.20" || l[0].ToString() == "192.168.1.21" || l[0].ToString() == "192.168.1.22" || l[0].ToString() == "192.168.1.23" || l[0].ToString() == "192.168.1.24" || l[0].ToString() == "192.168.1.25" || l[0].ToString() == "192.168.1.26" || l[0].ToString() == "192.168.101.47" || l[0].ToString() == "192.168.101.48")
                                         if (l[6]=="1")
                                             {
                                                 if (int.Parse(l[5]) < 0)
                                                 {
                                                     l[5] = PingIP(l[0].ToString(), l[2]).ToString();
                                                 }

                                             }
                                             else
                                             {
                                                 if (int.Parse(l[5]) < 0)
                                                 {
                                                     l[5] = ConnectCamera(IPC_IP, IPC_PORT, IPC_USER, IPC_PWD).ToString();
                                                 }
                                             }
                                          
                                             break;
                                         }
                                     }
                                    

                                 }
                             }));
                   
                }
            }

        }

        //清理上个页面播放的rtsp视频流
        // 释放libvlc实例
        private void vlcClear()
        {

            if (listforVlc.Count != 0)
            {
                for (int i = 0; i < listforVlc.Count; i++)
                {
                    VlcPlayer g = listforVlc[i];
                    g.Stop();//暂时注释
                    addMsg("视频流已关闭");
                }
                listforVlc.Clear();

            }


        }
        //关闭分屏的rtsp视频流
        private void vlcClearforfp()
        {
            if (listlxrtspforfp.Count != 0)
            {
                for (int i = 0; i < listlxrtspforfp.Count; i++)
                {
                    VlcPlayer g = listlxrtspforfp[i];
                    g.Stop();//暂时注释掉 
                    addMsg("分屏视频流已关闭");
                }
                listlxrtspforfp.Clear();

            }

        }
    
        //关闭一画面轮询的摄像头
        private void sxtClearfor1change()
        {   if(liststr4.Count != 0){
            foreach(var i in liststr4){
            if(i>=0){
               CHCNetSDK.NET_DVR_StopRealPlay(i);
             }
            }
            liststr4.Clear();
        }
        }
        //关闭正在播放的4,9画面的轮询摄像头
        private void sxtClearfro49change()
        {
            if(liststr3.Count != 0){
            foreach(var i in liststr3){
                if(i>=0){ 
                CHCNetSDK.NET_DVR_StopRealPlay(i);
                }
            }
            liststr3.Clear();
            }
        }

        //对正在播放的分屏的摄像头进行关闭。
        private void sxtClearforfp()
        {

            if (liststrforfp.Count != 0)
            {
                foreach (var i in liststrforfp)
                {
                    if (i >= 0)
                    {
                        CHCNetSDK.NET_DVR_StopRealPlay(i);
                    }
                }
                liststrforfp.Clear();
            }


        }
        /// <summary>

        /// 用于检查IP地址或域名是否可以使用TCP/IP协议访问(使用Ping命令),true表示Ping成功,false表示Ping失败 
        /// </summary>
        /// <param name="strIpOrDName">输入参数,表示IP地址或域名</param>
        /// <returns></returns>
        private int PingIP(string DoNameOrIP, String User)
        {
            try
            {
                Ping objPingSender = new Ping();
                PingOptions objPinOptions = new PingOptions();
                objPinOptions.DontFragment = true;
                string data = "";
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                int intTimeout = 1000;

                PingReply objPinReply = objPingSender.Send(DoNameOrIP, intTimeout, buffer, objPinOptions);
                string strInfo = objPinReply.Status.ToString();
                if (strInfo == "Success")
                {
                    String str = String.Format("[{1}-{0}车载摄像头连接成功]", DoNameOrIP, User);
                    addMsg(str);
                    return 11;
                }
                else
                {
                    String str = String.Format("[{1}-{0}车载摄像头连接失败]", DoNameOrIP, User);
                    addMsg(str);
                    return -11;
                }
            }
            catch (Exception)
            {
                addMsg("IP地址不能为空");
                return -11;
            }
        }




        //用于pingIP是否成功 如果成功则根据返回值来更新摄像头的在线状态。
        private Boolean Pingiptochange(string DoNameOrIP, String User)
        {
            try
            {
                Ping objPingSender = new Ping();
                PingOptions objPinOptions = new PingOptions();
                objPinOptions.DontFragment = true;
                string data = "";
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                int intTimeout = 1000;

                PingReply objPinReply = objPingSender.Send(DoNameOrIP, intTimeout, buffer, objPinOptions);
                string strInfo = objPinReply.Status.ToString();
                if (strInfo == "Success")
                {
                    String str = String.Format("[{1}-{0}车载摄像头连接成功]", DoNameOrIP, User);
                    addMsg(str);
                    return true;
                    
                }
                else
                {
                    String str = String.Format("[{1}-{0}车载摄像头连接失败]", DoNameOrIP, User);
                    addMsg(str);
                    return false;
                }
            }
            catch (Exception)
            {
                addMsg("IP地址不能为空");
                return false;
            }
        
        
        }

        private void icochange() {
            
            treeView1.ImageList = imageList1;
            while(true){
                for (int i = 0; i < treeView1.Nodes[0].GetNodeCount(false); i++)
                {

                    foreach (TreeNode tn in treeView1.Nodes[0].Nodes[i].Nodes) {


                        ThreadPool.QueueUserWorkItem(
                                 new WaitCallback(obj =>
                                 {
                                     if (ll.Count == IPCCOUNT)
                                     {
                                         foreach (List<String> l in ll)
                                         {
                                             if (tn.Text == l[4] && l[4] != null)
                                             {
                                                 if (Pingiptochange(l[0], l[2]))
                                                 {
                                                     tn.ImageIndex = 1;
                                                     tn.SelectedImageIndex = 1;
                                                     break;
                                                 }
                                                 else
                                                 {
                                                     tn.ImageIndex = 0;
                                                     tn.SelectedImageIndex = 0;
                                                     break;
                                                 }
                                             }
                                         }

                                     }
                                 }));
                        Thread.Sleep(100);
                        //tn.ImageIndex = 0;//表示其图片为图片列表中的第一个图片，若用第二个图片
                        //则tn.ImageIndex = 1;依次类推
                    
                    
                    
                    
                    }
                }  
            //foreach (TreeNode tn in treeView1.Nodes[0].Nodes)
            //{
            //    ThreadPool.QueueUserWorkItem(
            //                 new WaitCallback(obj =>
            //                 {
            //    if (ll.Count == IPCCOUNT)
            //    {
            //        foreach (List<String> l in ll)
            //        {
            //            if (tn.Text == l[4] && l[4] != null)
            //            {
            //                if (Pingiptochange(l[0],l[2]))
            //                {
            //                    tn.ImageIndex = 1;
            //                    tn.SelectedImageIndex = 1;
            //                    break;
            //                }
            //                else
            //                {
            //                    tn.ImageIndex = 0;
            //                    tn.SelectedImageIndex = 0;
            //                    break; 
            //                }
            //            }
            //        }

            //   }
            //                 }));
            //    Thread.Sleep(100);
            //    //tn.ImageIndex = 0;//表示其图片为图片列表中的第一个图片，若用第二个图片
            //    //则tn.ImageIndex = 1;依次类推
            //}
            break;
            }
        
        
        }
        public static List<Task> TaskList = new List<Task>();
        //画面轮询
        int z = 0;
        int j = -1;
        int zh = 0;
        List<String> liststr = new List<String>();
        List<int> liststr3 = new List<int>();
        [DllImport(@"..\bin\HCNetSDK.dll")]
        public static extern bool NET_DVR_Logout_V30(Int32 lUserID);
        private bool logout(Int32 lUserID)
        {
            Boolean b = NET_DVR_Logout_V30(lUserID);
            return b;
        }
        //对4，9画面摄像头进行播放
        int pos;
        private void lunxunChange(int i, String IPC_IP, String IPC_NAME)
        {
            try
            {
                //alter by qhz
                PictureBox p = panel1.Controls[0].Controls[cb++].Controls[0] as PictureBox;
                CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = CreatePreview(p.Handle);
                CHCNetSDK.REALDATACALLBACK RealData = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack);//预览实时流回调函数
                IntPtr pUser = new IntPtr();//用户数据
                IntPtr render_wnd = p.Handle;
                Dictionary<string, string> d = p.Tag as Dictionary<string, string>;
                int error = -1;
                if (i < 0)
                {
                    error = CHCNetSDK.NET_DVR_RealPlay_V40(i, ref lpPreviewInfo, null/*RealData*/, pUser);
                    liststr3.Add(error);
                    m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", p.Parent.TabIndex + 1)).SetValue(m_lRealHandleModel, error);
                    if (error < 0)
                    {
                        iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                        str = string.Format("综合实验平台:摄像机:{0} 播放窗口索引{2},错误码：{3}", IPC_NAME, i, cb + 1, iLastErr); //登录失败，输出错误号
                        addMsg(str);
                    }
                    else
                    {
                        str = string.Format("综合实验平台:摄像机：{0} 播放窗口索引{2}", IPC_NAME, error, cb + 1); //登录失败，输出错误号
                        addMsg(str);
                    }
                }
                else
                {
                    error = CHCNetSDK.NET_DVR_RealPlay_V40(i, ref lpPreviewInfo, null/*RealData*/, pUser);
                    liststr3.Add(error);
                    m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", p.Parent.TabIndex + 1)).SetValue(m_lRealHandleModel, error);
                    if (error < 0)
                    {
                        iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                        str = string.Format("综合实验平台:摄像机IP:{0} 播放窗口索引{2},错误码：{3}", IPC_NAME, error, p.TabIndex + 1, iLastErr); //登录失败，输出错误号
                       // addMsg(str);
                    }
                    else
                    {    //alter by qhz
                        str = string.Format("综合实验平台:摄像机索引{0} 播放窗口索引{2}", IPC_NAME, /*i*/error, p.TabIndex + 1); //登录失败，输出错误号
                       // addMsg(str);
                    }
                }
            }
            catch (Exception error)
            {
                WriteLog.Write(GetExceptionMsg(error, string.Empty));
                throw error;
            }
        }


        private void lxRtspfor1(String IPC_IP, String IPC_NAME, String IPC_PWD, String IPC_PORT, String IPC_USER)
        {
            PictureBox pb = panel1.Controls[0].Controls[0].Controls[0] as PictureBox;
            try
            {
                get_vlcplayer = (VlcPlayer)m_vlcplayerModel.GetType().GetField(string.Format("vlc_player_{0}", pb.Parent.TabIndex + 1)).GetValue(m_vlcplayerModel);
                if (get_vlcplayer != null)
                {
                    IntPtr render_wnd = pb.Handle;
                    get_vlcplayer.SetRenderWindow((int)render_wnd);
                    get_vlcplayer.PlayFromURL("rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mov");
                  //  get_vlcplayer.PlayFromURL("rtsp://" + IPC_USER + ":" + IPC_PWD + "@" + IPC_IP + ":" + IPC_PORT + "/h264/ch1/main/av_stream --rtsp-tcp");
                    listforVlc.Add(get_vlcplayer);
                    EnableWindow(render_wnd, false);
                    pb.Parent.MouseClick += new MouseEventHandler(RealPlayWnd_Click);
                    pb.Parent.MouseDoubleClick += new MouseEventHandler(PictureBox_MouseDoubleClick);
                }
            }
            catch
            {

            }

        }


        //针对4，9画面的RTSP视频流播放
        private void lxRtspfor49(String IPC_IP, String IPC_NAME, String IPC_PWD, String IPC_PORT, String IPC_USER)
        {
            PictureBox pb = panel1.Controls[0].Controls[cb++].Controls[0] as PictureBox; 
            try
            {
                get_vlcplayer = (VlcPlayer)m_vlcplayerModel.GetType().GetField(string.Format("vlc_player_{0}", pb.Parent.TabIndex + 1)).GetValue(m_vlcplayerModel);
                if (get_vlcplayer != null)
                {
                    IntPtr render_wnd = pb.Handle;
                    get_vlcplayer.SetRenderWindow((int)render_wnd);
                    get_vlcplayer.PlayFromURL("rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mov");
                   // get_vlcplayer.PlayFromURL("rtsp://" + IPC_USER + ":" + IPC_PWD + "@" + IPC_IP + ":" + IPC_PORT + "/h264/ch1/main/av_stream --rtsp-tcp");
                    get_vlcplayer.SetRatio(sbl);
                    listforVlc.Add(get_vlcplayer);
                    EnableWindow(render_wnd, false);
                    String str = String.Format("播放的车载摄像头名称：{0},IP地址为：{1}", IPC_NAME, IPC_IP);
                    pb.Parent.MouseClick += new MouseEventHandler(RealPlayWnd_Click);
                    pb.Parent.MouseDoubleClick += new MouseEventHandler(PictureBox_MouseDoubleClick);
                }
            }
            catch
            {

            }
        }

        private void txtTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')//这是允许输入退格键  
            {
                if ((e.KeyChar < '0') || (e.KeyChar > '9'))//这是允许输入0-9数字  
                {
                    e.Handled = true;
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {

            TVWall tv = new TVWall();
            tv.ShowDialog();
        }
        TimeSpan ts = new TimeSpan(0, 0, 10);
        private void button17_Click(object sender, EventArgs e)
        {

            VlcPlayerForm vlcp = new VlcPlayerForm();
            vlcp.ShowDialog();
        }

        private void timer4_Tick(object sender, EventArgs e)
        {   
            luntestip();
          
        }
       
        //对窗口大小变化进行的pictureBox的重新布局
        private void Previer_reload(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                
                for (int i = 0; i < panel1.Controls[0].Controls.Count; i++)
                {
                    PictureBox pb = panel1.Controls[0].Controls[i] as PictureBox;
                    pb.BackColor = System.Drawing.Color.Black;
                    pb.Width = panel1.Width / (panel1.Controls[0].Controls.Count == 9 ? 3 : (panel1.Controls[0].Controls.Count == 4 ? 2 : 1)) - 7;
                    pb.Height = panel1.Height / (panel1.Controls[0].Controls.Count == 9 ? 3 : (panel1.Controls[0].Controls.Count == 4 ? 2 : 1)) - 7;
                    pb.Name = string.Format("PictureBox{0}", panel1.Controls[0].Controls.Count);

                }
                for (int i = 0; i < panel1.Controls[0].Controls.Count; i++)
                {
                    PictureBox pb = panel1.Controls[0].Controls[i].Controls[0] as PictureBox;
                    pb.BackColor = System.Drawing.Color.Black;
                    pb.Width = panel1.Width / (panel1.Controls[0].Controls.Count == 9 ? 3 : (panel1.Controls[0].Controls.Count == 4 ? 2 : 1)) - 9;
                    pb.Height = panel1.Height / (panel1.Controls[0].Controls.Count == 9 ? 3 : (panel1.Controls[0].Controls.Count == 4 ? 2 : 1)) - 9;
                    //设置rtsp视频画面的width和height
                    sbl = pb.Width + ":" + pb.Height;
                    if(listforVlc.Count!=0){
                        foreach (var vlc in listforVlc)
                        {
                            vlc.SetRatio(sbl);

                        }
                    }
                    
                    pb.Name = string.Format("PictureBox{0}", panel1.Controls[0].Controls.Count);

                }
            }
            else if (this.WindowState == FormWindowState.Maximized)
            {
                //初始化时便进入到这里
             
                for (int i = 0; i < panel1.Controls[0].Controls.Count; i++)
                {
                    PictureBox pb = panel1.Controls[0].Controls[i] as PictureBox;
                    pb.BackColor = System.Drawing.Color.Black;
                    pb.Width = panel1.Width / (panel1.Controls[0].Controls.Count == 9 ? 3 : (panel1.Controls[0].Controls.Count == 4 ? 2 : 1)) - 7;
                    pb.Height = panel1.Height / (panel1.Controls[0].Controls.Count == 9 ? 3 : (panel1.Controls[0].Controls.Count == 4 ? 2 : 1)) - 7;
                    
                }
                for (int i = 0; i < panel1.Controls[0].Controls.Count; i++)
                {
                    PictureBox pb = panel1.Controls[0].Controls[i].Controls[0] as PictureBox;
                    pb.BackColor = System.Drawing.Color.Black;
                    pb.Width = panel1.Width / (panel1.Controls[0].Controls.Count == 9 ? 3 : (panel1.Controls[0].Controls.Count == 4 ? 2 : 1)) - 9;
                    pb.Height = panel1.Height / (panel1.Controls[0].Controls.Count == 9 ? 3 : (panel1.Controls[0].Controls.Count == 4 ? 2 : 1)) - 9;
                    pb.Name = string.Format("PictureBox{0}", panel1.Controls[0].Controls.Count);
                    sbl = pb.Width + ":" + pb.Height;
                    if (listforVlc.Count != 0)
                    {
                        foreach (var vlc in listforVlc)
                        {
                            vlc.SetRatio(sbl);

                        }
                    }
                }

            }
        }
        //预置点调用
        private void button11_Click(object sender, EventArgs e)
        {
            int getm_lRealHandle = (int)m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", now.Parent.TabIndex + 1)).GetValue(m_lRealHandleModel);//根据所选监控窗口，获取对应的返回索引值
         //   dwPresetIndex = (uint)int.Parse(cmbYuzhi.Text);
            Boolean b = CHCNetSDK.NET_DVR_PTZPreset(getm_lRealHandle, GOTO_PRESET, dwPresetIndex);
        }
        //预置点设置
        private void button12_Click(object sender, EventArgs e)
        {
            int getm_lRealHandle = (int)m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", now.Parent.TabIndex + 1)).GetValue(m_lRealHandleModel);//根据所选监控窗口，获取对应的返回索引值
          //  dwPresetIndex = (uint)int.Parse(cmbYuzhi.Text);
            Boolean b = CHCNetSDK.NET_DVR_PTZPreset(getm_lRealHandle, SET_PRESET, dwPresetIndex);

        }

        private void form_closing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show("你确定要关闭应用程序吗？", "关闭提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
            {
                this.FormClosing -= new FormClosingEventHandler(this.form_closing);//为保证Application.Exit();时不再弹出提示，所以将FormClosing事件取消
                shutdownAll();
                //退出登录
                foreach (var l in ll)
                {

                    if (int.Parse(l[5]) >= 0)
                    {
                        CHCNetSDK.NET_DVR_Logout(int.Parse(l[5]));
                        // m_lUserID = -1;
                    }
                }
                vlcClear();
                vlcClearforfp();
               
                //释放SDK资源
                CHCNetSDK.NET_DVR_Cleanup();
                //关闭主程序的时候关闭子线程
                try
                {
                    Thread.CurrentThread.Abort();
                }
                catch (ThreadAbortException)
                {
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
                //释放定时器的资源
                try {
                    timerfor9.Dispose();
                    timerforfp.Dispose();
                    timerforico.Dispose();
                    timer1.Dispose();
                    timer4.Dispose();
                   
                }
                catch {
                }
                

                Application.Exit();//退出整个应用程序
            }
            else
            {
                e.Cancel = true; //取消关闭事件
            }
        }
        //回放功能的实现
        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                sxtClearforfp();
                vlcClearforfp();
                CreatePictureBox(1);
                try {
                    timerfor9.Change(Timeout.Infinite, 1000);
                    timerfor9.Dispose();
                }
                catch {
                
                }
              //  button13.Text = "启动轮询";
                using (System.Diagnostics.Process myPro = new System.Diagnostics.Process())
                {
                    myPro.StartInfo.FileName = "cmd.exe";
                    myPro.StartInfo.UseShellExecute = false;
                    myPro.StartInfo.RedirectStandardInput = true;
                    myPro.StartInfo.RedirectStandardOutput = true;
                    myPro.StartInfo.RedirectStandardError = true;
                    myPro.StartInfo.CreateNoWindow = true;
                    myPro.Start();
                    String cmdExe = "C:\\Program Files\\iVMS-4200 Station\\iVMS-4200\\iVMS-4200 Client\\iVMS-4200.exe";
                    String cmdStr = "";
                    //如果调用程序路径中有空格时，cmd命令执行失败，可以用双引号括起来 ，在这里两个引号表示一个引号（转义）
                    string str = string.Format(@"""{0}"" {1} {2}", cmdExe, cmdStr, "&exit");
                    myPro.StandardInput.WriteLine(str);
                    myPro.StandardInput.AutoFlush = true;
                    myPro.WaitForExit();
                }
            }
            catch
            {

            }
        }
        //创建报警窗口
        private string sbl3;
        // String[] ayy;
        CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfofroBJ;
        //在分屏上创建报警窗口
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arr"></param>
        private void bjck(String arr) {
            //遍历listDict，获取具体摄像头信息
            foreach (var l in ll)
            {
                if (l[1] == arr)
                {
                    String IPC_IP = l[0];
                    String IPC_PORT = l[1];
                    String IPC_USER = l[2];
                    String IPC_PWD = l[3];
                    String IPC_NAME = l[4];
                    int m_userid = int.Parse(l[5]);
                    String IPC_RTSP = l[6];
                    //创建一个form 播放视频
                    sc = Screen.AllScreens;
                   // System.Windows.Forms.Form frm3 = new Form();
                    if (Screen.AllScreens.Length == 2)
                    {
                       // frm2.Controls.Clear();
                        sxtClearforfp();
                        vlcClearforfp();
                        Thread.Sleep(100);
                        createPicBox(1);
                        timerforfp.Dispose();
                    }
                    PictureBox pbBJ = frm2.Controls[0].Controls[0] as PictureBox;

                    //pbBJ.Invalidate();

                    //判断是播放摄像头视频流还是RTSP视频流
                   //   if (l[1] == "11001" || l[1] == "11002" || l[1] == "11003" || l[1] == "11004" || l[1] == "11005" || l[1] == "11006" || l[1] == "11007" || l[1] == "11008" || l[1] == "11009" || l[1] == "11010" || l[1] == "11011" || l[1] == "11012" || l[1] == "11013" || l[1] == "11014" || l[1] == "11015" || l[1] == "11016" || l[1] == "11017" || l[1] == "11018" || l[1] == "11019" || l[1] == "11020" || l[1] == "11021" || l[1] == "11022" || l[1] == "11023" || l[1] == "11024" || l[1] == "11025" || l[1] == "11026" || l[1] == "11047" || l[1] == "11048")
                    if(l[6]=="1")
                    {
                        try
                        {
                            get_vlcplayer = (VlcPlayer)m_vlcplayerModel.GetType().GetField(string.Format("vlc_player_{0}", pbBJ.TabIndex + 10)).GetValue(m_vlcplayerModel);
                            if (get_vlcplayer != null)
                            {
                                pbBJ.Invoke(new Action(() => {
                                    listlxrtspforfp.Add(get_vlcplayer);
                                    IntPtr render_wnd = pbBJ.Handle;
                                    get_vlcplayer.SetRenderWindow((int)render_wnd);
                                }));
                                get_vlcplayer.PlayFromURL("rtsp://" + IPC_USER + ":" + IPC_PWD + "@" + IPC_IP + ":" + IPC_PORT + "/h264/ch1/main/av_stream --rtsp-tcp");
                                //设置分屏播放时rtsp视频流视频画面的比例，使其填充完整个窗口
                                get_vlcplayer.SetRatio(sbl2);
                                String str = String.Format("分屏播放的车载摄像头名称：{0},IP地址为：{1},窗口索引为{2}", IPC_NAME, IPC_IP, cbb);
                                addMsg(str);
                             //   fp++;
                                break;

                            }
                        }
                        catch
                        {
                            //return false;
                        }

                        break;
                    }
                    else
                    {
                        try
                        {
                            int miuserid = ConnectCamera(IPC_IP, IPC_PORT, IPC_USER, IPC_PWD);
                            //CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = CreatePreview(pb.Handle);
                            pbBJ.Invoke(new Action(() => { lpPreviewInfofroBJ = CreatePreview(pbBJ.Handle); }));
                            CHCNetSDK.REALDATACALLBACK RealData = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack);//预览实时流回调函数
                            IntPtr pUser = new IntPtr();//用户数据
                            int error = -1;


                            pbBJ.Invoke(new Action(() =>
                            {
                                error = CHCNetSDK.NET_DVR_RealPlay_V40(miuserid, ref lpPreviewInfofroBJ, null/*RealData*/, pUser);
                               
                                liststrforfp.Add(error);
                            }));
                            m_lRealHandleModel.GetType().GetField(string.Format("m_lRealHandle{0}", pbBJ.Parent.TabIndex + 10)).SetValue(m_lRealHandleModel, error);
                            if (error < 0)
                            {
                              //  iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                               str = string.Format("综合实验平台:摄像机IP:{0} 播放窗口索引{2},错误码：{3}", IPC_NAME, error, 0, iLastErr); //登录失败，输出错误号
                               addMsg(str);
                                Thread.Sleep(100);
                              //  f4++;
                                break;
                            }
                            else
                            {    //alter by qhz
                               str = string.Format("综合实验平台:摄像机索引{0} 播放窗口索引{2}", IPC_NAME, /*i*/error, 0); //登录失败，输出错误号
                                addMsg(str);
                                Thread.Sleep(100);
                                break;
                            }

                        }
                        catch (Exception error)
                        {
                            WriteLog.Write(GetExceptionMsg(error, string.Empty));
                            throw error;
                            // return;
                        }

                    }
                    
                }
            }
            if (arr == "0")
            {
                createPicBox(4);
                Timerfp();
            }
        }
        private void frm3_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
        }
        

        /// <summary>
        /// 接收发送给本机ip对应端口号的数据报
        /// </summary>
        String IPBJ;
         void ReciveMsg()
        {
            while (true)
            {
                EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的IP和端口号
                byte[] buffer = new byte[1024];
                int length = Server.ReceiveFrom(buffer, ref point);//接收数据报
                String message = Encoding.UTF8.GetString(buffer, 0, length);
              // String[] ass= message.Split(':');
               // String ip = ass[0];
               // String port = ass[1];
              //  Console.WriteLine(point.ToString() + message);
                String[] ayy = point.ToString().Split(':');
               // String port = ayy[1];
                Console.WriteLine(ayy[0]);
                if(message!=null || message.Trim().ToString().Length!=0||message!=null){
                    this.Invoke(new Action(() => bjck(message)));
                }
                Console.WriteLine(ayy[1]);

                //UdpClient SendClient = new UdpClient();
                //IPAddress ipAddress =IPAddress.Parse(ayy[0]);
                //IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, int.Parse(ayy[1]));
                //String remoteIP = ayy[0];
                //byte[] bytes = System.Text.Encoding.UTF8.GetBytes("收到");
                //SendClient.Send(bytes, bytes.Length, ipEndPoint);


            }
        }

      
        private void preview_show(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }
    }
}

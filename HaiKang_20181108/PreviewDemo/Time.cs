using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PreviewDemo
{
    public partial class Time : Form
    {
        public Int32 m_lUserID = -1;
        private bool m_bInitSDK = false;
        private uint iLastErr = 0;
        private string strErr;
        public CHCNetSDK.NET_DVR_TIME m_struTimeCfg;
        public Action<string> SendEvent;
        public Time()
        {
            InitializeComponent();
        }

        private void Time_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void Init()
        {
            timer1.Start(); 
            getTree();
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
                    string IPC_JS= IniFile.ReadIniData(section, "IPC_JS", "");
                    string IPC_RTSP= IniFile.ReadIniData(section, "IPC_RTSP", "");
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    dict.Add("IPC_NAME", IPC_NAME);
                    dict.Add("IPC_IP", IPC_IP);
                    dict.Add("IPC_PORT", IPC_PORT);
                    dict.Add("IPC_USER", IPC_USER);
                    dict.Add("IPC_PWD", IPC_PWD);
                    dict.Add("IPC_JS", IPC_JS);
                    dict.Add("IPC_RTSP",IPC_RTSP);
                    listDict.Add(dict);
                }
            }
            TreeNode t1 = new TreeNode("综合实验平台");

            foreach (Dictionary<string, string> dict in listDict)
            {
                TreeNode t = new TreeNode();
                string IPC_NAME;
                dict.TryGetValue("IPC_NAME", out IPC_NAME);
                t.Text = IPC_NAME;
                t.Tag = dict;
                t1.Nodes.Add(t);
            }
            treeView1.Nodes.Add(t1);
            if (treeView1.Nodes.Count > 0)
                treeView1.Nodes[0].Expand();
            

        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                //更新子节点状态
                UpdateChildNodes(e.Node);
            }
        }

        private void UpdateChildNodes(TreeNode node)
        {
            foreach (TreeNode child in node.Nodes)
            {
                child.Checked = node.Checked;
                UpdateChildNodes(child);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbSystem.Text = System.DateTime.Now.ToString();  
        }

        private void button1_Click(object sender, EventArgs e)
        {
            setTimeButton(true);
        }

       /// <summary>
       /// 自动、手动校时
       /// </summary>
       /// <param name="auto"></param>
        private void setTimeButton(bool auto)
        {
                foreach (TreeNode n in treeView1.Nodes[0].Nodes)
                {
                    if (n.Checked)
                    {
                        ThreadPool.QueueUserWorkItem(
                         new WaitCallback(obj =>
                         {
                             Dictionary<string, string> d = n.Tag as Dictionary<string, string>;
                            // if (d["IPC_NAME"].ToString() != "行车摄像机1-1" && d["IPC_NAME"].ToString() != "客室1-1" && d["IPC_NAME"].ToString() != "客室1-2" && d["IPC_NAME"].ToString() != "客室1-3" && d["IPC_NAME"].ToString() != "客室1-4" && d["IPC_NAME"].ToString() != "客室2-1" && d["IPC_NAME"].ToString() != "客室2-2" && d["IPC_NAME"].ToString() != "客室2-3" && d["IPC_NAME"].ToString() != "客室2-4" && d["IPC_NAME"].ToString() != "客室3-1" && d["IPC_NAME"].ToString() != "客室3-2" && d["IPC_NAME"].ToString() != "客室3-3" && d["IPC_NAME"].ToString() != "客室3-4" && d["IPC_NAME"].ToString() != "客室4-1" && d["IPC_NAME"].ToString() != "客室4-2" && d["IPC_NAME"].ToString() != "客室4-3" && d["IPC_NAME"].ToString() != "客室4-4" && d["IPC_NAME"].ToString() != "客室5-1" && d["IPC_NAME"].ToString() != "客室5-2" && d["IPC_NAME"].ToString() != "客室5-3" && d["IPC_NAME"].ToString() != "客室5-4" && d["IPC_NAME"].ToString() != "客室6-1" && d["IPC_NAME"].ToString() != "客室6-2" && d["IPC_NAME"].ToString() != "客室6-3" && d["IPC_NAME"].ToString() != "客室6-4" && d["IPC_NAME"].ToString() != "行车摄像机2-1")
                            if(d["IPC_RTSP"].ToString()=="0")
                             {
                                 if (d["IPC_IP"].ToString() != "" && d["IPC_PORT"].ToString() != "" && d["IPC_USER"].ToString() != "" && d["IPC_PWD"].ToString() != "")
                                 {
                                     if (ConnectCamera(d["IPC_NAME"].ToString(), d["IPC_IP"].ToString(), d["IPC_PORT"].ToString(), d["IPC_USER"].ToString(), d["IPC_PWD"].ToString(), out m_lUserID))
                                         SetTime(auto, d["IPC_NAME"].ToString());//自动、手动校时
                                 }
                                 else
                                     SendEvent(string.Format("[综合实验平台_{0}_配置文件有错，其中IP、端口、用户名、密码不能为空]", d["IPC_NAME"].ToString()));
                             }
                             else {
                                 SendEvent(string.Format("[综合实验平台_{0}_校时无效，请检查是否为海康摄像头实时播放]", d["IPC_NAME"].ToString()));
                             
                             }
                             
                         }));
                    }
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
        private bool ConnectCamera(string Name,string IP, string PORT, string USER, string PWD, out Int32 m_lUserID)
        {
            try
            {
                CHCNetSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();

                //登录设备 Login the device
                m_lUserID = CHCNetSDK.NET_DVR_Login_V30(IP, int.Parse(PORT), USER, PWD, ref DeviceInfo);
                if (m_lUserID < 0)
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    strErr = string.Format("[综合实验平台_{0}]连接失败，错误码：{1}", Name, iLastErr); //登录失败，输出错误号            
                    SendEvent(strErr);
                    //MessageBox.Show(str);
                    return false;
                }
                else
                {
                    strErr = string.Format("[综合实验平台_{0}_{1}]连接成功",Name, IP);
                    SendEvent(strErr);
                    return true;
                    //登录成功
                    //MessageBox.Show("Login Success!");
                    //btnLogin.Text = "Logout";
                }
            }
            catch (Exception error)
            {
                SendEvent(GetExceptionMsg(error, string.Empty));
                throw error;
            }

        }

        /// <summary>
        /// 校时
        /// </summary>
        /// <param name="auto">是否自动</param>
        /// <param name="Name">设备名</param>
        /// <returns></returns>
        private string SetTime(bool auto,string Name)
        {
            try
            {
                m_struTimeCfg.dwYear = auto ? UInt32.Parse(DateTime.Now.Year.ToString()) : UInt32.Parse(dateTimePicker1.Value.Year.ToString());
                m_struTimeCfg.dwMonth = auto ? UInt32.Parse(DateTime.Now.Month.ToString()) : UInt32.Parse(dateTimePicker1.Value.Month.ToString());
                m_struTimeCfg.dwDay = auto ? UInt32.Parse(DateTime.Now.Day.ToString()) : UInt32.Parse(dateTimePicker1.Value.Day.ToString());
                m_struTimeCfg.dwHour = auto ? UInt32.Parse(DateTime.Now.Hour.ToString()) : UInt32.Parse(dateTimePicker2.Value.Hour.ToString());
                m_struTimeCfg.dwMinute = auto ? UInt32.Parse(DateTime.Now.Minute.ToString()) : UInt32.Parse(dateTimePicker2.Value.Minute.ToString());
                m_struTimeCfg.dwSecond = auto ? UInt32.Parse(DateTime.Now.Second.ToString()) : UInt32.Parse(dateTimePicker2.Value.Second.ToString());

                Int32 nSize = Marshal.SizeOf(m_struTimeCfg);
                IntPtr ptrTimeCfg = Marshal.AllocHGlobal(nSize);
                Marshal.StructureToPtr(m_struTimeCfg, ptrTimeCfg, false);

                if (!CHCNetSDK.NET_DVR_SetDVRConfig(m_lUserID, CHCNetSDK.NET_DVR_SET_TIMECFG, -1, ptrTimeCfg, (UInt32)nSize))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    strErr = string.Format("校时失败, 错误代码={0}", iLastErr);
                    SendEvent(string.Format("[综合实验平台_{0}]{1}", Name, strErr));
                    //设置时间失败，输出错误号 Failed to set the time of device and output the error code
                    //MessageBox.Show(strErr);
                }
                else
                {
                    strErr = string.Format("校时成功");
                    SendEvent(string.Format("[综合实验平台_{0}]{1}", Name, strErr));
                    //MessageBox.Show("校时成功！");
                }
                if (m_lUserID >= 0)
                    CHCNetSDK.NET_DVR_StopRealPlay(m_lUserID);
                Marshal.FreeHGlobal(ptrTimeCfg);
                return strErr;
            }
            catch (Exception error)
            {
                
                SendEvent(GetExceptionMsg(error, string.Empty));
                return GetExceptionMsg(error, string.Empty);
                throw error;
            }
               
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            setTimeButton(false);
        }

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
    }
}

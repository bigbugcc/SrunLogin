using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Configuration;
using YNNU_XYW.Entity;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Net;

namespace YNNU_XYW
{
    public partial class MainWindow : Window
    {
        private RestClient client;
        private readonly string url = "http://10.10.1.96";
        private string userIndex = "";
        private string userid = "";
        private string password = "";
        private bool btnStatus = false;
        private bool IsKeep = true;
        //实例化notifyIOC控件最小化托盘
        private NotifyIcon notifyIcon = null;
        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            initialTray();
            StatusBar();
            client = new RestClient(url);
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.55 Safari/537.36 Edg/96.0.1054.34";
            client.Timeout = 3000;
            Init();
            AutoLogin();
        }

        private void Init()
        {
            tb_account.Text = GetConfig("userid");
            tb_password.Text = GetConfig("password");
            cb_autoLogin.IsChecked = bool.Parse(GetConfig("autoLogin"));
            cb_remember.IsChecked = bool.Parse(GetConfig("remember"));
        }

        public void AutoLogin() {
            if (cb_autoLogin.IsChecked.Value)
            {
                Login(tb_account.Text, tb_password.Text);
            }
        }

        private bool GetOnlineStatus(bool IsShowDialog) {
            var req = new RestRequest("/eportal/InterFace.do?method=getOnlineUserInfo", Method.POST);
            req.AddParameter("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
            req.AddParameter("Accept", "*/*");
            req.AddParameter("Origin", url);
            req.AddParameter("Host", "10.10.1.96");
            req.AddParameter("Connection", "keep-alive");
            var rep = client.Execute(req);
            var status = JsonConvert.DeserializeObject<Respons>(rep.Content.ToString());

            if (!string.IsNullOrEmpty(status.userIndex))
            {
                userIndex = status.userIndex;
                if (IsShowDialog) {
                    var dialog = MessageBox.Show(
                        "用户名 ：" + status.userName + "\n" +
                        " 学号  ：" + status.userId + "\n" +
                        "IP地址 ：" + status.userIp + "\n\n" +
                        "操作：是否强制下线此用户？",
                        "当前已有账户登录!", MessageBoxButton.YesNo);
                    if (dialog == MessageBoxResult.Yes)
                    {
                        Logout(userIndex);
                    }
                }
                return true;
            }
            else {
                return false;
            }
        }

        private string GetMac() {

            if (!GetOnlineStatus(true))
            {
                var req = new RestRequest("/", Method.GET);
                //req.Timeout = 2000;
                req.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                req.AddHeader("Accept-Language", " zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
                var rep = client.Execute(req);
                if (rep.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(rep.Content))
                {
                    return rep.Content.ToString().Split('\'')[1].Split('?')[1];
                }
                else {
                    MessageBox.Show("未获取到服务信息！请重试");
                }
            }
            return "";
        }

        public void StatusBar() {
            Task.Factory.StartNew(async () =>
            {
                while (true) {
                    if (PingTest("10.10.1.96")!=null & PingTest("10.10.1.96").Status == IPStatus.Success)
                    {
                        if (PingTest("223.5.5.5").Status == IPStatus.Success)
                        {
                            LB_Status.Dispatcher.Invoke(() => { LB_Status.Foreground = new SolidColorBrush(Colors.Green); });
                            LB_StaText.Dispatcher.Invoke(() =>
                            {
                                LB_StaText.Foreground = new SolidColorBrush(Colors.Green);
                                LB_StaText.Content = "已连接网络！";
                            });
                        }
                        else {
                            LB_Status.Dispatcher.Invoke(() => { LB_Status.Foreground = new SolidColorBrush(Colors.Orange); });
                            LB_StaText.Dispatcher.Invoke(() => {
                                LB_StaText.Foreground = new SolidColorBrush(Colors.Orange);
                                LB_StaText.Content = "未连接网络！";
                            });
                        }
                        
                    }
                    else {
                        LB_Status.Dispatcher.Invoke(() => { LB_Status.Foreground= new SolidColorBrush(Colors.Red); });
                        LB_StaText.Dispatcher.Invoke(() => { 
                            LB_StaText.Foreground = new SolidColorBrush(Colors.Red);
                            LB_StaText.Content = "未找到验证服务器！";
                        });
                    }
                    await Task.Delay(2000);
                }
            });
        
        }

        private  void  Logout(string Index) {
            var req = new RestRequest("/eportal/InterFace.do?method=logout", Method.POST);
            req.AddParameter("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
            req.AddParameter("Accept", "*/*");
            req.AddParameter("Origin", url);
            req.AddParameter("Host", "10.10.1.96");
            req.AddParameter("Connection", "keep-alive");
            req.AddParameter("application/x-www-form-urlencoded; charset=UTF-8", "{\"userIndex:\"" + Index + "}", ParameterType.RequestBody);
            var rep = client.ExecuteAsync(req);

            var reps = JsonConvert.DeserializeObject<Respons>(rep.Result.Content.ToString());

            if (reps.result == "success")
            {
                if (btnStatus)
                {
                    ChangeStatus();
                }
                MessageBox.Show(reps.message);
            }
            else
            {
                MessageBox.Show("下线失败,请重试！");
            }
        }

        private bool Login(string userId, string password){
            if (PingTest("10.10.1.96") != null & PingTest("10.10.1.96").Status == IPStatus.Success)
            {
                var mac = GetMac();
                if (!string.IsNullOrEmpty(mac))
                {
                    var req = new RestRequest("/eportal/InterFace.do?method=login", Method.POST);
                    req.AddParameter("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
                    req.AddParameter("Accept", "*/*");
                    req.AddParameter("Origin", url);
                    req.AddParameter("userId", userId);
                    req.AddParameter("password", password);
                    req.AddParameter("passwordEncrypt", "false");
                    req.AddParameter("queryString", mac);
                    var rep = client.Execute(req);
                    var json = JsonConvert.DeserializeObject<Respons>(rep.Content.ToString());

                    if (rep.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(json.userIndex))
                    {
                        userIndex = json.userIndex;
                        ChangeStatus();
                        MessageBox.Show("登录成功！");
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("登录失败！");
                        return false;
                    }
                }
            }
            return false;
        }

        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            if (PingTest("10.10.1.96") != null & PingTest("10.10.1.96").Status == IPStatus.Success)
            {
                userid = tb_account.Text.Trim();
                password = tb_password.Text.Trim();

                if (!string.IsNullOrEmpty(userid) && !string.IsNullOrEmpty(password))
                {
                    if (btnStatus)
                    {
                        Logout(userIndex);
                        IsKeep = false;
                        return;
                    }
                    //保存密码
                    if (cb_remember.IsChecked.Value)
                    {
                        SetConfig("userid", userid);
                        SetConfig("password", password);
                    }

                    if (Login(userid, password))
                    {
                        KeepService();
                    }
                }
                else
                {
                    MessageBox.Show("请先填写账号密码！");
                }
            }
            else {
                MessageBox.Show("未找到验证服务器！");
            }
        }

        //测试网络状态
        public PingReply PingTest(string ip)
        {
            PingReply reply = null;
            Ping pingSender = null;
            try
            {
                pingSender = new Ping();

                PingOptions options = new PingOptions();
                options.DontFragment = true;
                string data = "hello world";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 1000;
                PingReply replyPing = pingSender.Send(ip, timeout, buffer, options);
                reply = replyPing;
            }
            catch (Exception ex)
            {
                reply = null;
            }
            finally
            {
                pingSender.Dispose();
            }
            return reply;
        }

        //保持在线
        private async void KeepService()
        {
            while (IsKeep)
            {
                var ba = new RestClient("https://www.baidu.com");
                ba.Timeout = 3000;
                ba.Execute(new RestRequest(Method.GET));
                if (!GetOnlineStatus(false))
                {
                    MessageBox.Show("已被抢占下线！");
                    ChangeStatus();
                    return;
                }
                await Task.Delay(300000);//执行周期5分钟
            }
        }

        private async void ChangeStatus(){
            if (btnStatus)
            {
                btn_login.Content = "登录";
            }
            else {
                btn_login.Content = "下线";
            }
            btnStatus = !btnStatus;
        }

        private string GetConfig(string key)
        {
            return ConfigurationManager.AppSettings[key].ToString().Trim();
        }

        private void SetConfig(string key, string value)
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings[key].Value = value;
            cfa.Save(ConfigurationSaveMode.Modified);
        }

        private void cb_autoLogin_Checked(object sender, RoutedEventArgs e)
        {
            SetConfig("autoLogin",cb_autoLogin.IsChecked.Value.ToString());
        }

        private void cb_remember_Checked(object sender, RoutedEventArgs e)
        {
            SetConfig("remember", cb_remember.IsChecked.Value.ToString());
        }

        // 最小化系统托盘
        private void initialTray()
        {
            //隐藏主窗体
            this.Visibility = Visibility.Visible;
            //设置托盘的各个属性
            notifyIcon = new NotifyIcon();
            notifyIcon.BalloonTipText = "校园网保持在线中...";//托盘气泡显示内容
            notifyIcon.Text = "YNNU-XYW";
            notifyIcon.Visible = true;//托盘按钮是否可见
            //重要提示：此处的图标图片在resouces文件夹。可是打包后安装发现无法获取路径，导致程序死机。建议复制一份resouces文件到UI层的bin目录下，确保万无一失。
            notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);

            notifyIcon.ShowBalloonTip(1000);//托盘气泡显示时间
            //双击事件
            //_notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseClick);
            //鼠标点击事件
            notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseClick);
            //右键菜单--退出菜单项
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("关闭");
            exit.Click += new EventHandler(exit_Click);
            //关联托盘控件
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { exit };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);
            //窗体状态改变时触发
            this.StateChanged += MainWindow_StateChanged;
        }


        // 托盘图标鼠标单击事件
        private void notifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //鼠标左键，实现窗体最小化隐藏或显示窗体
            if (e.Button == MouseButtons.Left)
            {
                if (this.Visibility == Visibility.Visible)
                {
                    this.Visibility = Visibility.Hidden;
                    //解决最小化到任务栏可以强行关闭程序的问题。
                    this.ShowInTaskbar = false;//使Form不在任务栏上显示
                }
                else
                {
                    this.Visibility = Visibility.Visible;
                    //this.WindowState = WindowState.Maximized;
                    //解决最小化到任务栏可以强行关闭程序的问题。
                    this.ShowInTaskbar = true;//使Form不在任务栏上显示
                    this.Activate();
                    this.Show();
                    this.WindowState = WindowState.Normal;
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                //object sender = new object();
                // EventArgs e = new EventArgs();
                exit_Click(sender, e);//触发单击退出事件
            }
        }

        // 窗体状态改变时候触发
        private void SysTray_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.Visibility = Visibility.Hidden;
            }
        }

        // 退出选项
        private void exit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定退出吗?",
                                               "YNNU-Network",
                                                MessageBoxButton.YesNo,
                                                MessageBoxImage.Question,
                                                MessageBoxResult.Yes) == MessageBoxResult.Yes)
            {
                Environment.Exit(0);
            }
        }


        // 窗口状态改变，最小化托盘
        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.Visibility = Visibility.Hidden;
            }
        }

    }
}

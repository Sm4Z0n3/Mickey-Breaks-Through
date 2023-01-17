using FGame.Main;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;
using System.Timers;
using System.Threading;

namespace FGame
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ad_grid.Margin = new Thickness(0,100,0,0);

            TimerStart();

            ScaleTransform flip = new ScaleTransform();
            flip.ScaleX = -1;
            Huowu.LayoutTransform = flip;
        }

        #region 定时器
        private DispatcherTimer cam;
        DispatcherTimer timer = new DispatcherTimer();
        private DispatcherTimer down;
        private DispatcherTimer up;
        private DispatcherTimer cent;
        private DispatcherTimer npc_cart;
        #endregion

        #region 声明
        Thickness margin;
        ini ini = new ini();
        #endregion

        #region 参数
        //得到初始分数
        int cent_i = 0;
        //运动速度 正常8
        int speed = 8;
        //世界重力 正常4
        int gravity = 4;
        //不知火舞对话触发
        int mess_huowu = 0;
        #endregion

        public void TimerStart()
        {
            cam = new DispatcherTimer();
            cam.Interval = TimeSpan.FromMilliseconds(100); //检测频率
            cam.Tick += camt;
            cam.Start();

            timer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Tick += 过关检测;
            timer.Start();

            down = new DispatcherTimer();
            down.Interval = TimeSpan.FromMilliseconds(1); //检测频率
            down.Tick += GravityDown;
            down.Start();

            cent = new DispatcherTimer();
            cent.Interval = TimeSpan.FromMilliseconds(1); //检测频率
            cent.Tick += GetCent;
            cent.Start();

            npc_cart = new DispatcherTimer();
            npc_cart.Interval = TimeSpan.FromMilliseconds(1); //检测频率
            npc_cart.Tick += Cart;
            npc_cart.Start();
        }

        private void GetCent(object sender, EventArgs e)
        {
            if(Player.Margin.Left + 162 >= Cent_0.Margin.Left && Cent_0.Margin.Top != 2000)
            {
                cent_i++;
                Cent_T.Content = "Cent：" + cent_i.ToString();
                Cent_0.Margin = new Thickness(Cent_0.Margin.Left, 2000, Cent_0.Margin.Right, Cent_0.Margin.Bottom);
            }

            if (Player.Margin.Left + 162 >= Cent_1.Margin.Left && Cent_1.Margin.Top != 2000)
            {
                cent_i++;
                Cent_T.Content = "Cent：" + cent_i.ToString();
                Cent_1.Margin = new Thickness(Cent_1.Margin.Left, 2000, Cent_1.Margin.Right, Cent_1.Margin.Bottom);
            }

            if (Player.Margin.Left + 162 >= Cent_2.Margin.Left && Cent_2.Margin.Top != 2000)
            {
                cent_i++;
                Cent_T.Content = "Cent：" + cent_i.ToString();
                Cent_2.Margin = new Thickness(Cent_2.Margin.Left, 2000, Cent_2.Margin.Right, Cent_2.Margin.Bottom);
            }

            if (Player.Margin.Left + 162 >= Huowu.Margin.Left && mess_huowu != 1)
            {
                MessageBox.Show("不知火舞：我只能说你妈死了", "触发对话");
                mess_huowu = 1;
            }

            if (Player.Margin.Left + 162 >= Zombie.Margin.Left)
            {
                MessageBox.Show("你死了，傻逼");
            }
        }
        
        private void Cart(object sender, EventArgs e)
        {
            Zombie.Margin = new Thickness (Zombie.Margin.Left + 6, Zombie.Margin.Top, Zombie.Margin.Right, Zombie.Margin.Bottom);
        }

            private void GravityDown(object sender, EventArgs e)
            {
            if (Player.Margin.Top < 472)
            {
                if (Player.Margin.Top <= 416 || Player.Margin.Top != 472)
                {
                    Player.Margin = new Thickness(Player.Margin.Left, Player.Margin.Top + gravity, Player.Margin.Right, Player.Margin.Bottom);
                }
            }

            }

        private void Jump(object sender, EventArgs e)
        {
            if (Player.Margin.Top >= 380)
            {
                Player.Margin = new Thickness(Player.Margin.Left, Player.Margin.Top - 160 / gravity, Player.Margin.Right, Player.Margin.Bottom);
            }
            else
            {
                down.Start();
                up.Stop();
            }
        }

        private void camt(object sender, EventArgs e)
        {
            if (Player.TranslatePoint(new Point(0, 0), this).X > this.ActualWidth / 2)
            {
                if(Map.Margin.Left != -2400)
                {
                    Map.Margin = new Thickness(Map.Margin.Left - 600, Map.Margin.Top, Map.Margin.Right, Map.Margin.Bottom);
                }
            }
        }

        private void 过关检测(object sender, EventArgs e)
        {
            if (Player.Margin.Left >= Map.Width - 400)
            {
                MessageBox.Show("恭喜通关，现在可以去殴打啥比米老鼠了");
                timer.Stop();
            }
        }


        #region 玩家动作
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            margin = Player.Margin;
            string state = "0";

            if (e.Key.ToString() == "A" || e.Key.ToString() == "")
            {
                if(Player.Margin.Left >= Gamep.Margin.Left + 10)
                {
                    margin.Left -= 1 * speed;
                    state = "A";
                    Player.Margin = margin;
                    ScaleTransform flip = new ScaleTransform();
                    flip.ScaleX = -1;
                    Player.LayoutTransform = flip;
                }
            }

            if (e.Key.ToString() == "D" || e.Key.ToString() == "d")
            {
                margin.Left += 1 * speed;
                state = "D";
                Player.Margin = margin;
                ScaleTransform flip = new ScaleTransform();
                flip.ScaleX = 1;
                Player.LayoutTransform = flip;
            }

            if (e.Key.ToString() == "W" || e.Key.ToString() == "w")
            {
                down.Stop();
                up = new DispatcherTimer();
                up.Interval = TimeSpan.FromMilliseconds(1); //检测频率
                up.Tick += Jump;
                up.Start();
                state = "W";
            }
            
            if (e.Key.ToString() == "S" || e.Key.ToString() == "w")
            {
                if (Player.Margin.Top< 472)
                {
                    margin.Top += 1 * speed;
                    Player.Margin = margin;
                    state = "W";
                }
            }

            if (e.Key == Key.Escape)
            {
                Thickness a = new Thickness(0, 2000, 0, 0);
                Gamep.Margin = a;
                a = new Thickness(0, 0, 0, 0);
                Homep.Margin = a;
            }
        }
        #endregion

        bool isDKeyLongPress = false;
       

        private void btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void btn_Play_Click(object sender, RoutedEventArgs e)
        {
            Thickness a = new Thickness(0, 2000, 0, 0);
            Homep.Margin = a;
            a = new Thickness(0, 0, 0, 0);
            Gamep.Margin = a;
            ScaleTransform flip = new ScaleTransform();
            flip.ScaleX = -1;
            Player.LayoutTransform = flip;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            down.Stop();
            up = new DispatcherTimer();
            up.Interval = TimeSpan.FromMilliseconds(1); //检测频率
            up.Tick += Jump;
            up.Start();
        }//↑

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            margin = Player.Margin;
            for (int i = 0; i < speed; i++)
            {
                margin.Top += 2;
                Player.Margin = margin;
            }
        }//↓

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            margin = Player.Margin;
            for (int i = 0; i < speed; i++)
            {
                margin.Left -= 2;
                Player.Margin = margin;
            }
            ScaleTransform flip = new ScaleTransform();
            flip.ScaleX = 1;
            Player.LayoutTransform = flip;
        }//←

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            margin = Player.Margin;
            for (int i = 0; i < speed; i++)
            {
                margin.Left += 2;
                Player.Margin = margin;
            }
        }//→

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            ad_grid.Visibility = Visibility.Hidden;
        }

        public void ReStart()
        {
            cent_i = 0;
            mess_huowu = 0;
            Cent_T.Content = "Cent：0";
            Player.Margin = new Thickness(92, 230, 0, 0);
            Cent_0.Margin = new Thickness(1026, 551, 0, 0);
            Cent_1.Margin = new Thickness(2547, 540, 0, 0);
            Cent_2.Margin = new Thickness(1467, 476, 0, 0);
        }

        private void btn_about_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("鱼形败类版权所有");
        }
    }
}

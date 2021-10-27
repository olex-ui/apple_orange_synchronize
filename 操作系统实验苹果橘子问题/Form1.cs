using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 操作系统实验苹果橘子问题
{
    public partial class Form1 : Form
    {

        public Semaphore plate = new Semaphore(2, 2);
        public Semaphore put_fruit = new Semaphore(1, 1);
        public Semaphore get_fruit = new Semaphore(1, 1);
        public Semaphore apple = new Semaphore(0, 2);
        public Semaphore orange = new Semaphore(0, 2);

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }



        private void Form1_Load(object sender, EventArgs e)
        {

            apple_left_picture.Visible = false;
            apple_right_picture.Visible = false;
            orange_left_picture.Visible = false;
            orange_right_picture.Visible = false;
        }


        private void Thread_father1()
        {
            while (true)
            {
                plate.WaitOne();       //等待盘子
                put_fruit.WaitOne();
                PMoveF1();
                put_fruit.Release();
                //Thread.Sleep(new Random().Next(500, 3000));    //随机休眠0.5~3秒
            }
        }

        private void PMoveF1()
        {
            if (orange_left_picture.Visible == false && apple_left_picture.Visible == false)        // 左侧可放苹果
            {
                Console.WriteLine("爸爸把苹果放在了盘子左侧...");
                textBox1.AppendText("爸爸把苹果放在了盘子左侧..." + "\r\n");
                int n = 20, x = 200, y = 44;
                for (int i = 0; i <= n; i++)
                {
                    fhand_picture.Location = new Point(x, y);
                    x += 6;
                    y += 5;
                    Thread.Sleep(50);
                }
                apple_left_picture.Visible = true;
                apple.Release();         //放了一个苹果,苹果信号量+1
                plate.Release();//释放资源
                fhand_picture.Image = Properties.Resources.放手;
                for (int i = 0; i <= n; i++)
                {
                    fhand_picture.Location = new Point(x, y);
                    x -= 6;
                    y -= 5;
                    Thread.Sleep(50);
                }
                fhand_picture.Image = Properties.Resources.苹果手;
            }
            else
            {
                Console.WriteLine("爸爸伸手到盘子左边放苹果，但盘子左边已有水果！");
                textBox1.AppendText("爸爸伸手到盘子左边放苹果，但盘子左边已有水果！" + "\r\n");
                plate.Release();
            }                
        }

        private void Thread_father2()
        {
            while (true)
            {
                plate.WaitOne();       //等待盘子
                put_fruit.WaitOne();
                PMoveF2();
                put_fruit.Release();
                Thread.Sleep(new Random().Next(500, 3000));     //随机休眠0.5~3秒
            }
        }

        private void PMoveF2()
        {
            if (orange_right_picture.Visible == false && apple_right_picture.Visible == false)
            {
                Console.WriteLine("爸爸把苹果放在了盘子右侧...");
                textBox1.AppendText("爸爸把苹果放在了盘子右侧..." + "\r\n");
                int n = 20, x = 200, y = 44;
                for (int i = 0; i <= n; i++)
                {
                    fhand_picture.Location = new Point(x, y);
                    x += 11;
                    y += 6;
                    Thread.Sleep(50);
                }

                //放好苹果，放手（空手回）
                apple_right_picture.Visible = true;
                apple.Release();
                plate.Release();//释放资源
                fhand_picture.Image = Properties.Resources.放手;
                for (int i = 0; i <= n; i++)
                {
                    fhand_picture.Location = new Point(x, y);
                    x -= 11;
                    y -= 6;
                    Thread.Sleep(50);
                }
                //最后还原手是苹果手
                fhand_picture.Image = Properties.Resources.苹果手;
            }
            else
            {
                Console.WriteLine("爸爸伸手到盘子右边放苹果，但盘子右边已有水果！");
                textBox1.AppendText("爸爸伸手到盘子右边放苹果，但盘子右边已有水果！\n" + "\r\n");
                plate.Release();//释放资源
            }
                
        }

        private void Thread_monther1()
        {
            while (true)
            {
                plate.WaitOne();       //等待盘子
                put_fruit.WaitOne();
                PMoveM1();
                put_fruit.Release();
                Thread.Sleep(new Random().Next(500, 3000));     //随机休眠0.5~3秒
            }
        }

        private void PMoveM1()
        {
            if (orange_right_picture.Visible == false && apple_right_picture.Visible == false)
            {
                Console.WriteLine("妈妈把橘子放在了盘子右侧...");
                textBox1.AppendText("妈妈把橘子放在了盘子右侧...\n" + "\r\n");
                int n = 20, a = 600, b = 44;
                for (int i = 0; i <= n; i++)
                {
                    mhand_picture.Location = new Point(a, b);
                    a -= 9;
                    b += 5;
                    Thread.Sleep(50);
                }
                mhand_picture.Image = Properties.Resources.放手;
                orange_right_picture.Visible = true;
                orange.Release();
                plate.Release();//释放资源
                for (int i = 0; i <= n; i++)
                {
                    mhand_picture.Location = new Point(a, b);
                    a += 9;
                    b -= 5;
                    Thread.Sleep(50);
                }
                mhand_picture.Image = Properties.Resources.橘子手;
            }
            else
            {
                int n = 17, a = 600, b = 44;
                for (int i = 0; i <= n; i++)
                {
                    mhand_picture.Location = new Point(a, b);
                    a -= 9;
                    b += 5;
                    Thread.Sleep(50);
                }
                Console.WriteLine("妈妈伸手到盘子右边放橘子，但盘子右边已有水果！");
                textBox1.AppendText("妈妈伸手到盘子右边放橘子，但盘子右边已有水果！\n" + "\r\n");
                if (apple_left_picture.Visible == false && orange_left_picture.Visible == false)
                {
                    orange_left_picture.Visible = true;
                    Console.WriteLine("妈妈把橘子放在了盘子左侧...");
                    textBox1.AppendText("妈妈把橘子放在了盘子左侧...\n" + "\r\n");
                    orange.Release();
                    mhand_picture.Image = Properties.Resources.放手;
                }
                for (int i = 0; i <= n; i++)
                {
                    mhand_picture.Location = new Point(a, b);
                    a += 9;
                    b -= 5;
                    Thread.Sleep(50);
                }
                mhand_picture.Image = Properties.Resources.橘子手;
                
                plate.Release();
            }
                
        }

        private void Thread_monther2()
        {
            while (true)
            {
                plate.WaitOne();       //等待盘子
                put_fruit.WaitOne();
                PMoveM2();
                put_fruit.Release();
                Thread.Sleep(new Random().Next(500, 3000));     //随机休眠0.5~3秒
            }
        }

        private void PMoveM2()
        {
            if (apple_left_picture.Visible == false && apple_left_picture.Visible == false)
            {
                Console.WriteLine("妈妈把橘子放在了盘子左侧...");
                textBox1.AppendText("妈妈把橘子放在了盘子左侧...\n" + "\r\n");
                int n = 20, a = 600, b = 44;
                for (int i = 0; i <= n; i++)
                {
                    mhand_picture.Location = new Point(a, b);
                    a -= 14;
                    b += 5;
                    Thread.Sleep(50);
                }
                mhand_picture.Image = Properties.Resources.放手;
                orange_left_picture.Visible = true;
                orange.Release();
                plate.Release();//释放资源
                for (int i = 0; i <= n; i++)
                {
                    mhand_picture.Location = new Point(a, b);
                    a += 14;
                    b -= 5;
                    Thread.Sleep(50);
                }
                mhand_picture.Image = Properties.Resources.橘子手;
            }
            else
            {
                int n = 17, a = 600, b = 44;
                for (int i = 0; i <= n; i++)
                {
                    mhand_picture.Location = new Point(a, b);
                    a -= 14;
                    b += 5;
                    Thread.Sleep(50);
                }             
                if (apple_right_picture.Visible == false && orange_right_picture.Visible == false)
                {
                    Console.WriteLine("妈妈把橘子放在了盘子右侧...");
                    textBox1.AppendText("妈妈把橘子放在了盘子右侧...\n" + "\r\n");
                    orange_right_picture.Visible = true;
                    orange.Release();
                    mhand_picture.Image = Properties.Resources.放手;
                }
                for (int i = 0; i <= n; i++)
                {
                    mhand_picture.Location = new Point(a, b);
                    a += 14;
                    b -= 5;
                    Thread.Sleep(50);
                }
                Console.WriteLine("妈妈伸手到盘子右边放橘子，但盘子右边已有水果！");
                textBox1.AppendText("妈妈伸手到盘子右边放橘子，但盘子右边已有水果！\n" + "\r\n");
                mhand_picture.Image = Properties.Resources.橘子手;
                plate.Release();
            }
                
        }



        private void Thread_son1()
        {
            while (true)
            {
                apple.WaitOne();
                get_fruit.WaitOne();
                PMoveS1();
                get_fruit.Release();
                Thread.Sleep(new Random().Next(500, 3000));
            }
        }

        private void PMoveS1()
        {
            if (apple_left_picture.Visible == true)
            {
                Console.WriteLine("儿子拿了左边的苹果...");
                textBox1.AppendText("儿子拿了左边的苹果...\n" + "\r\n");
                int a = 200, b = 304;
                for (int i = 0; i <= 20; i++)
                {
                    shand_picture.Location = new Point(a, b);
                    a += 6;
                    b -= 8;
                    Thread.Sleep(50);
                }
                apple_left_picture.Visible = false;

                shand_picture.Image = Properties.Resources.苹果手;
                for (int i = 0; i <= 20; i++)
                {
                    shand_picture.Location = new Point(a, b);
                    a -= 6;
                    b += 8;
                    Thread.Sleep(50);
                }
                //恢复默认拿手
                shand_picture.Image = Properties.Resources.拿手;
            }
            else
            {
                Console.WriteLine("儿子伸手到左边拿苹果，但左边盘子没有苹果!");
                textBox1.AppendText("儿子伸手到左边拿苹果，但左边盘子没有苹果!\n" + "\r\n");
                apple.Release();
            }
                
        }

        private void Thread_son2()
        {
            while (true)
            {
                apple.WaitOne();
                get_fruit.WaitOne();
                PMoveS2();
                get_fruit.Release();
                Thread.Sleep(new Random().Next(500, 3000));
            }
        }

        private void PMoveS2()
        {
            if (apple_right_picture.Visible == true)
            {
                Console.WriteLine("儿子拿了右边的苹果...");
                textBox1.AppendText("儿子拿了右边的苹果...\n" + "\r\n");
                int n = 20, a = 200, b = 304;
                for (int i = 0; i <= n; i++)
                {
                    shand_picture.Location = new Point(a, b);
                    a += 10;
                    b -= 8;
                    Thread.Sleep(50);
                }
                //儿子右苹果拿到，右苹果图片不可见
                apple_right_picture.Visible = false;
                //苹果手拿回苹果
                shand_picture.Image = Properties.Resources.苹果手;
                for (int i = 0; i <= n; i++)
                {
                    shand_picture.Location = new Point(a, b);
                    a -= 10;
                    b += 8;
                    Thread.Sleep(50);
                }
                //恢复默认拿手
                shand_picture.Image = Properties.Resources.拿手;
            }
            else
            {
                Console.WriteLine("儿子伸手到左边拿苹果，但左边盘子没有苹果!");
                textBox1.AppendText("儿子伸手到左边拿苹果，但左边盘子没有苹果!\n" + "\r\n");
                apple.Release();
            }
                
        }

        private void Thread_daughter1()
        {
            while (true)
            {
                orange.WaitOne();
                get_fruit.WaitOne();
                PMoveD1();
                get_fruit.Release();
                Thread.Sleep(new Random().Next(100, 1000));
            }
        }

        private void PMoveD1()
        {
            if (orange_left_picture.Visible == true)
            {
                Console.WriteLine("女儿拿了左边的橘子...");
                textBox1.AppendText("女儿拿了左边的橘子...\n" + "\r\n");
                int n = 20, x = 600, y = 304;
                for (int i = 0; i <= n; i++)
                {
                    dhand_picture.Location = new Point(x, y);
                    x -= 14;
                    y -= 8;
                    Thread.Sleep(50);
                }
                orange_left_picture.Visible = false;
                dhand_picture.Image = Properties.Resources.橘子手;
                for (int i = 0; i <= n; i++)
                {
                    dhand_picture.Location = new Point(x, y);
                    x += 14;
                    y += 8;
                    Thread.Sleep(50);
                }
                dhand_picture.Image = Properties.Resources.拿手;
            }
            else
            {
                Console.WriteLine("女儿伸手到左边拿橘子，但左边盘子没有橘子!");
                textBox1.AppendText("女儿伸手到左边拿橘子，但左边盘子没有橘子!\n" + "\r\n");
                orange.Release();
            }
                
        }

        private void Thread_daughter2()
        {
            while (true)
            {
                orange.WaitOne();
                get_fruit.WaitOne();
                PMoveD2();
                get_fruit.Release();
                Thread.Sleep(new Random().Next(500, 3000));
            }
        }

        private void PMoveD2()
        {
            if (orange_right_picture.Visible == true)
            {
                Console.WriteLine("女儿拿了右边的橘子...");
                textBox1.AppendText("女儿拿了右边的橘子...\n" + "\r\n");
                int n = 20, x = 600, y = 304;
                for (int i = 0; i <= n; i++)
                {
                    dhand_picture.Location = new Point(x, y);
                    x -= 9;
                    y -= 8;
                    Thread.Sleep(50);
                }
                orange_right_picture.Visible = false;
                dhand_picture.Image = Properties.Resources.橘子手;
                for (int i = 0; i <= n; i++)
                {
                    dhand_picture.Location = new Point(x, y);
                    x += 9;
                    y += 8;
                    Thread.Sleep(50);
                }
                dhand_picture.Image = Properties.Resources.拿手;
            }
            else
            {
                Console.WriteLine("女儿伸手到右边拿橘子，但右边盘子没有橘子!");
                textBox1.AppendText("女儿伸手到右边拿橘子，但右边盘子没有橘子!\n" + "\r\n");
                orange.Release();
            }
                
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);

        }

        private void btn_begin_Click(object sender, EventArgs e)
        {

            Thread father1 = new Thread(new ThreadStart(Thread_father1));
            Thread father2 = new Thread(new ThreadStart(Thread_father2));
            Thread monther1 = new Thread(new ThreadStart(Thread_monther1));
            Thread monther2 = new Thread(new ThreadStart(Thread_monther2));
            Thread son1 = new Thread(new ThreadStart(Thread_son1));
            Thread son2 = new Thread(new ThreadStart(Thread_son2));
            Thread daughter1 = new Thread(new ThreadStart(Thread_daughter1));
            Thread daughter2 = new Thread(new ThreadStart(Thread_daughter2));
            father1.Start();
            father2.Start();
            monther1.Start();
            monther2.Start();
            son1.Start();
            son2.Start();
            daughter1.Start();
            daughter2.Start();
        }

        private void btn_end_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void father_picture_Click(object sender, EventArgs e)
        {

        }

        private void son_picture_Click(object sender, EventArgs e)
        {

        }

        private void daughter_picture_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
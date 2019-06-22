using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyURL
{
    /// <summary>
    /// Login.xaml の相互作用ロジック
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            CheckPassword();
            InitializeComponent();

            this.passwordBox1.Focus();
        }

        private void CheckPassword()
        {
            if (File.Exists(@"const.ini"))
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string pwd1 = passwordBox1.Password;
            string pwd2 = passwordBox2.Password;

            if (pwd1 == pwd2 && pwd1.Length != 0)
            {
                String stBuffer = "";
                stBuffer = Des.DESEncrypt(pwd1);

                System.IO.StreamWriter sw = (new System.IO.StreamWriter(@"const.ini", false));
                sw.WriteLine(stBuffer);
                sw.Close();

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }

        private void passwordBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); //触发设定按钮点击事件
            }
        }

        private void passwordBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); //触发设定按钮点击事件
            }
        }
    }
}

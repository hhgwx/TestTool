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
using System.Windows.Shapes;

namespace MyURL
{
    /// <summary>
    /// ChangePassword.xaml の相互作用ロジック
    /// </summary>
    public partial class ChangePassword : Window
    {
        public string password { get; set; }
        public Boolean OKFlg = false;

        public ChangePassword()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string pwd_old = passwordBox_old.Password;
            string pwd_new1 = passwordBox_new1.Password;
            string pwd_new2 = passwordBox_new2.Password;

            if (password != pwd_old)
            {
                MessageBox.Show("就密码不对！");
                this.passwordBox_old.Clear();
                this.passwordBox_old.Focus();
                return;
            }

            if (pwd_new1 == pwd_new2 && pwd_new1.Length != 0)
            {
                password = pwd_new1;
                OKFlg = true;
                this.Close();
            } else
            {
                MessageBox.Show("两次新密码不一致 请确认！");
                this.passwordBox_new1.Clear();
                this.passwordBox_new2.Clear();
                this.passwordBox_new1.Focus();
                return;
            }
        }
    }
}

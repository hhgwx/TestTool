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
    /// safetyPassword.xaml の相互作用ロジック
    /// </summary>
    public partial class safetyPassword : Window
    {
        public string password = ""; //父画面传来的Password
        public Boolean OKFlg = false; //父画面返回判断用Flg

        public safetyPassword()
        {
            InitializeComponent();

            this.passwordBox1.Focus();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (password == this.passwordBox1.Password)
            {
                OKFlg = true;
                this.Close();
            } else
            {
                MessageBox.Show("密码不对请重新输入！");
                this.passwordBox1.Password = "";
                this.passwordBox1.Focus();
            }
        }

        private void passwordBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); //触发设定按钮点击事件
            }
        }
    }
}

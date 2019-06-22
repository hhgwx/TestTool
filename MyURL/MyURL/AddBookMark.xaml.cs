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
    /// AddBookMark.xaml の相互作用ロジック
    /// </summary>
    public partial class AddBookMark : Window
    {
        Boolean button_click_flag = false;

        public AddBookMark()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (this.textBox_ShuoMing.Text == "" || this.textBox_Url.Text == "")
            {
                MessageBox.Show("[说明]和[URL]不可以为空");
            }
            else
            {
                button_click_flag = true;
                this.Close();
            }

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (!button_click_flag) { 
                this.textBox_ShuoMing.Text = "";
                this.textBox_Url.Text = "";
                this.textBox_User.Text = "";
                this.textBox_Psw.Text = "";
            }
        }

        private void textBox_ShuoMing_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); //触发设定按钮点击事件
            }
        }

        private void textBox_Url_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); //触发设定按钮点击事件
            }
        }

        private void textBox_User_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); //触发设定按钮点击事件
            }
        }

        private void textBox_Psw_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); //触发设定按钮点击事件
            }
        }
    }
}

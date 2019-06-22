using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Configuration;

namespace PicManager
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {

        System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
        //String defaultfilePath = "";

        public MainWindow()
        {
            InitializeComponent();

            //this.DataContext = new TestViewMode();

            //this.comboBox_win1_percent
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (this.tabItem1.picMergeThread != null && this.tabItem1.picMergeThread.IsAlive)
            {
                this.tabItem1.picMergeThread.Abort();
            }
            if (this.tabItem1.statusUPdateThread != null && this.tabItem1.statusUPdateThread.IsAlive)
            {
                this.tabItem1.statusUPdateThread.Abort();
            }

            if (this.tabItem2.getPicListThread != null && this.tabItem2.getPicListThread.IsAlive)
            {
                this.tabItem2.getPicListThread.Abort();
            }
            if (this.tabItem2.getPicComThread != null && this.tabItem2.getPicComThread.IsAlive)
            {
                this.tabItem2.getPicComThread.Abort();
            }
        }
        /*
       private void button_Click(object sender, RoutedEventArgs e)
       {
           //Button buttonTemp = sender as Button;
           //MessageBox.Show(buttonTemp.Content.ToString());
           string tempString = string.Format("Sender:{2}\nOriginalSource:{0}\nSource:{1}",
               e.OriginalSource.GetType().FullName,
               e.Source.GetType().FullName,
               sender.GetType().FullName);
           //MessageBox.Show(tempString);
           ((TestViewMode)this.DataContext).TestBinding = ((Button)e.OriginalSource).Content.ToString();
       }

       public class BaseViewModel:INotifyPropertyChanged
       {
           public event PropertyChangedEventHandler PropertyChanged;

           public void OnPropertyChanged(string name)
           {
               if (PropertyChanged !=null)
               {
                   PropertyChanged(this,new PropertyChangedEventArgs(name));
               }
           }
       }

       public class TestViewMode:BaseViewModel
       {
           string name = "default";
           public string TestBinding
           {
               get
               {
                   return this.name;
               }

               set
               {
                   this.name = value;
                   this.OnPropertyChanged("TestBinding");
               }
           }
       }*/
    }
}

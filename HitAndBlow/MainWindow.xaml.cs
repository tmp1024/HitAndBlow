using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace HitAndBlow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HitAndBlowData data;

        public MainWindow()
        {
            InitializeComponent();

            // Create answer
            data = new HitAndBlowData();
            // Binding input data
            TextVal1.DataContext = data;
            TextVal2.DataContext = data;
            TextVal3.DataContext = data;
            TextVal4.DataContext = data;
            TextLog.DataContext = data;
        }

        private void Number_OnClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            data.InputData((string)button.Content);
        }
    }

    class HitAndBlowData : INotifyPropertyChanged
    {
        private string ans;
        private string val1;
        private string val2;
        private string val3;
        private string val4;
        private int digit;
        private int count;
        private string log;

        public event PropertyChangedEventHandler PropertyChanged;

        public HitAndBlowData()
        {
            ans = CreateAnswer();
            val1 = "*";
            val2 = "*";
            val3 = "*";
            val4 = "*";
            digit = 0;
            count = 0;
            log = "";
        }

        private string CreateAnswer()
        {
            int[] possibily = Enumerable.Range(0, 9).ToArray();
            possibily = possibily.OrderBy(i => Guid.NewGuid()).ToArray();
            return possibily[0].ToString() + possibily[1].ToString() + possibily[2].ToString() + possibily[3].ToString();
        }

        public void InputData(string input)
        {
            switch (input)
            {
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    InsertData(input);
                    break;
                case "BS":
                    DeleteData();
                    break;
                case "RES":
                    ResetData();
                    break;
            }
        }

        private void InsertData(string value)
        {
            if (val1 == value || val2 == value || val3 == value || val4 == value)
            {
                MessageBox.Show("Don't use same number", "Error");
                return;
            }

            if (val1 == "*")
            {
                Val1 = value;
            }
            else if (val2 == "*")
            {
                Val2 = value;
            }
            else if (val3 == "*")
            {
                Val3 = value;
            }
            else if (val4 == "*")
            {
                Val4 = value;
            }
            digit++;
            count++;

            if (digit == 4)
            {
                Solve();
            }
        }

        private void DeleteData()
        {
            if (val3 != "*")
            {
                Val3 = "*";
            }
            else if (val2 != "*")
            {
                Val2 = "*";
            }
            else if (val1 != "*")
            {
                Val1 = "*";
            }
            digit = digit == 0 ? 0 : digit - 1;
        }

        private void ResetData()
        {
            ans = CreateAnswer();
            Val1 = "*";
            Val2 = "*";
            Val3 = "*";
            Val4 = "*";
            digit = 0;
            count = 0;
            Log = "";
        }

        private void Solve()
        {
            int hit = 0;
            int blow = 0;
            string val = val1 + val2 + val3 + val4;
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    if (ans[i] != val[j]) continue;
                    if (i == j)
                    {
                        hit++;
                    }
                    else
                    {
                        blow++;
                    }
                }
            }

            WriteLog(hit, blow);

            Judge(hit, blow);
        }

        private void WriteLog(int hit, int blow)
        {
            if (log == "")
            {
                Log = string.Format("Hit: {0}, Blow: {1}", hit, blow);
            }
            else
            {
                Log = string.Format("Hit: {0}, Blow: {1}", hit, blow) + Environment.NewLine + log;
            }
        }

        private void Judge(int hit, int blow)
        {
            if (hit == 4)
            {
                Success();
            }
            else
            {
                Failure();
            }
        }

        private void Success()
        {
            if (count == 1)
            {
                MessageBox.Show("Success!" + Environment.NewLine + string.Format("You successed in {0} time.", count), "Success");
            }
            else
            {
                MessageBox.Show("Success!" + Environment.NewLine + string.Format("You successed in {0} times.", count), "Success");
            }
            ResetData();
        }

        private void Failure()
        {
            Val1 = "*";
            Val2 = "*";
            Val3 = "*";
            Val4 = "*";
            digit = 0;
        }

        public string Val1
        {
            get { return val1; }
            set
            {
                val1 = value;
                NotifiyPropertyChanged("Val1");
            }
        }

        public string Val2
        {
            get { return val2; }
            set
            {
                val2 = value;
                NotifiyPropertyChanged("Val2");
            }
        }

        public string Val3
        {
            get { return val3; }
            set
            {
                val3 = value;
                NotifiyPropertyChanged("Val3");
            }
        }

        public string Val4
        {
            get { return val4; }
            set
            {
                val4 = value;
                NotifiyPropertyChanged("Val4");
            }
        }

        public string Log
        {
            get { return log; }
            set
            {
                log = value;
                NotifiyPropertyChanged("Log");
            }
        }

        private void NotifiyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

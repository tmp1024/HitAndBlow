using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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

        // Click Number Button
        private void Number_OnClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            data.InputData((string)button.Content);
        }
    }

    internal class HitAndBlowData : INotifyPropertyChanged
    {
        private string _ans;
        private string _val1;
        private string _val2;
        private string _val3;
        private string _val4;
        private int _digit;
        private int _count;
        private string _log;

        public event PropertyChangedEventHandler PropertyChanged;

        public HitAndBlowData()
        {
            // Initialize
            _ans = CreateAnswer();
            _val1 = "*";
            _val2 = "*";
            _val3 = "*";
            _val4 = "*";
            _digit = 0;
            _count = 0;
            _log = "";
        }

        private string CreateAnswer()
        {
            // Create array 0..9
            int[] possibily = Enumerable.Range(0, 9).ToArray();
            // Shuffle
            possibily = possibily.OrderBy(i => Guid.NewGuid()).ToArray();
            return possibily[0] + possibily[1].ToString() + possibily[2] + possibily[3];
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
            // Check same number
            if (_val1 == value || _val2 == value || _val3 == value || _val4 == value)
            {
                MessageBox.Show("Don't use same number", "Error");
                return;
            }

            if (_val1 == "*")
            {
                Val1 = value;
            }
            else if (_val2 == "*")
            {
                Val2 = value;
            }
            else if (_val3 == "*")
            {
                Val3 = value;
            }
            else if (_val4 == "*")
            {
                Val4 = value;
            }
            _digit++;
            _count++;

            if (_digit == 4)
            {
                Solve();
            }
        }

        private void DeleteData()
        {
            if (_val3 != "*")
            {
                Val3 = "*";
            }
            else if (_val2 != "*")
            {
                Val2 = "*";
            }
            else if (_val1 != "*")
            {
                Val1 = "*";
            }
            _digit = _digit == 0 ? 0 : _digit - 1;
        }

        private void ResetData()
        {
            _ans = CreateAnswer();
            Val1 = "*";
            Val2 = "*";
            Val3 = "*";
            Val4 = "*";
            _digit = 0;
            _count = 0;
            Log = "";
        }

        private void Solve()
        {
            int hit = 0;
            int blow = 0;
            string val = _val1 + _val2 + _val3 + _val4;
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    if (_ans[i] != val[j]) continue;
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
            // Create first log
            if (_log == "")
            {
                Log = string.Format("Hit: {0}, Blow: {1}", hit, blow);
            }
            else
            {
                Log = string.Format("Hit: {0}, Blow: {1}", hit, blow) + Environment.NewLine + _log;
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
            if (_count == 1)
            {
                MessageBox.Show("Success!" + Environment.NewLine + string.Format("You successed in {0} time.", _count), "Success");
            }
            else
            {
                MessageBox.Show("Success!" + Environment.NewLine + string.Format("You successed in {0} times.", _count), "Success");
            }
            ResetData();
        }

        private void Failure()
        {
            Val1 = "*";
            Val2 = "*";
            Val3 = "*";
            Val4 = "*";
            _digit = 0;
        }

        public string Val1
        {
            get { return _val1; }
            set
            {
                _val1 = value;
                NotifiyPropertyChanged("Val1");
            }
        }

        public string Val2
        {
            get { return _val2; }
            set
            {
                _val2 = value;
                NotifiyPropertyChanged("Val2");
            }
        }

        public string Val3
        {
            get { return _val3; }
            set
            {
                _val3 = value;
                NotifiyPropertyChanged("Val3");
            }
        }

        public string Val4
        {
            get { return _val4; }
            set
            {
                _val4 = value;
                NotifiyPropertyChanged("Val4");
            }
        }

        public string Log
        {
            get { return _log; }
            set
            {
                _log = value;
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

using NPOI.Util;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace BasicTest
{
    /// <summary>
    /// LINQDemo1.xaml 的交互逻辑
    /// </summary>
    public partial class LINQDemo1 : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private ObservableCollection<int> _myItems;
        public ObservableCollection<int> MyItems
        {
            get { return _myItems; }
            set
            {
                _myItems = value;
                OnPropertyChanged(nameof(MyItems));
            }
        }

        public LINQDemo1()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void LINQDemo1_Loaded(object sender, RoutedEventArgs e)
        {
            var arr = LINQTest_QuerySyntax();
            MyItems = new ObservableCollection<int>(arr);
        }

        private IEnumerable<int> LINQTest()
        {
            var rnd = new Random(1334);
            var arr = Enumerable.Range(0, 200).Select(_=>rnd.Next(20));
            
            var dic = new Dictionary<int, int>();
            foreach (var item in arr)
            {
                if (dic.TryGetValue(item, out int count))
                {
                    dic[item] = count + 1;
                }
                else
                {
                    dic[item] = 1;
                }
            }
            return dic.Values;
        }

        private IEnumerable<int> LINQTest_MethodChain()
        {
            var rnd = new Random(1334);
            var dic = Enumerable.Range(0, 200)
                .Select(_ => rnd.Next(20))
                .GroupBy(x => x)
                .ToDictionary(g => g.Key, g => g.Count());
            return dic.Values;
        }

        private IEnumerable<int> LINQTest_QuerySyntax()
        {
            var rnd = new Random(1334);
            var arr = Enumerable.Range(0, 200).Select(_ => rnd.Next(20));
            var query =
                from n in arr
                group n by n into g
                select new { Number = g.Key, Count = g.Count() };

            var dic = query.ToDictionary(x => x.Number, x => x.Count);
            return dic.Values;
        }
    }
}

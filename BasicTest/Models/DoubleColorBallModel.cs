using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BasicTest
{
    /// <summary>
    /// 双色球模型类
    /// </summary>
    public class DoubleColorBallModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> Reds { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> Blues { get; } = new ObservableCollection<string>();

        public DoubleColorBallModel()
        {
            // 初始化6个红球和1个蓝球
            for (int i = 1; i <= 6; i++)
                Reds.Add(i.ToString("D2"));
            Blues.Add("01");
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
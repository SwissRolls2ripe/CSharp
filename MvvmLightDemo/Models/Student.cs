using System.ComponentModel;

namespace MvvmLightDemo.Models
{
    class Student : INotifyPropertyChanged
    {
        private string _name;
        private string _grade;
        public string StudentName
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(StudentName));
                }
            }
        }
        public string StudentGrade
        {
            get => _grade;
            set
            {
                if (_grade != value)
                {
                    _grade = value;
                    OnPropertyChanged(nameof(StudentGrade));
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

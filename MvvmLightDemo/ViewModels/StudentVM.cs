using MvvmLightDemo.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MvvmLightDemo.ViewModels
{
    class StudentVM
    {
        public ObservableCollection<Student> Students { get; set; }
        public Student OneStudent { get; set; }
        public ICommand AddStudentCommand { get; }
        public Student? SelectedStudent { get; set; }
        public ICommand RemoveStudentCommand { get; }
        public StudentVM()
        {
            OneStudent = new Student();
            Students = [];
            AddStudentCommand = new RelayCommand(AddStudent);
            RemoveStudentCommand = new RelayCommand(RemoveStudent, () => SelectedStudent != null);
        }

        private void RemoveStudent()
        {
            if (SelectedStudent == null) return; // 如果没有选中学生，则不执行删除操作
            Students.Remove(SelectedStudent);
        }

        public void AddStudent()
        {
            if (Students.Any(s => s.StudentName == OneStudent.StudentName))
            {
                return; // 如果学生姓名已存在，则不添加
            }
            Students.Add(new Student
            {
                StudentName = OneStudent.StudentName,
                StudentGrade = OneStudent.StudentGrade
            });
        }
    }
}

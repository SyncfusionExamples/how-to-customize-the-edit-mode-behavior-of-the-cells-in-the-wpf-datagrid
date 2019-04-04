using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGrid
{
    public class Student:INotifyPropertyChanged
    {
        public Student(int id, string nam, double salary, double gpa)
        {
            Id = id;
            Name = nam;
            Salary = salary;
            GradePointAverage = gpa;
        }
        public Student() { }
        public int Id { get; set; }
        private double salary;
        public string Name { get; set; }

        public double Salary 
        { 
            get
            {
                return salary;
            }
            
            set
            {
                salary = value;
                OnPropertyChanged("Salary");
            }
        
        }

        public double GradePointAverage { get; set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}

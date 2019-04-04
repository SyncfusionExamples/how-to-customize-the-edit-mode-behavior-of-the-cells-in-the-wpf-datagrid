using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGrid
{
    public class StudentCollection
    {
        public StudentCollection()
        {

            Students = new ObservableCollection<Student>
            {
                new Student(1, "Joe Smith ", 3412.45, 42.6),
                new Student(2, "John Doe", 1223.45, 56.8),
                new Student(3, "Mary Brown", 12234.6, 36.5),
                new Student(4, "Joe Smith ", 34535.4, 42.6),
                new Student(5, "John Doe", 12236.6, 56.8),
                new Student(6, "Mary Brown", 12455.6, 36.5)
            };
        }

        public ObservableCollection<Student> Students { get;  set; }
    }
}

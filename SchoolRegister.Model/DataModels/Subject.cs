using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolRegister.Model.DataModels
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<SubjectGroup> SubjectGroups { get; set; }
        public Teacher Teacher { get; set; }
        [ForeignKey("Teacher")]
        public int? TeacherId { get; set; }
        public IList<Grade> Grades { get; set; }

        public Subject()
        {
            SubjectGroups = new List<SubjectGroup>();
            Grades = new List<Grade>();
        }
    }
}

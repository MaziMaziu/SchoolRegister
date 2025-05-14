using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolRegister.Model.DataModels
{
    public class Student : User
    {
        public Group Group { get; set; }
        public int? GroupId { get; set; }
        public IList<Grade> Grades { get; set; }
        public Parent Parent { get; set; }
        public int? ParentId { get; set; }

        public double AverageGrade
        {
            get
            {
                if (Grades == null || !Grades.Any())
                {
                    return 0.0;
                }
                return Grades.Average(g => (double)g.GradeValue);
            }
        }

        public IDictionary<string, double> AverageGradePerSubject
        {
            get
            {
                if (Grades == null || !Grades.Any())
                {
                    return new Dictionary<string, double>();
                }

                return Grades
                    .Where(g => g.Subject != null && !string.IsNullOrEmpty(g.Subject.Name))
                    .GroupBy(g => g.Subject.Name)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Average(grade => (double)grade.GradeValue)
                    );
            }
        }

        public IDictionary<string, List<GradeScale>> GradesPerSubject
        {
            get
            {
                if (Grades == null || !Grades.Any())
                {
                    return new Dictionary<string, List<GradeScale>>();
                }

                return Grades
                    .Where(g => g.Subject != null && !string.IsNullOrEmpty(g.Subject.Name))
                    .GroupBy(g => g.Subject.Name)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(grade => grade.GradeValue).ToList()
                    );
            }
        }

        public Student()
        {
            Grades = new List<Grade>();
        }
    }
}
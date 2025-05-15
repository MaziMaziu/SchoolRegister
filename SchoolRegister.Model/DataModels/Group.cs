using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolRegister.Model.DataModels
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Students { get; set; } // <- zmień na User
        public IList<SubjectGroup> SubjectGroups { get; set; }
        public Group() { }
    }

}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SchoolRegister.Model.DataModels
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegistrationDate { get; set; }

        // Dodaj te właściwości:
        public int? GroupId { get; set; }
        public Group Group { get; set; }

        public int? ParentId { get; set; }
        public Parent Parent { get; set; }
    }

}
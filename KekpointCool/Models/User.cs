using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KekpointCool.Models
{
    public class User
    {
        public Guid? ID { get; set;}
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string LastName { get; set; }
        public string AccessLevel { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkillMService.Models
{
    public class Employee
    {
        public Employee(string name)
        {
            this.name = name;
        }

        public string id { get; set; }
        public string name { get; set; }
        public int location { get; set; }
       
    }
}
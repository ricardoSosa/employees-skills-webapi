using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkillMService.Models;


namespace SkillMService.Results
{
    public class SkillEmployeesResult
    {
        public string id { get; set; }
        public string name { get; set; }

        public List<Employee> JrEmployees { get; set; }
        public List<Employee> IntEmployees { get; set; }
        public List<Employee> SrSEmployees { get; set; }
        public List<Employee> LdSEmployees { get; set; }
    }
}
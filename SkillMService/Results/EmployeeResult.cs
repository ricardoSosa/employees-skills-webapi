using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkillMService.Models;

namespace SkillMService.Results
{
    public class EmployeeResult
    {

        public EmployeeResult(List<Employee> listEmployees, byte status)
        {
            this.employees = listEmployees;
            this.Status = status;
        }
        public List<Employee> employees { get; set; }
        public byte Status { get; set; }
       
    }
}
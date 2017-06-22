using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkillMService.Models
{
    public class EmployeeSkillViewModel
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public int LOCATION { get; set; }
        List<Skill> JrSkills { get; set; }
        List<Skill> IntSkills { get; set; }
        List<Skill> SrSkills { get; set; }
        List<Skill> LdSkills { get; set; }
    }
}
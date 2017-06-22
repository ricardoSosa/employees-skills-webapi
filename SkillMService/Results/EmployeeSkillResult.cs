using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkillMService.Models;

namespace SkillMService.Results
{
    public class EmployeeSkillResult
    {
        public EmployeeSkillResult() { }

        public EmployeeSkillResult( string id, 
                                    string name, 
                                    int location,
                                    List<Skill> jrskills,
                                    List<Skill> intskills,
                                    List<Skill> srskills,
                                    List<Skill> ldskills
                                        )
        {
            this.ID = id;
            this.NAME = name;
            this.JrSkills = jrskills;
            this.SrSkills = srskills;
            this.LdSkills = ldskills;
        }

        public string ID { get; set; }
        public string NAME { get; set; }
        public int LOCATION { get; set; }
        public List<Skill> JrSkills { get; set; }
        public List<Skill> IntSkills { get; set; }
        public List<Skill> SrSkills { get; set; }
        public List<Skill> LdSkills { get; set; }


        public byte Status { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkillMService.Models
{
    public class SkillGroupSkillVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        List<Skill> Skills { get;  set; }
    }
}
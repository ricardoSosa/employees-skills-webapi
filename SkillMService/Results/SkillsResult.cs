using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkillMService.Models;

namespace SkillMService.Results
{
    public class SkillsResult
    {
        public SkillsResult(List<Skill> skilllist, byte status)
        {
            this.skillList = skilllist;
            this.status = status;
        }

        public List<Skill> skillList { get; set; }
        public byte status { get; set; }
    }
}
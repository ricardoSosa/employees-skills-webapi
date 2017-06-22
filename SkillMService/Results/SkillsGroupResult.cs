using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkillMService.Models;

namespace SkillMService.Results
{
    public class SkillsGroupResult
    {
        public SkillsGroupResult() { }

        public int id { get; set; }
        public string name { get; set; }
        public List<SkillGroup> SkillGroups { get; set; }
    }
}

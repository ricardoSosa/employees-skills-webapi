using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkillMService.Models
{
    public class Family
    {
        //public string id { get; set; }

        public Family()
        {
        }

        public Family(string name)
        {
            this.name = name;
        }
        public string name { get; set; }
    }
}
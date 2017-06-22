using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SkillMService.Results;
using SkillMService.Models;
using SkillMService.App_Start;

namespace SkillMService.Controllers
{
    [RoutePrefix("api/Skill")]
    public class SkillController : ApiController
    {
        [Route("getList")]
        public SkillsResult Get()
        {
            List<Skill> skillList = DBConnection.GraphClient().Cypher
                    .Match("(child:Skill)")
                    .Return(child => child.As<Skill>())
                    .Results.ToList<Skill>();

            return new SkillsResult(skillList, 1);
        }
    }

    
}

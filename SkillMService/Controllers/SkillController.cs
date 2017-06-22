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

        [Route("getSkill")]
        public SkillsGroupResult getSkill(string id)
        {
            Skill skill = DBConnection.GraphClient().Cypher
                    .Match("(s:Skill)")
                    .Where("s.id = {id}")
                    .WithParam("id", id)
                    .Return(s => s.As<Skill>())
                    .Results.Single();            

            List<Family> skGroup = DBConnection.GraphClient().Cypher
                    .Match("(s:Skill)-[r:isFor]->(f:Family)")
                    .Where("s.id = {id}")
                    .WithParam("id", id)
                    .Return(f => f.As<Family>())
                    .Results.ToList<Family>();


            SkillsGroupResult skGroupRes = new SkillsGroupResult();
            skGroupRes.id = skill.id;
            skGroupRes.name = skill.name;
            skGroupRes.SkillGroups = skGroup;

            return skGroupRes;
        }

    }
   
    
}

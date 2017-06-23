using SkillMService.App_Start;
using SkillMService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SkillMService.Controllers
{
    [RoutePrefix("api/Family")]
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class FamilyController : ApiController
    {


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [Route("createSkillGroup")]
        public string createSkillGroup(string skillGroupName)
        {
            string result = "";

            if (skillGroupName != "")
            {
                if (existsSkillGroup(skillGroupName))
                {
                    result = "The skill actually exists.";
                }
                else
                {
                    Family newSkillGroup = new Family(skillGroupName);
                    DBConnection.GraphClient().Cypher
                        .Merge("(sk:SkillGroup {name: {name}})")
                        .OnCreate()
                        .Set("sk = {newSkillGroup}")
                        .WithParams(new
                        {
                            name = newSkillGroup.name,
                            newSkillGroup
                        })
                        .ExecuteWithoutResults();
                    result = "The skill has been created.";
                }
            }
            else
            {
                result = "The skill needs a name.";
            }
            return result;
        }



        public static bool existsSkillGroup(string skillGroupName)
        {
            bool exists = false;

            var skillGroup =
                DBConnection.GraphClient().Cypher
                .Match("(skgp:SkillGroup)")
                .Where("skgp.name = {skillGroupName}")
                .WithParam("skillGroupName", skillGroupName)
                .Return(skgp => skgp.As<Family>().name)
                .Results.SingleOrDefault();

            if (skillGroup == null)
            {
                exists = false;
            }
            else
            {
                exists = true;
            }

            return exists;
        }



    }
}

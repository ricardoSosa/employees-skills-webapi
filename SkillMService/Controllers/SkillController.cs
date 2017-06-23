using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SkillMService.Results;
using SkillMService.Models;
using SkillMService.App_Start;
using System.Web.Http.Cors;
using Neo4jClient;

namespace SkillMService.Controllers
{
    [RoutePrefix("api/Skill")]
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
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
                    .Match("(s:Skill)-[r:IS_RELATED_TO]->(f:SkillGroup)")
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


        [Route("getEmployees")]
        public SkillEmployeesResult getEmployees(string id)
        {
            Skill skill = DBConnection.GraphClient().Cypher
                    .Match("(s:Skill)")
                    .Where("s.id = {id}")
                    .WithParam("id", id)
                    .Return(s => s.As<Skill>())
                    .Results.Single();


            List<Employee> jrsemp = DBConnection.GraphClient().Cypher
                    .Match("(emp:Emp)-[r:Knows]->(s:Skill)")
                    .Where("s.id = {id} and r.value = '1'")
                    .WithParam("id", id)
                    .Return(emp => emp.As<Employee>())
                    .Results.ToList<Employee>();

            List<Employee> intemp = DBConnection.GraphClient().Cypher
                    .Match("(emp:Emp)-[r:Knows]->(s:Skill)")
                    .Where("s.id = {id} and r.value = '2'")
                    .WithParam("id", id)
                    .Return(emp => emp.As<Employee>())
                    .Results.ToList<Employee>();

            List<Employee> sremp = DBConnection.GraphClient().Cypher
                    .Match("(emp:Emp)-[r:Knows]->(s:Skill)")
                    .Where("s.id = {id} and r.value = '3'")
                    .WithParam("id", id)
                    .Return(emp => emp.As<Employee>())
                    .Results.ToList<Employee>();


            List<Employee> ldsemp = DBConnection.GraphClient().Cypher
                    .Match("(emp:Emp)-[r:Knows]->(s:Skill)")
                    .Where("s.id = {id} and r.value = '4'")
                    .WithParam("id", id)
                    .Return(emp => emp.As<Employee>())
                    .Results.ToList<Employee>();

            SkillEmployeesResult skempRes = new SkillEmployeesResult();
            skempRes.id = skill.id;
            skempRes.name = skill.name;
            skempRes.JrEmployees = jrsemp;
            skempRes.IntEmployees = intemp;
            skempRes.SrEmployees = sremp;
            skempRes.LdEmployees = ldsemp;

            return skempRes;           
        }



        public static  bool existsSkill(string skillName)
        {
            bool exists = false;

            var skill =
                DBConnection.GraphClient().Cypher
                    .Match("(sk:Skill)")
                    .Where("sk.name = {skillName}")
                    .WithParam("skillName", skillName)
                    .Return(sk => sk.As<Skill>().name)
                    .Results.SingleOrDefault();

            if (skill == null)
            {
                exists = false;
            }
            else
            {
                exists = true;
            }

            return exists;
        }


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [Route("createSkill")]
        public string createSkill(string skillName)
        {
            string result = "";

            if (skillName != "")
            {
                if (existsSkill(skillName))
                {
                    result = "The skill actually exists.";
                }
                else
                {
                    Skill newSkill = new Skill(skillName);
                    DBConnection.GraphClient().Cypher
                        .Merge("(sk:Skill {name: {name}})")
                        .OnCreate()
                        .Set("sk = {newSkill}")
                        .WithParams(new
                        {
                            name = newSkill.name,
                            newSkill
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


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [Route("updateSkill")]
        public string modifySkill(string skillName, string newName)
        {
            string result = "";

            if (skillName != "" && newName != "")
            {
                if (existsSkill(skillName))
                {
                    if (existsSkill(newName))
                    {
                        result = "Actually exists a skill with that new name.";
                    }
                    else
                    {
                        DBConnection.GraphClient().Cypher
                            .Match("(sk:Skill)")
                            .Where("sk.name = {skillName}")
                            .WithParam("skillName", skillName)
                            .Set("sk.name = {newName}")
                            .WithParam("newName", newName)
                            .ExecuteWithoutResults();

                        result = "The skill has been modified.";
                    }
                }
                else
                {
                    result = "The skill doesn't exists.";
                }
            }
            else
            {
                result = "The parameters cannot be empty.";
            }

            return result;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        [Route("relateSkillGroup")]
        public string relateSkillToSkillGroup(string skillName, string skillGroupName)
        {
            string result = "";

            if (skillName != "" && skillGroupName != "")
            {
                if (existsSkill(skillName) && FamilyController.existsSkillGroup(skillGroupName))
                {
                    if (isSkillRelatedToSkillGroup(skillName, skillGroupName))
                    {
                        result = "The skill is actually related to the skill group.";
                    }
                    else
                    {
                        DBConnection.GraphClient().Cypher
                           .Match("(sk:Skill), (skgp:SkillGroup)")
                           .Where("sk.name = {skillName}")
                           .WithParam("skillName", skillName)
                           .AndWhere("skgp.name = {skillGroupName}")
                           .WithParam("skillGroupName", skillGroupName)
                           .Create("(sk)-[:IS_RELATED_TO]->(skgp)")
                           .ExecuteWithoutResults();

                        result = "Assigned skill.";
                    }
                }
                else
                {
                    result = "The skill or skill group doesn't exist.";
                }
            }
            else
            {
                result = "The parameters cannot be empty.";
            }

            return result;
        }

        private bool isSkillRelatedToSkillGroup(string skillName, string skillGroupName)
        {
            bool isRelated = false;

            var result =
                DBConnection.GraphClient().Cypher
                .Match("(sk:Skill)-[rel:IS_RELATED_TO]->(skgp:SkillGroup)")
                .Where("sk.name = {skillName}")
                .WithParam("skillName", skillName)
                .AndWhere("skgp.name = {skillGroupName}")
                .WithParam("skillGroupName", skillGroupName)
                .Return(rel => rel.As<RelationshipInstance<object>>())
                .Results;

            if (result.Count() != 0)
            {
                isRelated = true;
            }

            return isRelated;
        }
    }
   
    
}

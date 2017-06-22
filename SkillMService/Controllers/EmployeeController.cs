using System.Web;
using Neo4jClient;
using SkillMService.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using SkillMService.App_Start;
using System.Web.Http;
using System.Web.Http.Results;
using SkillMService.Models;

namespace SkillMService.Controllers
{
    [RoutePrefix("api/Employee")]
    public class EmployeeController : ApiController
    {

        [Route("getList")]
        public EmployeeResult Get()
        {            
            List<Employee> parentsList = DBConnection.GraphClient().Cypher
                    .Match("(child:Emp)")
                    .Return(child => child.As<Employee>())
                    .Results.ToList<Employee>();
            return new EmployeeResult(parentsList, 1);            
        }


        [Route("getEmployee")]
        public EmployeeSkillResult GetEmployee(string userID)
        {            
            Employee parentsList = DBConnection.GraphClient().Cypher
                    .Match("(child:Emp)")
                    .Where("child.id = {id}")
                    .WithParam("id", userID)
                    .Return(child => child.As<Employee>())
                    .Results.Single();

            EmployeeSkillResult empSkill = new EmployeeSkillResult();
            empSkill.ID = parentsList.id;
            empSkill.NAME = parentsList.name;
            empSkill.LOCATION = parentsList.location;

            List<Skill> jrskills = DBConnection.GraphClient().Cypher
                    .Match("(emp:Emp)-[r:Knows]->(s:Skill)")
                    .Where("emp.id = {id} and r.value <= 35")
                    .WithParam("id", userID)
                    .Return(s => s.As<Skill>())
                    .Results.ToList<Skill>();

            List<Skill> intskills = DBConnection.GraphClient().Cypher
                    .Match("(emp:Emp)-[r:Knows]->(s:Skill)")
                    .Where("emp.id = {id} and r.value > 35 and r.value <= 60")
                    .WithParam("id", userID)
                    .Return(s => s.As<Skill>())
                    .Results.ToList<Skill>();

            List<Skill> srskills = DBConnection.GraphClient().Cypher
                    .Match("(emp:Emp)-[r:Knows]->(s:Skill)")
                    .Where("emp.id = {id} and r.value > 60 and r.value <= 90")
                    .WithParam("id", userID)
                    .Return(s => s.As<Skill>())
                    .Results.ToList<Skill>();


            List<Skill> ldskills = DBConnection.GraphClient().Cypher
                    .Match("(emp:Emp)-[r:Knows]->(s:Skill)")
                    .Where("emp.id = {id} and r.value > 90 and r.value <=100")
                    .WithParam("id", userID)
                    .Return(s => s.As<Skill>())
                    .Results.ToList<Skill>();

            empSkill.JrSkills = jrskills;
            empSkill.IntSkills = intskills;
            empSkill.SrSkills = srskills;
            empSkill.LdSkills = ldskills;


            empSkill.Status = 1;

            return empSkill;
        }
    }
}

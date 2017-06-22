using Neo4jClient;
using SkillMService.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SkillMService.App_Start;

using System.Web.Http;
using System.Web.Http.Results;
using SkillMService.Models;

namespace SkillMService.Controllers
{
    [RoutePrefix("api/Employee")]
    public class EmployeeController : ApiController
    {

        [Route("getValue")]
        public EmployeeResult Get()
        {
            GraphClient graphClient = DBConnection.GraphClient();
            string ret = "";

            List<Employee> parentsList = graphClient.Cypher
                    .Match("(child:EMP)")
                    .Return(child => child.As<Employee>())
                    .Results.ToList<Employee>();


           
            

            return new EmployeeResult(parentsList, 1);
            //return new JavaScriptSerializer().Serialize(parentsList);
        }
    }
}

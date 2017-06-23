using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkillMService.App_Start
{
    public static class DBConnection
    {
        static public GraphClient GraphClient()
        {
            GraphClient graphClient;
            graphClient = new GraphClient(new Uri("http://localhost:7474/db/data"), "neo4j", "skills");
            graphClient.Connect();
            return graphClient;
        }
    }
}
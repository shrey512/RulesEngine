using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using RulesEngine.Models;
using RulesEngineEditor.Models;

namespace RuleEngineSample.Data
{
     public class RuleEngineDBManager
    {
        private readonly string connectionstring;

        public RuleEngineDBManager(string connectionstring)
        {
            connectionstring = "Server=(localdb)\\mssqllocaldb;Database=RulesEngineEditorDB;Integrated Security=true;";
        }

        public SqlConnection OpenConnection()
        {
            string connectionstring = "Server=(localdb)\\mssqllocaldb;Database=RulesEngineEditorDB;Integrated Security=true;";
            SqlConnection connection = new SqlConnection(connectionstring);
            connection.Open();
            return connection;  
        }

        //public List<Rule> GetRules()
        //{
        //    List<Rule> rules = new List<Rule>();
        //    using (SqlConnection connection = OpenConnection())
        //    {
        //        SqlCommand command = new SqlCommand("SELECT * FROM [rule]", connection);
        //        SqlDataReader reader = command.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            Rule rule = new Rule
        //            {
        //                RuleName = reader["RuleName"].ToString(),
        //                SuccessEvent = reader["SuccessEvent"].ToString(),
        //                ErrorMessage = reader["ErrorMessage"].ToString(),
        //                Expression = reader["Expression"].ToString(),
        //                //RuleExpressionType = reader["RuleExpressionType"]
        //            };

        //            rules.Add(rule);
        //        }
        //    }
        //    return rules;
        //}

        public List<Demographic> Getdemographic()
        {
            List<Demographic> demographics = new List<Demographic>();
            using (SqlConnection connection = OpenConnection())
            {
                SqlCommand command = new SqlCommand("SELECT Id,First_Name,Last_Name,Age,Type,Gender FROM demographic", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Demographic demographic = new Demographic
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        First_Name = reader["First_Name"].ToString(),
                        Last_Name = reader["Last_Name"].ToString(),
                        Age = Convert.ToInt32(reader["Age"]),
                        Type = reader["Type"].ToString(),
                        Gender = reader["Gender"].ToString(),
                    };

                    demographics.Add(demographic);
                }
            }
            return demographics;
        }


        public void displayworkflowtable()
        {
            string connectionstring = "Server=(localdb)\\mssqllocaldb;Database=RulesEngineEditorDB;Integrated Security=true;";
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                string query = "SELECT * FROM Workflow";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"WorkflowName:{reader["WorkflowName"]}");
                    Console.WriteLine($"Seq:{reader["Seq"]}");
                    Console.WriteLine($"RuleExpressionType:{reader["RuleExpressionType"]}");
                }
            }
        }

        public void displayruletable()
        {
            string connectionstring = "Server=(localdb)\\mssqllocaldb;Database=RulesEngineEditorDB;Integrated Security=true;";
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                string query = "SELECT * FROM [rule]";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"Seq:{reader["Seq"]}");
                    Console.WriteLine($"RuleDataId:{reader["RuleDataId"]}");
                    Console.WriteLine($"WorkflowDataId:{reader["WorkflowDataId"]}");
                    Console.WriteLine($"RuleName:{reader["RuleName"]}");
                    Console.WriteLine($"Properties:{reader["Properties"]}");
                    Console.WriteLine($"Operator:{reader["Operator"]}");
                    Console.WriteLine($"Enabled:{reader["Enabled"]}");
                    Console.WriteLine($"RuleExpressionType:{reader["RuleExpressionType"]}");
                    Console.WriteLine($"Expression:{reader["Expression"]}");
                    Console.WriteLine($"Actions:{reader["Actions"]}");
                    Console.WriteLine($"SuccessEvent:{reader["SuccessEvent"]}");
                }
            }
        }

        public void displaydemographic()
        {
            string connectionstring = "Server=(localdb)\\mssqllocaldb;Database=RulesEngineEditorDB;Integrated Security=true;";
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                string query = "SELECT Id,First_Name,Last_Name,Age,Type, Gender FROM demographic";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"Id:{reader["Id"]}");
                    Console.WriteLine($"First_Name:{reader["First_Name"]}");
                    Console.WriteLine($"Last_Name:{reader["Last_Name"]}");
                    Console.WriteLine($"Age:{reader["Age"]}");
                    Console.WriteLine($"Type:{reader["Type"]}");
                    Console.WriteLine($"Gender:{reader["Gender"]}");

                }
            }
        }
    }

    public class Demographic
    {
        public int? Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public int Age { get; set; }
        public string Type { get; set; }
        public string Gender { get; set; }
    }
}

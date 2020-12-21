using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace EmployeePayrollRestSharpMSTest
{
    [TestClass]
    public class RestSharpTest
    {
        RestClient client;

        [TestInitialize]
        public void Setup()
        {
            //initializing te base url
            client = new RestClient("http://localhost:4000");
        }

        private IRestResponse getEmployeeList()
        {
            //arrange
            RestRequest request = new RestRequest("/Employee", Method.GET);
            //act
            IRestResponse response = client.Execute(request);
            return response;
        }

        [TestMethod]
        public void givenDetails_WhenCalledTheGetAPI_ShouldReturnEmployeeList()
        {
            //assert
            IRestResponse response = getEmployeeList();
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            //deserializing the object
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(10, dataResponse.Count);
            foreach (Employee employee in dataResponse)
            {
                Console.WriteLine("Id:" + employee.id + "\nName:" + employee.name + "\nSalary:" + employee.salary);
            }

        }

        [TestMethod]
        public void givenDetails_WhenCalledThePostAPI_ShouldAddTheEmployeeAndReturnEmployeeList()
        {
            //arrange
            RestRequest request = new RestRequest("/Employee", Method.POST);
            JObject jObjectbody = new JObject();
            jObjectbody.Add("name", "Liam");
            jObjectbody.Add("salary", "80000");

            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);
            //act
            IRestResponse response = client.Execute(request);
            //assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            //deserializing the object
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Liam", dataResponse.name);
            Assert.AreEqual("80000", dataResponse.salary);
        }

        [TestMethod]
        public void givenEmployees_WhenAddedMultipleEmployeesOnPost_ShouldReturnAddedEmployee()
        {
            List<Employee> employees = new List<Employee>();
            employees.Add(new Employee { name = "Minho", salary = "50000" });
            employees.Add(new Employee { name = "Minhee", salary = "60000" });
            foreach (Employee employee in employees)
            {
                //arrange
                RestRequest request = new RestRequest("/Employee", Method.POST);
                JObject jObjectbody = new JObject();
                jObjectbody.Add("name", employee.name);
                jObjectbody.Add("salary", employee.salary);
                request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);
                //act
                IRestResponse response = client.Execute(request);
                //assert
                Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
                //deserializing the object
                Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
                Assert.AreEqual(employee.name, dataResponse.name);
            }
        }
    }
}

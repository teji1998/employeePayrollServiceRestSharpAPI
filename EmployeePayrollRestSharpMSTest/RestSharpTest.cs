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

        /// <summary>
        /// Gets the employee list.
        /// </summary>
        /// <returns></returns>
        private IRestResponse getEmployeeList()
        {
            //arrange
            RestRequest request = new RestRequest("/Employee", Method.GET);
            //act
            IRestResponse response = client.Execute(request);
            return response;
        }

        /// <summary>
        /// Givens the details when called the get API should return employee list.
        /// </summary>
        [TestMethod]
        public void givenDetails_WhenCalledTheGetAPI_ShouldReturnEmployeeList()
        {
            //assert
            IRestResponse response = getEmployeeList();
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            //deserializing the object
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(13, dataResponse.Count);
            foreach (Employee employee in dataResponse)
            {
                Console.WriteLine("Id:" + employee.id + "\nName:" + employee.name + "\nSalary:" + employee.salary);
            }

        }

        /// <summary>
        /// Given the details when called the post API should add the employee and return employee list.
        /// </summary>
        [TestMethod]
        public void givenDetails_WhenCalledThePostAPI_ShouldAddTheEmployeeAndReturnEmployeeList()
        {
            //arrange
            RestRequest request = new RestRequest("/Employee", Method.POST);
            //creating JSON object
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

        /// <summary>
        /// Given employees when added multiple employees on post should return added employee.
        /// </summary>
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
                //creating Jobject
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

        /// <summary>
        /// Given the details when updated the employee should return updated employee details.
        /// </summary>
        [TestMethod]
        public void givenDetails_WhenUpdatedTheEmployee_ShouldReturnUpdatedEmployeeDetails()
        {
            //arrange
            //For a particular person updation
            RestRequest request = new RestRequest("/Employee/14", Method.PUT);
           //Jobject creation
            JObject jObjectBody = new JObject();
            jObjectBody.Add("name", "Teju");
            jObjectBody.Add("salary", "700000");
            request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);
            //act
            IRestResponse response = client.Execute(request);
           //assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            //deserializing an object
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual(dataResponse.salary, "700000");
        }

        /// <summary>
        /// Given the details when deleted employee should return ok status.
        /// </summary>
        [TestMethod]
        public void givenDetails_WhenDeletedEmployee_ShouldReturnOKStatus()
        {
            //arrange
            //to delete a particular employee
            RestRequest request = new RestRequest("/Employee/17", Method.DELETE);
            //act
            IRestResponse response = client.Execute(request);
            //assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }
    }
}

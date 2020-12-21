using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
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
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(10, dataResponse.Count);
            foreach (Employee employee in dataResponse)
            {
                Console.WriteLine("Id:" + employee.id + "\nName:" + employee.name + "\nSalary:" + employee.salary);
            }

        }
    }
}

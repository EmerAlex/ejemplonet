using Microservice1.Application.Person;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;

namespace Api.Test
{
    [TestClass]
    public class PersonControllerTest : IntegrationTestBuilder
    {
        [TestInitialize]
        public void Initialize()
        {
            BootstrapTestingSuite();
        }

        [TestMethod]
        public void GetPersonSuccess()
        {
            var c = this.TestClient.GetAsync($"api/Person/{PersonId}").Result;
            c.EnsureSuccessStatusCode();
            var response = c.Content.ReadAsStringAsync().Result;
            var personData = System.Text.Json.JsonSerializer.Deserialize<PersonDto>(response, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
            });
            Assert.IsNotNull(personData);
        }

        [TestMethod]
        public void PostPersonSuccess()
        {
            var postContent = new Microservice1.Application.Person.CreatePersonCommand
            {
                Email = "john@doe.com",
                FirstName = "john",
                LastName = "doe"
            };

            var c = this.TestClient.PostAsync("api/Person/sync", postContent, new JsonMediaTypeFormatter()).Result;
            c.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, c.StatusCode);
        }

        [TestMethod]
        public void PostPersonAsyncSuccess()
        {
            var postContent = new Microservice1.Application.Person.CreatePersonCommandAsync("john", "doe", "john@doe.com");

            var c = this.TestClient.PostAsync("api/Person", postContent, new JsonMediaTypeFormatter()).Result;
            c.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, c.StatusCode);
        }

        [TestMethod, ExpectedException(typeof(System.Text.Json.JsonException))]
        public void GetPersonFailure()
        {
            var c = this.TestClient.GetAsync($"api/Person/{Guid.NewGuid()}").Result;
            c.EnsureSuccessStatusCode();
            var response = c.Content.ReadAsStringAsync().Result;
            var personData = System.Text.Json.JsonSerializer.Deserialize<PersonDto>(response, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
            });
        }

        [TestMethod, ExpectedException(typeof(System.Net.Http.HttpRequestException))]
        public void GetPersonBadRequestFailure()
        {
            var c = this.TestClient.GetAsync($"api/Person/foobar").Result;
            c.EnsureSuccessStatusCode();
        }

    }

}

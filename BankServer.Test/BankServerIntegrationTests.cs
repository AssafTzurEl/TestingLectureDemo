using BankServer.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mime;
using System.Text;

namespace BankServer.Test
{
    [TestClass]
    public class BankServerIntegrationTests
    {
        /// <summary>
        /// Using a static ctor instead of a [ClassInitializer] method to avoid
        /// nullable _factory warnings.
        /// </summary>
        static BankServerIntegrationTests()
        {
            _factory = new WebApplicationFactory<Program>();
            // If you want to set specific services, add this to the previous line:
            //.WithWebHostBuilder(builder =>
            // {
            //     // ... Configure test services
            // });
        }

        /// <summary>
        /// Using a ctor instead of a [TestInitialize] method to avoid
        /// nullable _client warnings.
        /// </summary>
        public BankServerIntegrationTests()
        {
            _client = _factory.CreateClient();
            _client.DeleteAsync(AccountUrl).GetAwaiter().GetResult();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _factory.Dispose();
        }

        [TestMethod]
        public async Task When_NoAccountsExist_ExpectGetAllToReturnZeroItems()
        {
            var response = await _client.GetAsync(AccountUrl);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.ConvertTo<List<Account>>().Should().BeEmpty();
        }

        [TestMethod]
        public async Task When_CreateAccount_Expect_AccountCreated()
        {
            var newAccount = new Account { Name = "Assaf" };

            var response = await _client.PostAsync(AccountUrl, ObjectToContent(newAccount));

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var actualAccount = response.ConvertTo<Account>();

            actualAccount.Should().NotBeNull();
            response.Headers.Location.Should().Be($"/{AccountUrl}/{actualAccount!.Id}");
            actualAccount.Name.Should().Be(newAccount.Name);
            actualAccount.Id.Should().NotBe(0);
            actualAccount.Balance.Should().Be(0);
        }

        [TestMethod]
        public async Task When_CreateAccountAndGetAccount_Expect_AccountReceived()
        {
            var newAccount = new Account { Name = "Assaf" };

            var response = await _client.PostAsync(AccountUrl, ObjectToContent(newAccount));
            var actualAccount = response.ConvertTo<Account>();

            response = await _client.GetAsync($"{AccountUrl}/{actualAccount!.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            actualAccount = response.ConvertTo<Account>();
            actualAccount.Should().NotBeNull();
            actualAccount!.Name.Should().Be(newAccount.Name);
            actualAccount.Id.Should().NotBe(0);
            actualAccount.Balance.Should().Be(0);
        }

        private StringContent ObjectToContent<T>(T obj)
        {
            var json = JsonConvert.SerializeObject(obj);

            return new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
        }

        private static string AccountUrl = "account";
        private static WebApplicationFactory<Program> _factory;
        private HttpClient _client;
    }
}

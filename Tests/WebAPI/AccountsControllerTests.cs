using Entities.Concrete;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebAPI;

namespace Tests.WebAPI
{
    [TestFixture]
    public class AccountsControllerTests : WebApplicationFactory<Startup>
    {
        protected HttpClient Client;
        
        [SetUp]
        public void Setup()
        {
            Client = CreateClient();
        }

        [Test]
        public async Task Get()
        {
            const string requestUri = "accounts";

            var response = await Client.GetAsync(requestUri);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        [TestCase(123, "TRY", 100)]
        public async Task Post(int accountNumber, string currencyCode, decimal balance)
        {
            const string requestUri = "accounts/create";            
            var account = new Account { AccountNumber = accountNumber, CurrencyCode = currencyCode, Balance = balance };
            var httpContent = new StringContent(JsonConvert.SerializeObject(account), Encoding.UTF8, "application/json");

            var response = await Client.PostAsync(requestUri, httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}

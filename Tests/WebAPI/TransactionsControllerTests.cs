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
    public class TransactionControllerTests : WebApplicationFactory<Startup>
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
            const string requestUri = "transactions";

            var response = await Client.GetAsync(requestUri);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        [TestCase(123, 156, 23.45)]
        public async Task Post(int senderAccountNumber, int receiverAccountNumber, decimal amount)
        {
            const string requestUri = "transactions/create";
            var transaction = new Transaction { SenderAccountNumber = senderAccountNumber, ReceiverAccountNumber = receiverAccountNumber, Amount = amount };
            var httpContent = new StringContent(JsonConvert.SerializeObject(transaction), Encoding.UTF8, "application/json");

            var response = await Client.PostAsync(requestUri, httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}

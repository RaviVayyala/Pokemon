using Moq;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using pokemon.Core;
using Moq.Protected;
using pokemon.Models;

namespace Pokemon.Tests.Core
{
    [TestFixture]
    public class PokemonHttpClientTests
    {
        private MockRepository _mockRepository;
        private Mock<IHttpClientFactory> _mockHttpClientFactory;
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockHttpClientFactory = _mockRepository.Create<IHttpClientFactory>();
        }

        private PokemonHttpClient CreatePokemonHttpClient(HttpStatusCode httpStatusCode, HttpContent httpContent)
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = httpStatusCode, Content = httpContent });

            var client = new HttpClient(_mockHttpMessageHandler.Object);
            _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(client);

            return new PokemonHttpClient(
                _mockHttpClientFactory.Object);
        }

        [Test]
        public async Task GivenAValidUrlWithValidName_WhenGetAsyncIsCalled_ThenTheExpectedObjectReturned()
        {
            // Arrange
            var pokemonHttpClient = CreatePokemonHttpClient(HttpStatusCode.OK, new StringContent("{'id':1,'name':'Perseverance Rover'}"));
            string url = "https://someUrldontknowifitexists.com/valid";

            // Act
            var result = await pokemonHttpClient.GetAsync<TestObject>(url);

            // Assert
            Assert.AreEqual("Perseverance Rover", result.Name);
            _mockRepository.VerifyAll();
        }

        [Test]
        public async Task GivenAValidUrlWithInvalidName_WhenGetAsyncIsCalled_ThenThrowsAPIExceptionWithStatusCode()
        {
            // Arrange
            var pokemonHttpClient = CreatePokemonHttpClient(HttpStatusCode.BadRequest, new StringContent("{'id':1,'name':'Perseverance Rover'}"));
            string url = "https://someUrldontknowifitexists.com/invalid";

            // Act
            var ex = Assert.ThrowsAsync<APIException>(() => pokemonHttpClient.GetAsync<TestObject>(url));
            Assert.AreEqual(ex.StatusCodes, 400);
            _mockRepository.VerifyAll();
        }

        private class TestObject
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}

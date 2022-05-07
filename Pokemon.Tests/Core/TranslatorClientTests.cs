using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using pokemon.Core;
using pokemon.Models;

namespace Pokemon.Tests.Core
{
    [TestFixture]
    public class TranslatorClientTests
    {
        private MockRepository _mockRepository;

        private Mock<IPokemonHttpClient> _mockPokemonHttpClient;
        private Mock<IOptions<TranslatorAPISettings>> _mockOptions;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockPokemonHttpClient = _mockRepository.Create<IPokemonHttpClient>();
            _mockOptions = _mockRepository.Create<IOptions<TranslatorAPISettings>>();
        }

        private TranslatorClient CreateTranslatorClient()
        {
            return new TranslatorClient(
                _mockPokemonHttpClient.Object,
                _mockOptions.Object);
        }

        [Test]
        public async Task GivenAValidDesc_WhenGetYodaTranslationIsCalled_ThenTheTranslatedObjectIsReturned()
        {
            // Arrange
            var expected = "please make sure you don't have a new line special char in this string";
            _mockPokemonHttpClient.Setup(x => x.GetAsync<Translation>(It.IsAny<string>()))
                .Returns(Task.FromResult(new Translation() { Contents = new Contents() { Translated = expected } }));

            _mockOptions.Setup(X => X.Value).Returns(new TranslatorAPISettings()
            {
                BaseUrl = "jusforwordingasbase",
                ShakespeareTranslatePath = "Stratford-upon-Avon",
                YodaTranslatePath = "Star-Wars"
            });


            var translatorApiClient = CreateTranslatorClient();
            string longdesc = "very long string";

            // Act
            var result = await translatorApiClient.GetYodaTranslation(
                longdesc);

            // Assert
            Assert.AreEqual(expected, result);
            _mockRepository.VerifyAll();
        }

        [Test]
        public async Task GivenAValidDesc_WhenGetShakespeareTranslationIsCalled_ThenTheTranslatedObjectIsReturned()
        {
            // Arrange
            var expected = "please make sure you don't have new line special char in this string - some difference";
            _mockPokemonHttpClient.Setup(x => x.GetAsync<Translation>(It.IsAny<string>()))
                .Returns(Task.FromResult(new Translation() { Contents = new Contents() { Translated = expected } }));

            _mockOptions.Setup(X => X.Value).Returns(new TranslatorAPISettings()
            {
                BaseUrl = "jusforwordingasbase",
                ShakespeareTranslatePath = "Stratford-upon-Avon",
                YodaTranslatePath = "Star-Wars"
            });


            var translatorApiClient = CreateTranslatorClient();
            string longdesc = "long string has more words";

            // Act
            var result = await translatorApiClient.GetShakespeareTranslation(
                longdesc);

            // Assert
            Assert.AreEqual(expected, result);
            _mockRepository.VerifyAll();
        }
    }
}

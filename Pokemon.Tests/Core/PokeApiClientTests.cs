using System;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using pokemon.Core;
using pokemon.Models;
using System.Threading.Tasks;

namespace Pokemon.Tests.Core
{
    [TestFixture]
    public class PokeApiClientTests
    {
        private MockRepository _mockRepository;

        private Mock<IPokemonHttpClient> _mockPokemonHttpClientBuilder;
        private Mock<IOptions<PokeAPISettings>> _mockOptions;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockPokemonHttpClientBuilder = _mockRepository.Create<IPokemonHttpClient>();
            _mockOptions = _mockRepository.Create<IOptions<PokeAPISettings>>();
        }

        private PokeApiClient CreatePokeApiClient()
        {
            return new PokeApiClient(
                _mockPokemonHttpClientBuilder.Object,
                _mockOptions.Object);
        }

        [Test]
        public async Task GivenAValidNameAndSettings_WhenGetSpeciesIsCalled_ThenTheExpectedObjectReturned()
        {
            // Arrange
            var expectedname = "go";
            _mockPokemonHttpClientBuilder.Setup(x => x.GetAsync<PokemonSpeciesModel>(It.IsAny<string>()))
                .Returns(Task.FromResult(new PokemonSpeciesModel() { Name = expectedname }));

            _mockOptions.Setup(X => X.Value).Returns(new PokeAPISettings()
            {
                BaseUrl = "jusforwordingasbase",
                GetNamePath = "mars"
            });


            var pokeApiClient = CreatePokeApiClient();
            string pokemonName = "go";

            // Act
            var result = await pokeApiClient.GetSpecies(
                pokemonName);

            // Assert
            Assert.AreEqual(expectedname, result.Name);
            _mockRepository.VerifyAll();
        }

        [Test]
        public async Task GivenAValidNameAndSettings_WhenGetSpeciesIsCalledAndWhenThereIsAAPIError_ThenAPIExceptionIsThrown()
        {
            // Arrange
            var expectedMessage = "go";
            _mockPokemonHttpClientBuilder.Setup(x => x.GetAsync<PokemonSpeciesModel>(It.IsAny<string>()))
                .ThrowsAsync(new APIException("some error", 007));

            _mockOptions.Setup(X => X.Value).Returns(new PokeAPISettings()
            {
                BaseUrl = "jusforwordingasbase",
                GetNamePath = "mars"
            });


            var pokeApiClient = CreatePokeApiClient();
            string pokemonName = "go";

            // Act
            var ex = Assert.ThrowsAsync<APIException>(() => pokeApiClient.GetSpecies(pokemonName));
            Assert.AreEqual(ex.StatusCodes, 007);
            _mockRepository.VerifyAll();
        }
    }
}

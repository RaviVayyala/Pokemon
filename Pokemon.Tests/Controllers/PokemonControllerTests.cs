using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using pokemon.Controllers;
using pokemon.Core;
using pokemon.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pokemon.Tests.Controllers
{
    [TestFixture]
    public class PokemonControllerTests
    {
        private MockRepository _mockRepository;

        private Mock<IPokeApiClient> _mockPokeApiClient;
        private Mock<ITranslatorClient> _mockTranslatorClient;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockPokeApiClient = _mockRepository.Create<IPokeApiClient>();
            _mockTranslatorClient = _mockRepository.Create<ITranslatorClient>();
        }

        private PokemonController CreatePokemonController()
        {
            return new PokemonController(
                _mockPokeApiClient.Object,
                _mockTranslatorClient.Object);
        }

        [Test]
        public void GivenAValidName_WhenGetPokemonSpeciesIsCalled_ThenReturnOkObject()
        {
            // Arrange
            var expectedname = "jamesbond";
            _mockPokeApiClient.Setup(x => x.GetSpecies(It.IsAny<string>()))
                .Returns(Task.FromResult(new PokemonSpeciesModel() { Name = expectedname, FlavorTexts = new List<PokemonSpeciesFlavorText>(), Habitat = new Common() }));

            var pokemonController = CreatePokemonController();

            // Act
            var action = pokemonController.GetPokemonSpecies(expectedname).Result;

            // Assert
            Assert.IsTrue(action is OkObjectResult);
            _mockPokeApiClient.Verify(x => x.GetSpecies(It.IsAny<string>()), Times.Once);
            _mockRepository.VerifyAll();
        }

        [Test]
        public void GivenAEmptyString_WhenGetPokemonSpeciesIsCalled_ThenBadRequestObject()
        {
            // Arrange
            var pokemonController = CreatePokemonController();

            // Act
            var action = pokemonController.GetPokemonSpecies("").Result;

            // Assert
            Assert.IsTrue(action is BadRequestObjectResult);
            _mockPokeApiClient.Verify(x => x.GetSpecies(It.IsAny<string>()), Times.Never);
            _mockRepository.VerifyAll();
        }

        [Test]
        public void GivenAValidName_WhenGetPokemonSpeciesTranslatedIsCalled_ThenReturnOkObject()
        {
            // Arrange
            var desc = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";
            _mockPokeApiClient.Setup(x => x.GetSpecies(It.IsAny<string>()))
                .Returns(Task.FromResult(new PokemonSpeciesModel() { Name = "somename", FlavorTexts = new List<PokemonSpeciesFlavorText>() { new PokemonSpeciesFlavorText() { FlavorText = desc, Language = new Common() { Name = "en" } } }, Habitat = new Common() { Name = "cave" } }));

            _mockTranslatorClient.Setup(x => x.GetYodaTranslation(It.IsAny<string>()))
                .Returns(Task.FromResult(desc.Replace("l", "k")));

            var pokemonController = CreatePokemonController();

            // Act
            var action = pokemonController.GetPokemonSpeciesTranslated("somename").Result;

            // Assert
            Assert.IsTrue(action is OkObjectResult);
            _mockPokeApiClient.Verify(x => x.GetSpecies("somename"), Times.Once);
            _mockTranslatorClient.Verify(x => x.GetYodaTranslation(desc), Times.Once);
            _mockTranslatorClient.Verify(x => x.GetShakespeareTranslation(desc), Times.Never);
            _mockRepository.VerifyAll();
        }

        [Test]
        public void GivenAValidName_WhenGetShakespeareTranslationIsCalled_ThenReturnOkObject()
        {
            // Arrange
            var desc = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";
            _mockPokeApiClient.Setup(x => x.GetSpecies(It.IsAny<string>()))
                .Returns(Task.FromResult(new PokemonSpeciesModel() { Name = "somename", FlavorTexts = new List<PokemonSpeciesFlavorText>() { new PokemonSpeciesFlavorText() { FlavorText = desc, Language = new Common() { Name = "en" } } }, Habitat = new Common() { Name = "else" } }));

            _mockTranslatorClient.Setup(x => x.GetShakespeareTranslation(It.IsAny<string>()))
                .Returns(Task.FromResult(desc.Replace("i", "v")));

            var pokemonController = CreatePokemonController();

            // Act
            var action = pokemonController.GetPokemonSpeciesTranslated("somename").Result;

            // Assert
            Assert.IsTrue(action is OkObjectResult);

            _mockPokeApiClient.Verify(x => x.GetSpecies("somename"), Times.Once);
            _mockTranslatorClient.Verify(x => x.GetShakespeareTranslation(desc), Times.Once);
            _mockTranslatorClient.Verify(x => x.GetYodaTranslation(desc), Times.Never);
            _mockRepository.VerifyAll();
        }

        [Test]
        public void GivenAEmptyString_WhenGetPokemonSpeciesTranslatedIsCalled_ThenBadRequestObject()
        {
            // Arrange
            var pokemonController = CreatePokemonController();

            // Act
            var action = pokemonController.GetPokemonSpeciesTranslated("").Result;

            // Assert
            Assert.IsTrue(action is BadRequestObjectResult);
            _mockRepository.VerifyAll();
        }
    }
}

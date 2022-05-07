using System.IO;
using Microsoft.AspNetCore.Mvc;
using pokemon.Core;
using pokemon.Mappers;
using pokemon.Models;
using System.Threading.Tasks;

namespace pokemon.Controllers
{
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokeApiClient _pokeApiClient;
        private readonly ITranslatorClient _translatorClient;

        public PokemonController(IPokeApiClient pokeApiClient, ITranslatorClient translatorClient)
        {
            _pokeApiClient = pokeApiClient;
            _translatorClient = translatorClient;
        }

        [HttpGet("{name?}")]
        public async Task<IActionResult> GetPokemonSpecies(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Please provide name");

            var speciesModel = await _pokeApiClient.GetSpecies(name);
            return Ok(SpeciesMapper.GetSpeciesModel(speciesModel));
        }

        [Route("translated/{name}")]
        [HttpGet]
        public async Task<IActionResult> GetPokemonSpeciesTranslated(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Please provide name");

            var speciesModel = await _pokeApiClient.GetSpecies(name);
            var data = SpeciesMapper.GetSpeciesModel(speciesModel);

            if (!string.IsNullOrWhiteSpace(data.Description))
            {
                //small hack to replace new line with a space. For some reason \n is throwing error in transaltor api
                data.Description = data.Description.Replace("\n", " ");

                if (data.Habitat.ToLower() == "cave" || data.Habitat.ToLower() == "legendary")
                {
                    data.Description = await _translatorClient.GetYodaTranslation(data.Description);
                }
                else
                {
                    data.Description = await _translatorClient.GetShakespeareTranslation(data.Description);
                }
            }

            return Ok(data);
        }
    }
}
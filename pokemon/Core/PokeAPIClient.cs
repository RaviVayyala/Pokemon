using Microsoft.Extensions.Options;
using pokemon.Models;
using System.Threading.Tasks;

namespace pokemon.Core
{
    public class PokeApiClient : IPokeApiClient
    {
        private readonly IPokemonHttpClient _httpClient;
        private readonly PokeAPISettings _pokeApiSettings;

        public PokeApiClient(IPokemonHttpClient httpClient, IOptions<PokeAPISettings> pokeApiSettings)
        {
            _httpClient = httpClient;
            _pokeApiSettings = pokeApiSettings.Value;
        }

        public async Task<PokemonSpeciesModel> GetSpecies(string pokemonName)
        {
            var url = $"{_pokeApiSettings.BaseUrl}{_pokeApiSettings.GetNamePath}/{pokemonName}";
            return await _httpClient
                .GetAsync<PokemonSpeciesModel>(url);
        }
    }
}

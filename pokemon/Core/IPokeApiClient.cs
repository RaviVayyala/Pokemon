using System.Threading.Tasks;
using pokemon.Models;

namespace pokemon.Core
{
    public interface IPokeApiClient
    {
        Task<PokemonSpeciesModel> GetSpecies(string pokemonName);
    }
}
using pokemon.Models;
using System.Linq;

namespace pokemon.Mappers
{
    public static class SpeciesMapper
    {
        public static SpeciesModel GetSpeciesModel(PokemonSpeciesModel pokemonSpeciesModel)
        {
            return new SpeciesModel
            {
                Name = pokemonSpeciesModel?.Name,
                Description = pokemonSpeciesModel?.FlavorTexts?.FirstOrDefault(x => x.Language?.Name.ToLower() == "en")
                    ?.FlavorText,
                Habitat = pokemonSpeciesModel?.Habitat.Name,
                IsLegendary = pokemonSpeciesModel != null && pokemonSpeciesModel.IsLegendary
            };
        }
    }
}

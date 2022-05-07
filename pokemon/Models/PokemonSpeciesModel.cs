using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace pokemon.Models
{
    public class PokemonSpeciesModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_legendary")]
        public bool IsLegendary { get; set; }

        [JsonProperty("habitat")]
        public Common Habitat { get; set; }

        [JsonProperty("flavor_text_entries")]
        public List<PokemonSpeciesFlavorText> FlavorTexts { get; set; }
    }

    public class PokemonSpeciesFlavorText
    {
        [JsonProperty("flavor_text")]
        public string FlavorText { get; set; }

        [JsonProperty("language")]
        public Common Language { get; set; }

        [JsonProperty("version")]
        public Common Version { get; set; }
    }

    public class Common
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string URL { get; set; }
    }

    public class Contents
    {
        [JsonProperty("translated")]
        public string Translated { get; set; }
    }

    public class Translation
    {
        [JsonProperty("contents")]
        public Contents Contents { get; set; }
    }
}
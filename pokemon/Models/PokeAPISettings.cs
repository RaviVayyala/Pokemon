namespace pokemon.Models
{
    public class PokeAPISettings : APISettings
    {
        public string GetNamePath { get; set; }
    }

    public class TranslatorAPISettings : APISettings
    {
        public string YodaTranslatePath { get; set; }

        public string ShakespeareTranslatePath { get; set; }
    }
}

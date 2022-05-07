using Microsoft.Extensions.Options;
using pokemon.Models;
using System.Threading.Tasks;

namespace pokemon.Core
{
    public class TranslatorClient : ITranslatorClient
    {
        private readonly IPokemonHttpClient _httpClient;
        private readonly TranslatorAPISettings _translatorApiSettings;

        public TranslatorClient(IPokemonHttpClient httpClient, IOptions<TranslatorAPISettings> translatorApiSettings)
        {
            _httpClient = httpClient;
            _translatorApiSettings = translatorApiSettings.Value;
        }

        public async Task<string> GetYodaTranslation(string text)
        {
            var url = $"{_translatorApiSettings.BaseUrl}{_translatorApiSettings.YodaTranslatePath}?text={text}";
            var translation = await _httpClient
                .GetAsync<Translation>(url);

            return translation.Contents.Translated;
        }

        public async Task<string> GetShakespeareTranslation(string text)
        {
            var url = $"{_translatorApiSettings.BaseUrl}{_translatorApiSettings.ShakespeareTranslatePath}?text={text}";
            var translation = await _httpClient
                .GetAsync<Translation>(url);

            return translation.Contents.Translated;
        }
    }
}

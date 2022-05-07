using Newtonsoft.Json;
using pokemon.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace pokemon.Core
{
    public class PokemonHttpClient : IPokemonHttpClient
    {
        private readonly HttpClient _httpClient;
        private string _queryString;
        private string _path;
        private string _keylessQueryString;
        private string _baseUrl;

        public PokemonHttpClient(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient();
            _queryString = string.Empty;
            _path = string.Empty;
            _keylessQueryString = string.Empty;
            _baseUrl = string.Empty;
        }

        public async Task<T> GetAsync<T>(string url)
        {
            using var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode == false)
                throw new APIException("ThirdParty API Error", (int)response.StatusCode);


            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
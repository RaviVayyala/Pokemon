using System.Threading.Tasks;

namespace pokemon.Core
{
    public interface IPokemonHttpClient
    {
        Task<T> GetAsync<T>(string url);
    }
}
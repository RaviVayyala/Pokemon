using System.Threading.Tasks;
using pokemon.Models;

namespace pokemon.Core
{
    public interface ITranslatorClient
    {
        Task<string> GetYodaTranslation(string text);
        Task<string> GetShakespeareTranslation(string text);
    }
}
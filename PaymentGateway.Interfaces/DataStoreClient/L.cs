using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PaymentGateway.Interfaces.DataStoreClient
{
    public class LibraryService : ILibraryService
    {
        private readonly HttpClient _httpClient;

        public LibraryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<string>> Download(string uri)
        {
            IEnumerable<string> words = Enumerable.Empty<string>();
       
            _httpClient.Timeout = TimeSpan.FromMinutes(5);
            var result = await _httpClient.GetAsync("/qualified/challenge-data/master/words_alpha.txt");

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var content = await result.Content.ReadAsStringAsync();
                words = content.Split("\n");
            }

            return words;
        }
    }

    public interface ILibraryService
    {
        Task<IEnumerable<string>> Download(string uri);
    }
}
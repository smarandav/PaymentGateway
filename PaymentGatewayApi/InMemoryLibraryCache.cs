using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGatewayApi
{
    public class InMemoryLibraryCache : ILibraryCache
    {
        public IEnumerable<string> Words { get; }

        public InMemoryLibraryCache(IEnumerable<string> words)
        {
            Words = words;
        }
    }
    
    public interface ILibraryCache
    {
        IEnumerable<string> Words { get; }
    }

    public class WordCompletionService : IWordCompletionService
    {
        private readonly ILibraryCache _libraryCache;

        public WordCompletionService(ILibraryCache libraryCache)
        {
            _libraryCache = libraryCache;
        }

        public IEnumerable<string> Find(string prefix)
        {
            return _libraryCache.Words.Where(w => w.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase));
        }
    }

    public interface IWordCompletionService
    {
        IEnumerable<string> Find(string prefix);
    }
}
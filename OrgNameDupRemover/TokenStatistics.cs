using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgNameDupRemover
{
    public class TokenStatistics
    {
        private readonly Dictionary<Token, int> _dict = new Dictionary<Token, int>();
        public int NumberOfDucoments { get; protected set; }

        public void AddDocuments(IEnumerable<Token[]> documents)
        {
            foreach (var doc in documents) AddDocument(doc);
        }

        public void AddDocument(Token[] tokens)
        {
            var seenTokens = new HashSet<Token>();
            foreach (var token in tokens.Where(token => !seenTokens.Contains(token)))
            {
                seenTokens.Add(token);
                int frequency = 0;
                _dict.TryGetValue(token, out frequency);
                _dict[token] = frequency + 1;
            }
            NumberOfDucoments++;
        }

        public int? GetDocumentFrequency(Token token)
        {
            int frequency = 0;
            return _dict.TryGetValue(token, out frequency)
                ? frequency
                : (int?)null;
        }
    }
}

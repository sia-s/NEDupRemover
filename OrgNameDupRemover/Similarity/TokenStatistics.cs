using System.Collections.Generic;
using System.Linq;
using OrgNameDupRemover.Tokenizing;

namespace OrgNameDupRemover.Similarity
{
    /// <summary>
    /// Keeps statistics about tokens of a document (tokenized string) collection. 
    /// It basically keeps token frequencies and number of documents to be used to
    /// calculate IDF (inverse document frequency) for tokens
    /// </summary>
    public class TokenStatistics
    {
        private readonly Dictionary<Token, int> _dict = new Dictionary<Token, int>();
        public int NumberOfDucoments { get; protected set; }

        public void AddDocuments(IEnumerable<TokenizedString> documents)
        {
            foreach (var doc in documents) AddDocument(doc);
        }

        /// <summary>
        /// Adds string tokens to the set
        /// </summary>
        /// <param name="tokens"></param>
        public void AddDocument(TokenizedString tokens)
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

        /// <summary>
        /// If token has been seen returns the number of times it was seen in added documents
        /// otherwise returns null
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public int? GetDocumentFrequency(Token token)
        {
            int frequency = 0;
            return _dict.TryGetValue(token, out frequency)
                ? frequency
                : (int?)null;
        }
    }
}

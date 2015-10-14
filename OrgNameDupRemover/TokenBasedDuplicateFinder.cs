using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrgNameDupRemover.Similarity;
using OrgNameDupRemover.Tokenizing;

namespace OrgNameDupRemover
{
    /// <summary>
    /// Finds duplicates in a list of tokenized strings
    /// </summary>
    public class TokenBasedDuplicateFinder
    {
        private readonly ITokenBasedSimilarity _similarity;

        public TokenBasedDuplicateFinder(ITokenBasedSimilarity similarity)
        {
            _similarity = similarity;
        }

        /// <summary>
        /// Processes a collection of tokenized strins and groups terms that are similar. It uses
        /// a token based similarity measure and puts two strings in a group if their similarity is 
        /// not less than given threshold. Strings can only be in one group and transitive similarity is 
        /// not applied. Strings are removed from the list as soon as they are detected to be a dup of 
        /// another string. Because of the nature of similarity measures, order of strings in the input
        /// list can affect the result.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="similarityThreshold"></param>
        /// <returns></returns>
        public List<List<TokenizedString>> Find(List<TokenizedString> collection, double similarityThreshold)
        {
            var termsDictionary = GetTokenTermsDictionary(collection);
            var processed = new HashSet<TokenizedString>();
            var finalList = new List<List<TokenizedString>>();
            //TODO: good candidate for parallelization
            foreach (var term in collection)
            {
                if (processed.Contains(term)) continue;
                var thisSet = new List<TokenizedString> {term};
                //check terms with at least one common token
                foreach (var candicate in term.SelectMany(t => termsDictionary[t]).Where(t => t != term).Distinct())
                {
                    if (_similarity.GetScore(term, candicate) < similarityThreshold) continue;

                    processed.Add(candicate); //no transitive similarity
                    thisSet.Add(candicate);
                }
                processed.Add(term);                
                finalList.Add(thisSet);
            }

            return finalList;
        }
        
        /// <summary>
        /// Creates a mapping between tokens and lists of strings that include those tokens
        /// </summary>
        /// <param name="terms"></param>
        /// <returns></returns>
        private Dictionary<Token, List<TokenizedString>> GetTokenTermsDictionary(IEnumerable<TokenizedString> terms)
        {
            var dict = new Dictionary<Token, List<TokenizedString>>();
            foreach (var term in terms)
            {
                foreach (var token in term)
                {
                    List<TokenizedString> termList;
                    if (!dict.TryGetValue(token, out termList))
                    {
                        termList = new List<TokenizedString>();
                        dict[token] = termList;
                    }
                    termList.Add(term);
                }
            }
            return dict;
        }                       
    }
}

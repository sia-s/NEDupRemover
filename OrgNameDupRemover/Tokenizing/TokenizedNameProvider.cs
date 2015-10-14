using System.Collections.Generic;
using System.Linq;
using OrgNameDupRemover.Preprocessing;

namespace OrgNameDupRemover.Tokenizing
{
    /// <summary>
    /// Creates tokenized version of strings after applying given preprocessing steps
    /// </summary>
    public class TokenizedNameProvider
    {
        private readonly ITokenizer _tokenizer;
        private readonly List<ITokenTransformer> _tokenTransformers;
        private readonly List<IStringNameOptimizer> _optimizers;

        public TokenizedNameProvider(ITokenizer tokenizer, List<ITokenTransformer> tokenTransformers,
            List<IStringNameOptimizer> optimizers)
        {
            _tokenizer = tokenizer;
            _tokenTransformers = tokenTransformers;
            _optimizers = optimizers;
        }

        /// <summary>
        /// Applies oprimizers to the strings, tokenizes them and applies transforms to tokens to get
        /// final tokenized strings
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        public List<TokenizedString> GetTokenizedNames(IEnumerable<string> names)
        {
            var list = new List<TokenizedString>();
            foreach (var name in names)
            {
                string optimized = _optimizers.Aggregate(name, (current, optimizer) => optimizer.Optimize(current));
                var tokens = _tokenizer.Tokenize(name, optimized);
                list.Add(new TokenizedString(
                    tokens.Original, 
                    tokens.Select(token => 
                            _tokenTransformers.Aggregate(token, (current, transformer) => transformer.Transform(current)))
                          .ToArray()));                                    
            }
            return list;
        }       
    }
}

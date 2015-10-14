using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OrgNameDupRemover.Tokenizing;

namespace OrgNameDupRemover.Similarity
{
    /// <summary>
    /// Contains a set of tokens and a weight for each
    /// </summary>
    public class BagOfTokens : IEnumerable<Token>
    {
        private readonly Dictionary<Token, double> _weights = new Dictionary<Token, double>();

        public double TotalWeight { get; protected set; }

        public BagOfTokens(IEnumerable<Token> tokens)
        {
            foreach (var token in tokens)
            {
                double w = 0.0;
                _weights.TryGetValue(token, out w);
                _weights[token] = w + 1;
            }
            TotalWeight = tokens.Count();
        }

        /// <summary>
        /// Get assigned weight for the token. If token is not found 0 is returned.
        /// Weights are initially set to token frequencies
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public double GetWeight(Token token)
        {
            double w;
            return _weights.TryGetValue(token, out w)
                ? w
                : 0.0;
        }

        /// <summary>
        /// Set weight of a token 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="d"></param>
        public void SetWeight(Token token, double d)
        {
            //TODO: throw exception when token is not found
            double oldWeight;
            TotalWeight += _weights.TryGetValue(token, out oldWeight) ? (d - oldWeight) : d;
            _weights[token] = d;
        }

        public IEnumerator<Token> GetEnumerator()
        {
            return _weights.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace OrgNameDupRemover
{
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

        public double GetWeight(Token token)
        {
            double w;
            return _weights.TryGetValue(token, out w)
                ? w
                : 0.0;
        }

        public void SetWeight(Token token, double d)
        {
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

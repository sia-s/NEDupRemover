using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgNameDupRemover
{
    public class SoftTfIdf : ITokenBasedSimilarity
    {
        private readonly TokenStatistics _tokenStatistics;
        private readonly double _tokenMatchThreshold;
        private readonly ITokenDistance _distance;

        public SoftTfIdf(TokenStatistics tokenStatistics, ITokenDistance distance, double tokenMatchThreshold)
        {
            _tokenStatistics = tokenStatistics;
            _tokenMatchThreshold = tokenMatchThreshold;
            _distance = distance;
        }

        public double GetScore(TokenizedString source, TokenizedString target)
        {
            var sVector = new TokenUnitVector(source, _tokenStatistics);
            var tVector = new TokenUnitVector(target, _tokenStatistics);
            
            double sim = 0.0;
            foreach (var sToken in sVector)
            {
                if (tVector.Contains(sToken))
                {
                    sim += sVector.GetWeight(sToken) * tVector.GetWeight(sToken);
                }
                else
                {
                    // find best matching token
                    var matchToken = tVector
                        .Select(t => new {Token = t, Score = _distance.Score(sToken, t)})
                        .Where(t => t.Score >= _tokenMatchThreshold)
                        .OrderByDescending(t => t.Score)
                        .FirstOrDefault();

                    if (matchToken != null)
                    {
                        sim += sVector.GetWeight(sToken) * tVector.GetWeight(matchToken.Token) * matchToken.Score;
                    }
                }
            }

            return sim;
        }

        class TokenUnitVector : BagOfTokens
        {
            public TokenUnitVector(IEnumerable<Token> tokens, TokenStatistics tokenStatistics)
                : base(tokens)
            {
                CalculateTfIdfs(tokenStatistics);
            }

            private void CalculateTfIdfs(TokenStatistics tokenStatistics)
            {
                double normalizer = 0.0;
                double numDocuments = tokenStatistics.NumberOfDucoments;

                foreach (var token in this.ToList())
                {
                    if (numDocuments > 0)
                    {
                        int df = tokenStatistics.GetDocumentFrequency(token) ?? 1;
                        double w = Math.Log(GetWeight(token) + 1) * Math.Log(numDocuments / df);
                        SetWeight(token, w);
                        normalizer += w * w;
                    }
                    else
                    {
                        SetWeight(token, 1.0);
                        normalizer += 1.0;
                    }
                }
                normalizer = Math.Sqrt(normalizer);
                foreach (var token in this.ToList())
                {
                    SetWeight(token, GetWeight(token) / normalizer);
                }                
            }
        }
    }
}

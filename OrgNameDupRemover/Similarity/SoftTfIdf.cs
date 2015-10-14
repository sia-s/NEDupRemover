﻿using System;
using System.Collections.Generic;
using System.Linq;
using OrgNameDupRemover.Tokenizing;

namespace OrgNameDupRemover.Similarity
{
    /// <summary>
    /// Calculates similarity between two sets of tokens using soft TF-IDF algorithm
    /// Implementation is translated and revised from SecondString see: <a href="http://secondstring.sourceforge.net/"></a>
    /// </summary>
    public class SoftTfIdf : ITokenBasedSimilarity
    {
        private const double DefaultTokenMatchThreshold = 0.95;

        private readonly TokenStatistics _tokenStatistics;
        private readonly double _tokenMatchThreshold;
        private readonly ITokenDistance _distance;
        private readonly bool _alwaysCompareLongerToShorter;

        public SoftTfIdf(TokenStatistics tokenStatistics, ITokenDistance distance)
            : this(tokenStatistics, distance, DefaultTokenMatchThreshold, false)
        {
        }

        public SoftTfIdf(TokenStatistics tokenStatistics, ITokenDistance distance, bool alwaysCompareLongerToShorter)
            : this(tokenStatistics, distance, DefaultTokenMatchThreshold, alwaysCompareLongerToShorter)
        {
        }

        public SoftTfIdf(TokenStatistics tokenStatistics, ITokenDistance distance, double tokenMatchThreshold)
            : this(tokenStatistics, distance, tokenMatchThreshold, false)
        {
        }

        public SoftTfIdf(TokenStatistics tokenStatistics, ITokenDistance distance, double tokenMatchThreshold, bool alwaysCompareLongerToShorter)
        {
            _tokenStatistics = tokenStatistics;
            _tokenMatchThreshold = tokenMatchThreshold;
            _alwaysCompareLongerToShorter = alwaysCompareLongerToShorter;
            _distance = distance;
        }

        /// <summary>
        /// Calculates a score between 0.0 (completely different) and 1.0 (the same). Two token lists are
        /// concerted to unit vector weights and Soft TF-IDF algoeithm is applied to them
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public double GetScore(TokenizedString source, TokenizedString target)
        {
            if (_alwaysCompareLongerToShorter && source.Count < target.Count)
            {
                var temp = source;
                source = target;
                target = temp;
            }

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

        public override string ToString()
        {
            return "SoftTFIDF with internal threshold=" + _tokenMatchThreshold;
        }
        
        /// <summary>
        /// A representation of tokenized strings which calculates a unit vector 
        /// from term frequencies and inverse document frequencies of tokens 
        /// </summary>
        class TokenUnitVector : BagOfTokens
        {
            public TokenUnitVector(IEnumerable<Token> tokens, TokenStatistics tokenStatistics)
                : base(tokens)
            {
                CalculateTfIdfs(tokenStatistics);
            }

            /// <summary>
            /// Calculates the unit vector
            /// </summary>
            /// <param name="tokenStatistics"></param>
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

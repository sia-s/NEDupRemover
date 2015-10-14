using OrgNameDupRemover.Tokenizing;

namespace OrgNameDupRemover.Similarity
{
    /// <summary>
    /// Computes similarity only by measuring distance between original string values
    /// </summary>
    public class DistanceOnlySimilarity : ITokenBasedSimilarity
    {
        private readonly ITokenDistance _distance;

        public DistanceOnlySimilarity(ITokenDistance distance)
        {
            _distance = distance;
        }

        /// <summary>
        /// Calculates similarity score which is between 0.0 and 1.0
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public double GetScore(TokenizedString source, TokenizedString target)
        {
            return _distance.Score(new Token(source.Original), new Token(target.Original));
        }

        public override string ToString()
        {
            return "Distance only similarity with " + _distance.GetType().Name + " metric";
        }
    }
}

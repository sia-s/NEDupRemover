using OrgNameDupRemover.Tokenizing;

namespace OrgNameDupRemover.Similarity
{
    /// <summary>
    /// Calculates similarity between two strings using Jaro Winkler algorithm
    /// </summary>
    public class JaroWinklerDistance : ITokenDistance
    {
        private readonly SimMetricsMetricUtilities.JaroWinkler _distance = new SimMetricsMetricUtilities.JaroWinkler();

        /// <summary>
        /// REturns a number between 0.0 (entirely different) to 1.0 (exactly the same)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public double Score(Token source, Token target)
        {
            return _distance.GetSimilarity(source.Value, target.Value);            
        }
    }
}

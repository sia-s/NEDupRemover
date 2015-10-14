namespace OrgNameDupRemover.Preprocessing
{
    /// <summary>
    /// Convers a domain name to a set of words by inserting spaces
    /// </summary>
    public class DomainnameOptimizer : IStringNameOptimizer
    {
        /// <summary>
        /// Using a dictionary finds best sequennce of words that would make the domain name
        /// if they were concatenated without spaces.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public string Optimize(string target)
        {
            //TODO: if target is domain name break it into words using a dictionary and viterbi algorithm
            return target;
        }
    }
}

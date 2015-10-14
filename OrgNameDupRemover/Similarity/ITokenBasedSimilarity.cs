using OrgNameDupRemover.Tokenizing;

namespace OrgNameDupRemover.Similarity
{
    /// <summary>
    /// A similarity measure calculator which works on tokens 
    /// </summary>
    public interface ITokenBasedSimilarity
    {
        double GetScore(TokenizedString source, TokenizedString target);
    }
}

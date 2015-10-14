using OrgNameDupRemover.Tokenizing;

namespace OrgNameDupRemover.Similarity
{
    /// <summary>
    /// A string distance calculator which returns a score between 0.0 and 1.0
    /// from completely different to exactly the same tokens
    /// </summary>
    public interface ITokenDistance
    {
        double Score(Token s, Token t);
    }
}

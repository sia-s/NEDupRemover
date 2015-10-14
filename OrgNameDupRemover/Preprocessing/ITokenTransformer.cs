using OrgNameDupRemover.Tokenizing;

namespace OrgNameDupRemover.Preprocessing
{
    /// <summary>
    /// An operation on a single token which changes the litteral of it or entirely 
    /// replaces it with another litteral
    /// </summary>
    public interface ITokenTransformer
    {
        Token Transform(Token token);
    }
}

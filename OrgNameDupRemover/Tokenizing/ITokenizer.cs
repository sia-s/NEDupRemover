namespace OrgNameDupRemover.Tokenizing
{
    /// <summary>
    /// Convers a string to a list of tokens
    /// </summary>
    public interface ITokenizer
    {
        TokenizedString Tokenize(string str);
        TokenizedString Tokenize(string original, string surrogate);
    }
}

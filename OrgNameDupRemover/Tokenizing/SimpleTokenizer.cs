using System.Linq;

namespace OrgNameDupRemover.Tokenizing
{
    /// <summary>
    /// Simple delimiter tokenization
    /// </summary>
    public class SimpleTokenizer : ITokenizer
    {
        private static readonly char[] DefaultDelimiters = {' ', '.' , ','};

        private readonly char[] _delimiters;

        public SimpleTokenizer() : this(DefaultDelimiters)
        {}

        public SimpleTokenizer(char[] delimiters)
        {
            _delimiters = delimiters;
        }

        /// <summary>
        /// Splits the string into a list of tokes wrapped in <see cref="TokenizedString"/>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public TokenizedString Tokenize(string str)
        {
            return new TokenizedString(str, GetTokens(str));
        }

        /// <summary>
        /// Stores original in the result but uses surrogate to find tokens
        /// </summary>
        /// <param name="original"></param>
        /// <param name="surrogate"></param>
        /// <returns></returns>
        public TokenizedString Tokenize(string original, string surrogate)
        {
            return new TokenizedString(original, GetTokens(surrogate));
        }

        private Token[] GetTokens(string str)
        {
            return str.Split(_delimiters)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => new Token(s.Trim().ToLower()))
                .ToArray();
        }
    }
}

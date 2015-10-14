using System.Collections.Generic;
using OrgNameDupRemover.Tokenizing;

namespace OrgNameDupRemover.Preprocessing
{
    /// <summary>
    /// Transforms abbreviations to complete words
    /// </summary>
    public class AbbreviationTransformer : ITokenTransformer
    {
        //TODO: read from file
        private static readonly Dictionary<string, string> Abbreviations = new Dictionary<string, string>
        {
            {"inc", "incorporated"},
            {"st", "saint"},
            {"ltd", "limited"}
        };

        /// <summary>
        /// if the token contains a defined abbreviation, returned token will
        /// contain the term wich was abbreviated. Otherwise same token is 
        /// returned
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Token Transform(Token token)
        {
            string replacement;
            return Abbreviations.TryGetValue(token.Value, out replacement)
                ? new Token(replacement)
                : token;
        }
    }
}

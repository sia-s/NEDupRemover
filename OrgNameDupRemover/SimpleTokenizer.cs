using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgNameDupRemover
{
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

        public TokenizedString Tokenize(string str)
        {
            return 
                new TokenizedString(str,
                str.Split(DefaultDelimiters)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => new Token(s.Trim().ToLower()))
                .ToArray());
        }
    }
}

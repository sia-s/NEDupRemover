using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OrgNameDupRemover.Tokenizing
{
    /// <summary>
    /// A Wrapper for tokenized strings
    /// </summary>
    public class TokenizedString : IEnumerable<Token>
    {
        private readonly Token[] _tokens;
        
        public string Original { get; private set; }
        public int Count { get { return _tokens.Length; } }

        public TokenizedString(string str, Token[] tokens)
        {
            Original = str;
            _tokens = tokens;
        }

        public virtual IEnumerator<Token> GetEnumerator()
        {
            return _tokens.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public override int GetHashCode()
        {
            return Original.GetHashCode();
        }

        public override string ToString()
        {
            return Original;
        }
    }
}

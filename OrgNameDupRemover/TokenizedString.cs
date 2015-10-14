using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgNameDupRemover
{
    public class TokenizedString : IEnumerable<Token>
    {
        private readonly Token[] _tokens;
        private string _str;
        
        public TokenizedString(string str, Token[] tokens)
        {
            _str = str;
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
    }
}

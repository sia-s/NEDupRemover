using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgNameDupRemover
{
    public interface ITokenizer
    {
        TokenizedString Tokenize(string str);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgNameDupRemover
{
    public interface ITokenDistance
    {
        double Score(Token s, Token t);
    }
}

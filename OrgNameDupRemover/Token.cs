using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgNameDupRemover
{
    public class Token
    {
        public string Value {get; protected set; }

        public Token(string value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            return Value.Equals((obj as Token).Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}

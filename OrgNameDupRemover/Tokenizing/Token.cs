namespace OrgNameDupRemover.Tokenizing
{
    /// <summary>
    /// A single token in a string
    /// </summary>
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

        public override string ToString()
        {
            return Value;
        }
    }
}

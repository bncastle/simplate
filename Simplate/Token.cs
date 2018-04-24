namespace Pixelbyte.CodeGen
{
    public class Token
    {
        public TokenType type;
        public string name = null;
        public ITemplateElement[] parameters = null;
        public string indentation;

        public Token() { }
        public Token(TokenType tokenType) { type = tokenType; }
        public Token(TokenType tokenType, string name) { type = tokenType; this.name = name; }
        public Token(TokenType tokenType, string name, ITemplateElement[] parameters) { type = tokenType; this.name = name; this.parameters = parameters; }

        public bool IsFunction { get { return (!string.IsNullOrEmpty(name) && parameters != null); } }
    }
}

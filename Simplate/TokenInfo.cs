namespace Pixelbyte.Simplate
{
    public class TokenInfo
    {
        public TokenType type;
        public string name = null;
        public ITemplateElement[] parameters = null;
        public string indentation;

        public TokenInfo() { }
        public TokenInfo(TokenType tokenType) { type = tokenType; }
        public TokenInfo(TokenType tokenType, string name) { type = tokenType; this.name = name; }
        public TokenInfo(TokenType tokenType, string name, ITemplateElement[] parameters) { type = tokenType; this.name = name; this.parameters = parameters; }

        public bool IsFunction { get { return (!string.IsNullOrEmpty(name) && parameters != null); } }
    }
}

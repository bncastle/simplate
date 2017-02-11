namespace Pixelbyte.Simplate
{
    public enum TokenType
    {
        None,
        ParenOpen, ParenClose,
        DoubleQuote,
        BeginCommand, ExitCommand,
        Function, Word, In,
        ForEach, End,
        If //TODO: Implement
    }
}

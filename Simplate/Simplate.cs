using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pixelbyte.Simplate
{
    /// <summary>
    /// Simplate - A very simple template system by Pixelbyte Studios
    ///  Give it a text template and compile it using Compile()
    ///  Once that is done you can get output from it by giving it a Functor class object
    ///  and a parameters dictionary which should have all and any variable items in it you wish to
    ///  use in the template
    /// </summary>
    public class Simplate : ITemplateElement
    {
        const string NEWLINE = "\r\n";
        const string SPACE = " \t";
        const string WHITESPACE = NEWLINE + SPACE;
        const string WORD_BREAKS = WHITESPACE + "(){}[]";

        int line;  //Current line in the template
        int col;   //Current column in the template

        TextReader reader;
        char currentchar, prevChar;

        //Parser State stuff
        bool inCommand = false;

        //This list will contain the compiled template elements
        List<ITemplateElement> elements;

        char PeekChar { get { return Convert.ToChar(reader.Peek()); } }
        void NextChar() { if (!Eof && PeekChar == '\n') { line++; col = 0; } else col++; prevChar = currentchar; currentchar = Convert.ToChar(reader.Read()); }

        bool Eof { get { return reader.Peek() == -1; } }

        private Simplate(TextReader rdr)
        {
            reader = rdr;
            line = 0;
            col = 0;
        }

        public static Simplate Compile(TextReader tr)
        {
            Simplate tk = new Simplate(tr);
            tk.Compile();
            return tk;
        }

        public static Simplate Compile(string template)
        {
            using (var sr = new StringReader(template))
                return Compile(sr);
        }

        void Compile()
        {
            if (reader == null)
                throw new Exception("TextReader cannot be null!");

            elements = new List<ITemplateElement>();

            TokenInfo t;
            var sb = new StringBuilder();
            NextChar();
            while (!Eof)
            {
                t = GetToken();
                switch (t.type)
                {
                    case TokenType.BeginCommand:
                        ITemplateElement elem = ParseCommand();

                        if (elem != null)
                        {
                            //Anything except a ForEachelement, add
                            //the current acquired characters to our compiled template
                            if (!(elem is ForEachElement))
                                elements.Add(new TextElement(sb.ToString()));
                            elements.Add(elem);
                        }
                        sb.Length = 0;
                        break;
                    case TokenType.None:
                        sb.Append(currentchar);
                        if (currentchar == '\n')
                        {
                            elements.Add(new TextElement(sb.ToString()));
                            sb.Length = 0;
                        }
                        NextChar();
                        break;
                    default:
                        NextChar();
                        break;
                }
            }

            //Anything left is probably a plain text command
            sb.Append(currentchar);
            if (sb.Length > 0)
            {
                // Console.Write(sb.ToString());
                //Store the text element away for later
                elements.Add(new TextElement(sb.ToString()));
                sb.Length = 0;
            }
        }

        ITemplateElement ParseCommand()
        {
            TokenInfo t;
            ITemplateElement element = null;

            do
            {
                t = GetToken();
                switch (t.type)
                {
                    case TokenType.Function:
                        element = new MethodElement(t.name, t.parameters);
                        break;
                    case TokenType.Word:
                        //Assuming since it isnt a keyword or a function that it is a variable
                        element = new TextElement(t.name, true);
                        break;
                    case TokenType.ForEach:
                        element = ForEach();
                        return element;
                    case TokenType.End:
                        element = new EndBlockElement(t.name);
                        break;
                }
            } while (t.type != TokenType.ExitCommand);
            return element;
        }

        TokenInfo FunctionOrWord()
        {
            TokenInfo ti = new TokenInfo(TokenType.Word, ParseWord());

            //Eat whitespace if there is any
            EatWhitespace();

            //Looks like we have a function
            if (currentchar == '(')
            {
                NextChar();
                ti.parameters = GetParenContents();
                ti.type = TokenType.Function;
            }
            return ti;
        }

        ITemplateElement[] GetParenContents()
        {
            TokenInfo t;
            int depth = 1;
            List<ITemplateElement> paramElements = new List<ITemplateElement>();
            StringBuilder sb = new StringBuilder();

            while (!Eof)
            {
                t = GetToken();
                switch (t.type)
                {
                    case TokenType.None:
                        if (NEWLINE.IndexOf(currentchar) == -1) sb.Append(currentchar);
                        break;
                    case TokenType.ParenOpen:
                        depth++;
                        break;
                    case TokenType.ParenClose:
                        NextChar();
                        depth--;
                        if (depth == 0) return paramElements.ToArray();
                        break;
                    case TokenType.Function:
                        paramElements.Add(new MethodElement(t.name, t.parameters));
                        break;
                    case TokenType.Word:
                        //If this is the name of an iterator variable, treat it as such
                        bool isVariable = !string.IsNullOrEmpty(forEachIterator) && t.name == forEachIterator;
                        paramElements.Add(new TextElement(t.name, isVariable));
                        break;
                    default:
                        NextChar();
                        break;
                }
            }
            throw new Exception("Unable to find paren contents!");
        }

        string ParseWord()
        {
            StringBuilder sb = new StringBuilder();
            while (!Eof && WORD_BREAKS.IndexOf(currentchar) == -1)
            {
                sb.Append(currentchar);
                NextChar();
            }
            return sb.ToString();
        }

        //This lets us keep track of the iterator variable which we will look for
        //in other methods so we can appropriately replace it with its value
        string forEachIterator = null;
        ITemplateElement ForEach()
        {
            TokenInfo t;
            string collectionName = null;
            bool foundIn = false;
            StringBuilder sb = new StringBuilder();
            List<ITemplateElement> foreachElements = new List<ITemplateElement>();
            do
            {
                t = GetToken();
                switch (t.type)
                {
                    case TokenType.Word:
                        if (foundIn)
                            collectionName = t.name;
                        else
                        {
                            forEachIterator = t.name;
                        }
                        break;
                    case TokenType.In: foundIn = true; break;
                    case TokenType.ExitCommand: EatNewlines(); break;
                    case TokenType.BeginCommand:
                        //Look in the command
                        ITemplateElement templateElement = ParseCommand();

                        EndBlockElement ce = templateElement as EndBlockElement;

                        if (ce != null && ce.type == EndBlockType.ForEach) { t.type = TokenType.End; EatNewlines(); }
                        else
                        {
                            if (sb.Length > 0) foreachElements.Add(new TextElement(sb.ToString()));
                            foreachElements.Add(templateElement);
                        }
                        sb.Length = 0;
                        break;
                    case TokenType.End: break;
                    default:
                        sb.Append(currentchar);
                        if (currentchar == '\n')
                        {
                            foreachElements.Add(new TextElement(sb.ToString()));
                            sb.Length = 0;
                        }
                        NextChar();
                        break;
                }

            } while (!Eof && t.type != TokenType.End);

            if (string.IsNullOrEmpty(collectionName))
                throw new Exception("foreach requires an iterable item! ex: {{foreach item in items}} where items is an iterable parameter");
            if (string.IsNullOrEmpty(forEachIterator))
                throw new Exception("foreach requires an iterable item variable Name! ex: {{foreach item in items}} where item is the object within the collection");

            //Now add this ForEach to the template
            var fei = new ForEachElement(collectionName, forEachIterator, foreachElements);
            forEachIterator = null;
            return fei;
        }

        TokenInfo GetToken()
        {
            switch (currentchar)
            {
                case '{': if (PeekChar == '{') { NextChar(); NextChar(); inCommand = true; return new TokenInfo(TokenType.BeginCommand); } break;
            }

            if (inCommand)
            {
                //Whitespace is not important in a command
                EatWhitespace();

                switch (currentchar)
                {
                    case '(': return new TokenInfo(TokenType.ParenOpen);
                    case ')': return new TokenInfo(TokenType.ParenClose);
                    case '}': if (PeekChar == '}') { NextChar(); NextChar(); inCommand = false; return new TokenInfo(TokenType.ExitCommand); } break;
                }

                //Whitespace is not important in a command
                EatWhitespace();

                TokenInfo info = FunctionOrWord();

                //Not a function, must be a word
                if (!info.IsFunction)
                {
                    switch (info.name.ToLower())
                    {
                        case "foreach": info.type = TokenType.ForEach; break;
                        case "end": info.type = TokenType.End; info.name = ParseWord(); break;
                        case "if": info.type = TokenType.If; break;
                        case "in": info.type = TokenType.In; break;
                        default: info.type = TokenType.Word; break;
                    }
                }
                return info;
            }
            return new TokenInfo(TokenType.None);
        }

        void EatWhitespace() { while (!Eof && WHITESPACE.IndexOf(currentchar) > -1) NextChar(); }

        bool IsWhiteSpace(StringBuilder sb)
        {
            for (int i = 0; i < sb.Length; i++)
            {
                if (sb[i] != ' ' && sb[i] != '\t') return false;
            }
            return true;
        }

        void EatNewlines() { while (!Eof && NEWLINE.IndexOf(currentchar) > -1) NextChar(); }

        //Parses a double-quoted string
        //
        string ParseString()
        {
            //Eat the opening quote
            NextChar();

            StringBuilder sb = new StringBuilder();
            while (!Eof && PeekChar != '"')
            {
                //grab the next char
                NextChar();


                //TODO: Check for special chars, escape sequences etc??
                switch (currentchar)
                {
                    case '\\': //Escape sequence
                        if (Eof) break;
                        NextChar();
                        if (currentchar == '"')
                            sb.Append(currentchar);
                        else
                            throw new Exception("Unrecognized escape sequence: line: " + line.ToString() + " col: " + col.ToString());
                        break;
                    default:
                        sb.Append(currentchar);
                        break;
                }
            }
            return sb.ToString();
        }

        public string GetOutput(Dictionary<string, object> parameters, Functors functors)
        {
            if (elements == null || elements.Count == 0)
                throw new Exception("No elements to process. Has this template been compiled?");

            var sb = new StringBuilder();
            foreach (var element in elements)
            {
                sb.Append(element.GetOutput(parameters, functors));
            }
            return sb.ToString();
        }
    }
}

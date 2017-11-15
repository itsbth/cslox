namespace CSLox
{
    using System;
    using System.Collections.Generic;
    using static TokenType;

    internal class Scanner
    {
        private static readonly Dictionary<string, TokenType> Keywords = new Dictionary<string, TokenType>
        {
            { "and", AND },
            { "class", CLASS },
            { "else", ELSE },
            { "false", FALSE },
            { "for", FOR },
            { "fun", FUN },
            { "if", IF },
            { "nil", NIL },
            { "or", OR },
            { "print", PRINT },
            { "return", RETURN },
            { "super", SUPER },
            { "this", THIS },
            { "true", TRUE },
            { "var", VAR },
            { "while", WHILE },
        };

        private string source;
        private List<Token> tokens = new List<Token>();
        private int start = 0;
        private int current = 0;
        private int line = 1;

        public Scanner(string source)
        {
            this.source = source;
        }

        internal List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                start = current;
                ScanToken();
            }

            tokens.Add(new Token(EOF, string.Empty, null, line));
            return tokens;
        }

        private void ScanToken()
        {
            char c = Advance();
            switch (c)
            {
                case '(': AddToken(LEFT_PAREN); break;
                case ')': AddToken(RIGHT_PAREN); break;
                case '{': AddToken(LEFT_BRACE); break;
                case '}': AddToken(RIGHT_BRACE); break;
                case ',': AddToken(COMMA); break;
                case '.': AddToken(DOT); break;
                case '-': AddToken(MINUS); break;
                case '+': AddToken(PLUS); break;
                case ';': AddToken(SEMICOLON); break;
                case '*': AddToken(STAR); break;
                case '?': AddToken(QUESTION); break;
                case ':': AddToken(COLON); break;

                case '!': AddToken(Match('=') ? BANG_EQUAL : BANG); break;
                case '=': AddToken(Match('=') ? EQUAL_EQUAL : EQUAL); break;
                case '<': AddToken(Match('=') ? LESS_EQUAL : LESS); break;
                case '>': AddToken(Match('=') ? GREATER_EQUAL : GREATER); break;
                case '/':
                    if (Match('/'))
                    {
                        while (Peek() != '\n' && !IsAtEnd())
                        {
                            Advance();
                        }
                    }
                    else if (Match('*'))
                    {
                        MultilineComment();
                    }
                    else
                    {
                        AddToken(SLASH);
                    }

                    break;
                case ' ':
                case '\r':
                case '\t':
                    break;
                case '\n':
                    line++;
                    break;
                case '"':
                    String();
                    break;
                default:
                    if (char.IsDigit(c))
                    {
                        Number();
                    }
                    else if (char.IsLetter(c))
                    {
                        Identifier();
                    }
                    else
                    {
                        Program.Error(line, $"Unexpected character '{c}'.");
                    }

                    break;
            }
        }

        private void MultilineComment()
        {
            int level = 1;
            while (level > 0 && !IsAtEnd())
            {
                if (Match('/') && Match('*'))
                {
                    level += 1;
                }
                else if (Match('*') && Match('/'))
                {
                    level -= 1;
                }
                else
                {
                    if (Peek() == '\n')
                    {
                        line += 1;
                    }

                    Advance();
                }
            }

            if (level != 0)
            {
                Program.Error(line, "Unterminated comment.");
            }
        }

        private void String()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n')
                {
                    line++;
                }

                Advance();
            }

            if (IsAtEnd())
            {
                Program.Error(line, "Unterminated string.");
                return;
            }

            Advance();
            AddToken(STRING, source.Substring(start + 1, current - start - 2));
        }

        private void Number()
        {
            while (char.IsDigit(Peek()))
            {
                Advance();
            }

            if (Peek() == '.' && char.IsDigit(PeekNext()))
            {
                Advance();
                while (char.IsDigit(Peek()))
                {
                    Advance();
                }
            }

            AddToken(NUMBER, double.Parse(source.Substring(start, current - start)));
        }

        private void Identifier()
        {
            while (char.IsLetterOrDigit(Peek()))
            {
                Advance();
            }

            var ident = source.Substring(start, current - start);
            AddToken(Keywords.GetValueOrDefault(ident, IDENTIFIER));
        }

        private void AddToken(TokenType type, object literal = null)
        {
            tokens.Add(new Token(type, source.Substring(start, current - start), literal, line));
        }

        private char Peek()
        {
            if (IsAtEnd())
            {
                return '\0';
            }

            return source[current];
        }

        private char PeekNext()
        {
            if (current + 1 >= source.Length)
            {
                return '\0';
            }

            return source[current + 1];
        }

        private bool Match(char expected)
        {
            if (IsAtEnd())
            {
                return false;
            }

            if (Peek() == expected)
            {
                Advance();
                return true;
            }

            return false;
        }

        private char Advance() => source[current++];

        private bool IsAtEnd() => current >= source.Length;
    }
}
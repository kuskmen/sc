namespace sc
{
    using System;
    using System.IO;
    using System.Text;

    public class Lexer
    {
        const char EOF = '\u001a';
        const char CR = '\r';
        const char LF = '\n';
        const char Escape = '\\';
     
        static readonly string keywords =
            " typedef static void char bool short int long double signed unsigned struct " +
            " union enum case default if else switch while do for continue break return ";
        static readonly string specialSymbols1 =
            "(),;[]{}~?:.";
        static readonly string specialSymbols2 =
            "!&|+-<=>*%^/";
        static readonly string specialSymbols2Pairs =
            " != && || ++ -- <= == >= *= %= += -= &= ^= |= << >> /= ->";
        static readonly string specialSymbolsTriples =
            " <<= >>= ";

        private readonly TextReader reader;

        private char ch;
        private int line, column;

        public bool SkipComments { get; set; } = true;

        public Lexer(TextReader reader)
        {
            this.reader = reader;
            this.line = 1;
            this.column = 0;
            ReadNextChar();
        }

        public void ReadNextChar()
        {
            int ch1 = reader.Read();
            column++;
            ch = (ch1 < 0) ? EOF : (char)ch1;
            if (ch == CR)
            {
                line++;
                column = 0;
            }
            else if (ch == LF)
            {
                column = 0;
            }
        }

        public char UnEscape(char c)
        {
            return c switch
            {
                't' => '\t',
                'n' => '\n',
                'r' => '\r',
                'f' => '\f',
                '\'' => '\'',
                '"' => '\"',
                '0' => '\0',
                Escape => Escape,
                _ => c,
            };
        }

        public SyntaxToken Lex()
        {
            while (true)
            {
                int start_column = column;
                int start_line = line;

                // Letter
                if (ch >= 'a' && ch <= 'z' || ch >= 'A' && ch <= 'Z' || ch == '_' || ch == '.')
                {
                    var sb = new StringBuilder();
                    while (ch >= 'a' && ch <= 'z' || ch >= 'A' && ch <= 'Z' || ch == '_' || ch == '.' || ch >= '0' && ch <= '9')
                    {
                        sb.Append(ch);
                        ReadNextChar();
                    }
                    string id = sb.ToString();
                    if (id.Equals("false") || id.Equals("true"))
                    {
                        return SyntaxTokenFactory.Create(start_line, start_column, id);
                    }
                    else if (keywords.Contains(" " + id + " "))
                    {
                        return SyntaxTokenFactory.Create(start_line, start_column, id);
                    }
                    return SyntaxTokenFactory.CreateWithValue(start_line, start_column, SyntaxKind.IdentifierToken, id);
                }
                
                // Digit
                else if (ch >= '0' && ch <= '9')
                {
                    var sb = new StringBuilder();
                    while (ch >= '0' && ch <= '9')
                    {
                        sb.Append(ch);
                        ReadNextChar();
                    }
                    if (ch == '.')
                    {
                        sb.Append(ch);
                        ReadNextChar();
                        while (ch >= '0' && ch <= '9')
                        {
                            sb.Append(ch);
                            ReadNextChar();
                        }
                        return new SyntaxTokenWithValue<double>(start_line, start_column, SyntaxKind.DoubleToken, Convert.ToDouble(sb.ToString(), System.Globalization.NumberFormatInfo.InvariantInfo));
                    }
                    return new SyntaxTokenWithValue<int>(start_line, start_column, SyntaxKind.NumberToken, Convert.ToInt32(sb.ToString()));
                }

                // TODO: HexDigit
                
                // Escape
                else if (ch == '\'')
                {
                    ReadNextChar();
                    char ch1 = ch;
                    if (ch == Escape)
                    {
                        ReadNextChar();
                        ch1 = UnEscape(ch);
                    }
                    ReadNextChar();
                    if (ch == '\'') ReadNextChar();
                    return new SyntaxTokenWithValue<char>(start_line, start_column, SyntaxKind.CharToken, ch);
                }

                // String
                else if (ch == '"')
                {
                    var sb = new StringBuilder();
                    ReadNextChar();
                    while (ch != '"' && ch != EOF)
                    {
                        char ch1 = ch;
                        if (ch == Escape)
                        {
                            ReadNextChar();
                            ch1 = UnEscape(ch);
                        }
                        sb.Append(ch1);
                        ReadNextChar();
                    }
                    ReadNextChar();
                    return SyntaxTokenFactory.CreateWithValue(start_line, start_column, SyntaxKind.StringToken, sb.ToString());
                }

                // SpecialSymbol
                else if (specialSymbols1.Contains(ch.ToString()))
                {
                    char ch1 = ch;
                    ReadNextChar();
                    return SyntaxTokenFactory.Create(start_line, start_column, ch1);
                }
                else if (specialSymbols2.Contains(ch.ToString()))
                {
                    char ch1 = ch;
                    if (ch == '/')
                    {
                        ReadNextChar();
                        if (ch == '/')
                        {
                            if (SkipComments)
                            {
                                while (ch != CR && ch != LF && ch != EOF)
                                {
                                    ReadNextChar();
                                }
                                ReadNextChar();
                            }
                            else
                            {
                                var sb = new StringBuilder();
                                while (ch != CR && ch != LF && ch != EOF)
                                {
                                    ReadNextChar();
                                    sb.Append(ch);
                                }
                                ReadNextChar();
                                return SyntaxTokenFactory.Create(start_line, start_column, "//");
                            }
                        }
                        else if (ch == '*')
                        {
                            if (SkipComments)
                            {
                                ReadNextChar();
                                do
                                {
                                    while (ch != '*' && ch != EOF)
                                    {
                                        ReadNextChar();
                                    }
                                    ReadNextChar();
                                } while (ch != '/' && ch != EOF);
                                ReadNextChar();
                            }
                            else
                            {
                                StringBuilder s = new StringBuilder();
                                ReadNextChar();
                                do
                                {
                                    while (ch != '*' && ch != EOF)
                                    {
                                        s.Append(ch);
                                        ReadNextChar();
                                    }
                                    ReadNextChar();
                                } while (ch != '/' && ch != EOF);
                                ReadNextChar();
                                return SyntaxTokenFactory.Create(start_line, start_column, "/*");
                            }
                        }
                        else return SyntaxTokenFactory.Create(start_line, start_column, ch1.ToString());
                    }
                    else
                    {
                        ReadNextChar();
                        char ch2 = ch;
                        if (specialSymbols2Pairs.Contains(" " + ch1 + ch2 + " "))
                        {
                            ReadNextChar();
                            char ch3 = ch;
                            if (specialSymbolsTriples.Contains(" " + ch1 + ch2 + ch3 + " "))
                            {
                                ReadNextChar();
                                return SyntaxTokenFactory.Create(start_line, start_column, ch1.ToString() + ch2 + ch3);
                            }
                            return SyntaxTokenFactory.Create(start_line, start_column, ch1.ToString() + ch2);
                        }
                        return SyntaxTokenFactory.Create(start_line, start_column, ch1.ToString());
                    }
                }

                // Space
                else if (ch == ' ' || ch == '\t' || ch == CR || ch == LF)
                {
                    ReadNextChar();
                    continue;
                }
                
                else if (ch == EOF)
                {
                    return SyntaxTokenFactory.Create(start_line, start_column, EOF);
                }
                else
                {
                    var s = ch.ToString();
                    ReadNextChar();
                    return SyntaxTokenFactory.Create(start_line, start_column, s);
                }
            }
        }
    }
}
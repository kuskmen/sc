namespace sc
{
    public static class SyntaxTokenFactory
    {
        public static SyntaxToken Create(int line, int column, char ch)
        {
            return ch switch
            {
                '!' => new SyntaxToken(line, column, SyntaxKind.ExclamationToken),
                '&' => new SyntaxToken(line, column, SyntaxKind.AmpersandToken),
                '|' => new SyntaxToken(line, column, SyntaxKind.BarToken),
                '+' => new SyntaxToken(line, column, SyntaxKind.PlusToken),
                '-' => new SyntaxToken(line, column, SyntaxKind.MinusToken),
                '<' => new SyntaxToken(line, column, SyntaxKind.LessThanToken),
                '=' => new SyntaxToken(line, column, SyntaxKind.EqualsToken),
                '>' => new SyntaxToken(line, column, SyntaxKind.GreaterThanToken),
                '*' => new SyntaxToken(line, column, SyntaxKind.AsteriskToken),
                '{' => new SyntaxToken(line, column, SyntaxKind.OpenBracketToken),
                '}' => new SyntaxToken(line, column, SyntaxKind.ClosingBracketToken),
                ';' => new SyntaxToken(line, column, SyntaxKind.SemiColonToken),
                ',' => new SyntaxToken(line, column, SyntaxKind.CommaToken),
                '(' => new SyntaxToken(line, column, SyntaxKind.OpenBraceToken),
                ')' => new SyntaxToken(line, column, SyntaxKind.ClosingBraceToken),
                '[' => new SyntaxToken(line, column, SyntaxKind.OpenSquareBracketToken),
                ']' => new SyntaxToken(line, column, SyntaxKind.ClosingSquareBracketToken),
                ':' => new SyntaxToken(line, column, SyntaxKind.ColonToken),
                '.' => new SyntaxToken(line, column, SyntaxKind.DotToken),
                '~' => new SyntaxToken(line, column, SyntaxKind.TildeToken),
                '%' => new SyntaxToken(line, column, SyntaxKind.PercentToken),
                '^' => new SyntaxToken(line, column, SyntaxKind.CaretToken),
                '/' => new SyntaxToken(line, column, SyntaxKind.SlashToken),
                '?' => new SyntaxToken(line, column, SyntaxKind.QuestionToken),
                '\u001a' => new SyntaxToken(line, column, SyntaxKind.EndOfFileToken),
                _ => new SyntaxToken(line, column, SyntaxKind.Unspecified),
            };
        }

        public static SyntaxToken Create(int line, int column, string text)
        {
            switch (text)
            {
                // boolean
                case "true":
                    return new SyntaxToken(line, column, SyntaxKind.FalseKeywordToken);
                case "false":
                    return new SyntaxToken(line, column, SyntaxKind.FalseKeywordToken);

                // keywords
                case "typedef":
                    return new SyntaxToken(line, column, SyntaxKind.TypedefKeyword);
                case "static":
                    return new SyntaxToken(line, column, SyntaxKind.StaticKeyword);
                case "struct":
                    return new SyntaxToken(line, column, SyntaxKind.StructKeyword);
                case "union":
                    return new SyntaxToken(line, column, SyntaxKind.UnionKeyword);
                case "enum":
                    return new SyntaxToken(line, column, SyntaxKind.EnumKeyword);
                case "case":
                    return new SyntaxToken(line, column, SyntaxKind.CaseKeyword);
                case "default":
                    return new SyntaxToken(line, column, SyntaxKind.DefaultKeyword);
                case "else":
                    return new SyntaxToken(line, column, SyntaxKind.ElseKeyword);
                case "if":
                    return new SyntaxToken(line, column, SyntaxKind.IfKeyword);
                case "do":
                    return new SyntaxToken(line, column, SyntaxKind.DoKeyword);
                case "while":
                    return new SyntaxToken(line, column, SyntaxKind.WhileKeyword);
                case "switch":
                    return new SyntaxToken(line, column, SyntaxKind.SwitchKeyword);
                case "return":
                    return new SyntaxToken(line, column, SyntaxKind.ReturnKeyword);
                case "break":
                    return new SyntaxToken(line, column, SyntaxKind.BreakKeyword);
                case "continue":
                    return new SyntaxToken(line, column, SyntaxKind.ContinueKeyword);
                case "for":
                    return new SyntaxToken(line, column, SyntaxKind.ForKeyword);


                // pairs
                case "//":
                case "/*":
                    return new SyntaxToken(line, column, SyntaxKind.CommentToken);
                case "==":
                    return new SyntaxToken(line, column, SyntaxKind.EqualsEqualsToken);
                case "++":
                    return new SyntaxToken(line, column, SyntaxKind.PlusPlusToken);
                case "--":
                    return new SyntaxToken(line, column, SyntaxKind.MinusMinusToken);
                case "<<":
                    return new SyntaxToken(line, column, SyntaxKind.LessThanLessThanToken);
                case ">>":
                    return new SyntaxToken(line, column, SyntaxKind.GreaterThanGreaterThanToken);
                case "->":
                    return new SyntaxToken(line, column, SyntaxKind.DashGreaterThanToken);
                case "/=":
                    return new SyntaxToken(line, column, SyntaxKind.DashEqualToken);
                case "|=":
                    return new SyntaxToken(line, column, SyntaxKind.BarEqualToken);
                case "&&":
                    return new SyntaxToken(line, column, SyntaxKind.AmpersandAmpersandToken);
                case "||":
                    return new SyntaxToken(line, column, SyntaxKind.BarBarToken);
                case "<=":
                    return new SyntaxToken(line, column, SyntaxKind.LessThanEqualToken);
                case ">=":
                    return new SyntaxToken(line, column, SyntaxKind.GreaterThanEqualToken);
                case "*=":
                    return new SyntaxToken(line, column, SyntaxKind.AsteriksEqualToken);
                case "%=":
                    return new SyntaxToken(line, column, SyntaxKind.PercentEqualToken);
                case "+=":
                    return new SyntaxToken(line, column, SyntaxKind.PlusEqualToken);
                case "-=":
                    return new SyntaxToken(line, column, SyntaxKind.MinusEqualToken);
                case "&=":
                    return new SyntaxToken(line, column, SyntaxKind.AmpersandEqualToken);
                case "!=":
                    return new SyntaxToken(line, column, SyntaxKind.ExclamationEqualToken);
                case "^=":
                    return new SyntaxToken(line, column, SyntaxKind.CaretEqualToken);

                // tripples
                case "<<=":
                    return new SyntaxToken(line, column, SyntaxKind.LessThanLessThanEqualToken);
                case ">>=":
                    return new SyntaxToken(line, column, SyntaxKind.GreaterThanGreaterThanEqualToken);
                case "void":
                case "char":
                case "bool":
                case "short":
                case "int":
                case "long":
                case "double":
                case "signed":
                case "unsigned":
                    return new SyntaxToken(line, column, SyntaxKind.KeywordToken);
                default:
                    return new SyntaxToken(line, column, SyntaxKind.Unspecified);
            }
        }

        public static SyntaxToken CreateWithValue<T>(int line, int column, SyntaxKind kind, T value)
        {
            return kind switch
            {
                SyntaxKind.StringToken => new SyntaxTokenWithValue<T>(line, column, SyntaxKind.StringToken, value),
                SyntaxKind.IdentifierToken => new SyntaxTokenWithValue<T>(line, column, SyntaxKind.IdentifierToken, value),
                _ => new SyntaxToken(line, column, SyntaxKind.Unspecified),
            };
        }
    }

    public enum SyntaxKind
    {
        Unspecified = -1,
        EndOfFileToken = 0,
        NumberToken = 2,
        DoubleToken = 3,
        KeywordToken = 4,
        CommentToken = 5,
        TrueKeywordToken = 6,
        FalseKeywordToken = 7,
        CharToken = 8,
        StringToken = 9,
        IdentifierToken = 10,
        StaticKeyword = 11,
        TypedefKeyword = 12,
        StructKeyword = 13,
        UnionKeyword = 14,
        EnumKeyword = 15,
        CaseKeyword = 16,
        DefaultKeyword = 17,
        IfKeyword = 18,
        ElseKeyword = 19,
        SwitchKeyword = 20,
        WhileKeyword = 21,
        DoKeyword = 22,
        ForKeyword = 23,
        ContinueKeyword = 24,
        BreakKeyword = 25,
        ReturnKeyword = 26,

        // token
        OpenBracketToken = 27,
        ClosingBracketToken = 28,
        SemiColonToken = 29,
        CommaToken = 30,
        OpenBraceToken = 31,
        ClosingBraceToken = 32,
        OpenSquareBracketToken = 33,
        ClosingSquareBracketToken = 34,
        EqualsToken = 35,
        AsteriskToken = 36,
        EqualsEqualsToken = 37,
        LessThanToken = 38,
        GreaterThanToken = 39,
        AmpersandToken = 40,
        ColonToken = 41,
        PlusPlusToken = 42,
        MinusMinusToken = 43,
        DotToken = 44,
        LessThanLessThanToken = 45,
        GreaterThanGreaterThanToken = 46,
        PlusToken = 47,
        TildeToken = 48,
        MinusToken = 49,
        BarToken = 50,
        ExclamationToken = 51,
        PercentToken = 52,
        CaretToken = 53,
        SlashToken = 54,
        QuestionToken = 55,
        DashGreaterThanToken = 56,
        DashEqualToken = 57,
        BarEqualToken = 58,
        AmpersandAmpersandToken = 59,
        BarBarToken = 60,
        LessThanEqualToken = 61,
        GreaterThanEqualToken = 62,
        AsteriksEqualToken = 63,
        PercentEqualToken = 64,
        PlusEqualToken = 65,
        MinusEqualToken = 66,
        AmpersandEqualToken = 67,
        ExclamationEqualToken = 68,
        CaretEqualToken = 69,
        LessThanLessThanEqualToken = 70,
        GreaterThanGreaterThanEqualToken = 71,

        // syntax
        TypeSpecifierSyntax = 72,
        AbstractDeclaratorSyntax = 73,
        UnaryExpression = 74,
        AdditiveExpression = 75,
        AndExpression = 76,
        TypeName = 77,
        DeclarationSpecifierSyntax = 78,
        CompoundStatement = 79,
        StructDeclarationSyntax = 80,
        Statement = 81,
        ShiftExpression = 82,
        SelectionStatement = 83,
        RelationalExpression = 84,
        Program = 85,
        PrimaryExpression = 86,
        PostfixExpression = 87,
        MultiplicativeExpression = 88,
        LogicalOrExpression = 89,
        LogicalAndExpression = 90,
        LabeledStatement = 91,
        JumpStatement = 92,
        IterationStatement = 93,
        InclusiveOrExpression = 94,
        IdentUnit = 95,
        FunctionDefinition = 96,
        ExpressionStatement = 97,
        Expression = 98,
        ExclusiveOrExpression = 99,
        EqualityExpression = 100,
        DirectAbstractDeclarator = 101,
        DeclaratorSyntax = 102,
        DeclarationSyntax = 103,
        ConstantExpression = 104,
        ConditionalExpression = 105,
        CastExpression = 106,
        AssignmentExpression = 107,
    }
}
namespace sc
{
    using global::sc.Diagnostics;
    using global::sc.Parse.Units;
    using System.Collections.Generic;
    using System.Reflection.Emit;

    public class Parser
    {
        private readonly Scanner scanner;
        private readonly Emit emit;
        private readonly Table unitTable;
        private SyntaxToken token;
        private readonly IDiagnostics diag;

        private readonly Stack<Label> breakStack = new Stack<Label>();
        private readonly Stack<Label> continueStack = new Stack<Label>();


        public Parser(Scanner scanner, Emit emit, Table symbolTable, IDiagnostics diag)
        {
            this.scanner = scanner;
            this.emit = emit;
            this.unitTable = symbolTable;
            this.diag = diag;
        }

        public bool Parse(out Program program)
        {
            ReadNextToken();
            return IsProgram(out program) && token.Kind == SyntaxKind.EndOfFileToken;
        }

        public void ReadNextToken()
        {
            token = scanner.Next();
        }

        public bool CheckToken(SyntaxKind kind)
        {
            if (token.Kind == kind)
            {
                ReadNextToken();
                return true;
            }

            return false;
        }

        void SkipUntilSemiColon()
        {
            //IToken Tok;
            //do
            //{
            //    Tok = scanner.Next();
            //} while (!((Tok is EOFToken) ||
            //             (Tok is SpecialSymbolToken) && ((Tok as SpecialSymbolToken).Value == ";")));
            //scanner.Next();
        }

        public void Error(string message)
        {
            diag.Error(token.Line, token.Column, message);
            SkipUntilSemiColon();
        }

        // [1] Program = {Declaration | FunctionDefinition}.
        private bool IsProgram(out Program program)
        {
            program = new Program();
            while (!(token.Kind == SyntaxKind.EndOfFileToken) && (ParseDeclaration(out var declaration) || ParseFunctionDefinition(out var functionDefinition)))
            {
                // globalScope.Add(functionDefinition);

                program.AddChild(declaration);
                // program.FunctionDefinition = functionDefinition;
            }
            
            return diag.GetErrorCount() == 0;
        }

        // [2] Declaration = DeclarationSpecifier [Declarator] ';'.
        private bool ParseDeclaration(out DeclarationSyntax declaration)
        {
            declaration = new DeclarationSyntax();

            if (!ParseDeclarationSpecifier(out var declarationSpecifier)) return false;
            declaration.DeclarationSpecifier = declarationSpecifier;

            if (IsDeclarator(out var declarator))
            {
                declaration.Declarator = declarator;
            }

            if (!CheckToken(SyntaxKind.SemiColonToken)) return false;

            declaration.AddChild(declarationSpecifier);
            declaration.AddChild(declarator);

            return true;
        }

        // [3] DeclarationSpecifier = ['typedef' | 'static'] TypeSpecifier.
        private bool ParseDeclarationSpecifier(out DeclarationSpecifierSyntax declarationSpecifier)
        {
            declarationSpecifier = new DeclarationSpecifierSyntax();

            var keyword = token;
            if (CheckToken(SyntaxKind.TypedefKeyword) || CheckToken(SyntaxKind.StaticKeyword))
                declarationSpecifier.Keyword = keyword;

            if (!IsTypeSpecifier(out var typeSpecifier)) return false;
            
            declarationSpecifier.TypeSpecifier = typeSpecifier;
            declarationSpecifier.AddChild(typeSpecifier);

            return true;
        }

        // [4] TypeSpecifier = 'void' | 'char' | 'bool' | 'short' | 'int' | 'long' | 'double' | 'signed' | 'unsigned' |
        // StructOrUnionSpecifier | EnumSpecifier
        private bool IsTypeSpecifier(out TypeSpecifierSyntax typeSpecifier)
        {
            typeSpecifier = new TypeSpecifierSyntax();

            if (IsStructOrUnionSpecifier(out var structOrUnionSpecifier))
            {
                //typeSpecifier.Keyword =
                 //   new KeywordToken(structOrUnionSpecifier.Ident.Line, structOrUnionSpecifier.Ident.Column, structOrUnionSpecifier.Ident.Value);
                typeSpecifier.AddChild(structOrUnionSpecifier);
                // userDefinedType guaranteed not to be null
                return true;
            }
            else if (IsEnumSpecifier(out var enumSpecifier))
            {
                typeSpecifier.Keyword = enumSpecifier.Keyword;

                return true;
            }
            else if (token.Kind == SyntaxKind.KeywordToken)
            {
                typeSpecifier.Keyword = token;

                // predefined type
                // scope.Add(new PredefinedTypeUnit(keywordToken));
                ReadNextToken();
                return true;
            }

            return false;
        }

        // [5] StructOrUnionSpecifier = ('struct' | 'union') [Ident] '{' {StructDeclaration} '}'.
        private bool IsStructOrUnionSpecifier(
            out StructOrUnionSpecifierSyntax structOrUnionSpecifier)
        {
            structOrUnionSpecifier = new StructOrUnionSpecifierSyntax();
            var keyword = token;
            if (CheckToken(SyntaxKind.StructKeyword) || CheckToken(SyntaxKind.UnionKeyword))
            {
                structOrUnionSpecifier.Keyword = keyword;

                var ident = token;

                if (CheckToken(SyntaxKind.IdentifierToken))
                    structOrUnionSpecifier.Ident = ident;
                else
                    structOrUnionSpecifier.Ident =
                        SyntaxTokenFactory.CreateWithValue(token.Line, token.Column, SyntaxKind.IdentifierToken, $"{token.Line}_{token.Column}_AutoGenerated");

                if (!CheckToken(SyntaxKind.OpenBracketToken)) return false;

                while (IsStructDeclaration(out var structDeclarationUnit))
                {
                    structOrUnionSpecifier.StructDeclaration.Add(structDeclarationUnit);
                    structOrUnionSpecifier.AddChild(structDeclarationUnit);
                }

                if (!CheckToken(SyntaxKind.ClosingBracketToken)) return false;

                return true;
            }

            return false;
        }

        // [6] StructDeclaration = TypeSpecifier Declarator {',' Declarator} ';'.
        private bool IsStructDeclaration(
            out StructDeclarationSyntax structDeclaration)
        {
            structDeclaration = new StructDeclarationSyntax();
            var structDeclarationScope = unitTable.BeginScope();
            
            if (!IsTypeSpecifier(out var typeSpecifier)) return false;
            structDeclaration.TypeSpecifier = typeSpecifier;
            structDeclaration.AddChild(typeSpecifier);

            if (!IsDeclarator(out var declarator)) return false;
            structDeclaration.Declarators.Add(declarator);
            structDeclaration.AddChild(declarator);

            while (CheckToken(SyntaxKind.CommaToken))
            {
                if (!IsDeclarator(out var declarator1)) return false;

                structDeclaration.Declarators.Add(declarator1);
                structDeclaration.AddChild(declarator1);
                //emit.AddField(declarationInfo1.Id.Value, type, arraySize);
            }
            if (!CheckToken(SyntaxKind.SemiColonToken)) return false;

            // emit.AddField(declarationInfo.Id.Value, type, arraySize);


            structDeclarationScope.Add(structDeclaration);

            unitTable.EndScope();
            return true;
        }

        // [7] EnumSpecifier = 'enum' [Ident] '{' Enumerator {',' Enumerator} '}'
        private bool IsEnumSpecifier(out EnumSpecifierSyntax enumSpecifier)
        {
            enumSpecifier = new EnumSpecifierSyntax();
            if (!CheckToken(SyntaxKind.EnumKeyword)) return false;

            var ident = token;
            if (CheckToken(SyntaxKind.IdentifierToken))
            {
                enumSpecifier.Ident = ident;
            }

            if (!CheckToken(SyntaxKind.OpenBracketToken)) return false;
            unitTable.BeginScope();
            if (!IsEnumerator(out var enumerator)) return false;
            enumSpecifier.Enumerators.Add(enumerator);

            while (CheckToken(SyntaxKind.CommaToken))
            {
                if (!IsEnumerator(out var enumerator1)) return false;
                enumSpecifier.Enumerators.Add(enumerator1);
            }
            unitTable.EndScope();
            if (!CheckToken(SyntaxKind.ClosingBracketToken)) return false;

            return true;
        }

        // [8] Enumerator = Ident ['=' ConstantExpression].
        private bool IsEnumerator(out EnumeratorSyntax enumerator)
        {
            enumerator = new EnumeratorSyntax();

            var ident = token;
            if (!CheckToken(SyntaxKind.IdentifierToken)) return false;
            enumerator.Ident = ident;
            if (CheckToken(SyntaxKind.EqualsToken))
            {
                if (!IsConstantExpression(out var constantExpression)) return false;

                return true;
            }

            return true;
        }

        // [9] Declarator = {'*'} DirectDeclarator.
        private bool IsDeclarator(out DeclaratorSyntax declarator)
        {
            declarator = new DeclaratorSyntax();
            var specialSymbol = token;
            while (CheckToken(SyntaxKind.AsteriskToken))
            {
                declarator.Asteriks.Add(specialSymbol);
            }
            if (!IsDirectDeclarator(out var directDeclarator)) return false;
            declarator.DirectDeclarator = directDeclarator;

            return true;
        }

        // [10] DirectDeclarator = Ident | '(' Declarator ')' | DirectDeclarator ( '[' [ConstantExpression] ']' | '(' [ParameterTypeList] | [Ident { ',' Ident }] ')' ).
        private bool IsDirectDeclarator(
            out DirectDeclaratorSyntax directDeclarator)
        {
            var ident = token;
            directDeclarator = new DirectDeclaratorSyntax();
            // field
            if (CheckToken(SyntaxKind.IdentifierToken))
            {
                directDeclarator.Ident = ident;

                return true;
            }
            // TODO
            // odd way of declaring field?
            else if (CheckToken(SyntaxKind.OpenBraceToken))
            {
                if (!IsDeclarator(out var declarator)) Error("Expected declarator.");
                if (!CheckToken(SyntaxKind.ClosingBraceToken)) Error("Expected special symbol ')'.");

                return true;
            }
            else
            {
                // array field
                if (CheckToken(SyntaxKind.OpenSquareBracketToken))
                {
                    if (IsConstantExpression(out var constantExpression)) ;
                    if (!CheckToken(SyntaxKind.ClosingSquareBracketToken)) Error("Expected special symbol ']'.");

                    return true;
                }
                // method
                else if (CheckToken(SyntaxKind.OpenBraceToken))
                {
                    if (IsParameterTypeList()) ;
                    else if (CheckToken(SyntaxKind.IdentifierToken))
                    {
                        while (CheckToken(SyntaxKind.CommaToken))
                        {
                            if (!CheckToken(SyntaxKind.IdentifierToken)) Error("Expected identifier.");
                        }
                    }

                    if (!CheckToken(SyntaxKind.ClosingBraceToken)) Error("Expected special symbol ')'.");
                }
            }

            return false;
        }

        // [11] ParameterTypeList = ParameterDeclaration {',' ParameterDeclaration}.
        private bool IsParameterTypeList()
        {
            if (!IsParameterDeclaration()) return false;
            while (CheckToken(SyntaxKind.CommaToken))
            {
                if (!IsParameterDeclaration()) return false;
            }

            return true;
        }

        // [12] ParameterDeclaration = DeclarationSpecifier [Declarator | AbstractDeclarator].
        private bool IsParameterDeclaration()
        {
            if (!ParseDeclarationSpecifier(out var declarationSpecifier)) return false;
            if (IsDeclarator(out var declarator) || IsAbstractDeclarator(out var abstractDeclarator)) ;

            return true;
        }

        // [13] TypeName = TypeSpecifier [AbstractDeclarator].
        private bool IsTypeName(out TypeName typeName)
        {
            typeName = new TypeName();
            if (!IsTypeSpecifier(out var type)) return false;
            while (!IsAbstractDeclarator(out var abstractDeclarator)) ;

            return true;
        }

        // [14] AbstractDeclarator = {'*'} DirectAbstractDeclarator.
        private bool IsAbstractDeclarator(out AbstractDeclarator abstractDeclarator)
        {
            abstractDeclarator = new AbstractDeclarator();
            while (CheckToken(SyntaxKind.AsteriskToken)) ;
            if (!IsDirectAbstractDeclarator(out var directAbstractDeclarator)) return false;

            return true;
        }

        // [15] DirectAbstractDeclarator = '(' AbstractDeclarator ')' |
        // [DirectAbstractDeclarator] ( '[' [ConstantExpression] ']' | '(' [ParameterTypeList] ')' ).
        private bool IsDirectAbstractDeclarator(out DirectAbstractDeclarator directAbstractDeclarator)
        {
            directAbstractDeclarator = new DirectAbstractDeclarator();
            if (CheckToken(SyntaxKind.OpenBraceToken))
            {
                if (!IsAbstractDeclarator(out var abstractDeclarator)) return false;
                if (!CheckToken(SyntaxKind.ClosingBraceToken)) return false;
            }
            else if (IsDirectAbstractDeclarator(out var moreDirectAbstractDeclarator)) ;
            else if (CheckToken(SyntaxKind.OpenSquareBracketToken))
            {
                if (IsConstantExpression(out var constantExpression)) ;
                if (!CheckToken(SyntaxKind.ClosingSquareBracketToken)) return false;
            }
            else if (CheckToken(SyntaxKind.OpenBraceToken))
            {
                if (IsParameterTypeList()) ;
                if (!CheckToken(SyntaxKind.ClosingBraceToken)) return false;
            }

            return true;
        }

        // [16] FunctionDefinition = [DeclarationSpecifier] Declarator {Declaration} CompoundStatement.
        private bool ParseFunctionDefinition(out FunctionDefinition functionDefinition)
        {
            functionDefinition = new FunctionDefinition();

            if (ParseDeclarationSpecifier(out var declarationSpecifier))
            {
                functionDefinition.DeclarationSpecifier = declarationSpecifier;
            }

            if (!IsDeclarator(out var declarator)) return false;
            functionDefinition.Declarator = declarator;

            while (ParseDeclaration(out var declaration))
            {
                functionDefinition.Declarations.Add(declaration);
            }

            if (!IsCompoundStatement(out var compoundStatement)) return false;
            functionDefinition.CompoundStatement = compoundStatement;

            return true;
        }

        // [17] Expression = AssignmentExpression {',' AssignmentExpression}.
        private bool IsExpression(out Expression expression)
        {
            expression = new Expression();
            if (!IsAssignmentExpression(out var assignmentExpression)) return false;
            while (CheckToken(SyntaxKind.CommaToken))
            {
                if (!IsAssignmentExpression(out var moreAssignmentExpression)) return false;
            }

            return true;
        }

        // [18] ConstantExpression = ConditionalExpression .
        private bool IsConstantExpression(out ConstantExpression constantExpression)
        {
            constantExpression = new ConstantExpression();
            if (!IsConditionalExpression(out var conditionalExpression)) Error("Expected conditional expression.");

            return true;
        }

        // [19] AssignmentExpression = ConditionalExpression |
        // UnaryExpression ('=' | '*=' | '/=' | '%=' | '+=' | '-=' | '<<=' | '>>=' | '&=' | '^=' | '|=') AssignmentExpression.
        private bool IsAssignmentExpression(out AssignmentExpression assignmentExpression)
        {
            assignmentExpression = new AssignmentExpression();
            if (IsConditionalExpression(out var conditionalExpression))
            {
                return true;
            }
            else if (IsUnaryExpression(out var unaryExpression))
            {
                if (CheckToken(SyntaxKind.EqualsToken))
                {
                }
                else if (CheckToken(SyntaxKind.AsteriksEqualToken))
                {
                }
                else if (CheckToken(SyntaxKind.DashEqualToken))
                {
                }
                else if (CheckToken(SyntaxKind.PercentEqualToken))
                {
                }
                else if (CheckToken(SyntaxKind.PlusEqualToken))
                {
                }
                else if (CheckToken(SyntaxKind.MinusEqualToken))
                {
                }
                else if (CheckToken(SyntaxKind.LessThanLessThanEqualToken))
                {
                }
                else if (CheckToken(SyntaxKind.GreaterThanGreaterThanEqualToken))
                {
                }
                else if (CheckToken(SyntaxKind.AmpersandEqualToken))
                {
                }
                else if (CheckToken(SyntaxKind.CaretEqualToken))
                {
                }
                else if (CheckToken(SyntaxKind.BarEqualToken))
                {
                }

                if (!IsAssignmentExpression(out var moreAssignmentExpression)) return false;
            }

            return true;
        }

        // [20] ConditionalExpression = LogicalORExpression |
        // LogicalORExpression '?' Expression ':' ConditionalExpression.
        private bool IsConditionalExpression(out ConditionalExpression conditionalExpression)
        {
            conditionalExpression = new ConditionalExpression();
            if (!IsLogicalOrExpression(out var logicalOrExpression))
            {
                return false;
            }

            if (CheckToken(SyntaxKind.QuestionToken))
            {
                if (IsExpression(out var expression))
                {
                    if (CheckToken(SyntaxKind.ColonToken))
                    {
                        return IsConditionalExpression(out var conditionalExpression1);
                    }
                }
            }

            return false;
        }

        // [21] LogicalORExpression = LogicalANDExpression {'||' LogicalANDExpression}.
        private bool IsLogicalOrExpression(out LogicalOrExpression logicalOrExpression)
        {
            logicalOrExpression = new LogicalOrExpression();
            if (!IsLogicalAndExpression(out var logicalAndExpression)) return false;
            while (CheckToken(SyntaxKind.BarBarToken))
            {
                if (!IsLogicalAndExpression(out var moreLogicalAndExpression)) return false;
            }

            return true;
        }

        // [22] LogicalANDExpression = InclusiveORExpression {'&&' InclusiveORExpression}.
        private bool IsLogicalAndExpression(out LogicalAndExpression logicalAndExpression)
        {
            logicalAndExpression = new LogicalAndExpression();
            if (!IsInclusiveOrExpression(out var inclusiveOrExpression)) return false;
            while (CheckToken(SyntaxKind.AmpersandAmpersandToken))
            {
                if (!IsLogicalOrExpression(out var logicalOrExpression)) return false;
            }

            return true;
        }

        // [23] InclusiveORExpression = ExclusiveORExpression {'|' ExclusiveORExpression}.
        private bool IsInclusiveOrExpression(out InclusiveOrExpression inclusiveOrExpression)
        {
            inclusiveOrExpression = new InclusiveOrExpression();
            if (!IsExclusiveOrExpression(out var exclusiveOrExpression)) return false;
            while (CheckToken(SyntaxKind.BarToken))
            {
                if (!IsExclusiveOrExpression(out var moreExclusiveOrExpression)) return false;
            }

            return true;
        }

        // [24] ExclusiveORExpression = ANDExpression {'^' ANDExpression}.
        private bool IsExclusiveOrExpression(out ExclusiveOrExpression exclusiveOrExpression)
        {
            exclusiveOrExpression = new ExclusiveOrExpression();
            if (!IsAndExpresion(out var isAndExpression)) return false;
            while (CheckToken(SyntaxKind.CaretToken))
            {
                if (!IsAndExpresion(out var moreIsAndExpression)) return false;
            }

            return true;
        }

        // [25] ANDExpression = EqualityExpression {'&' EqualityExpression}.
        private bool IsAndExpresion(out AndExpression isAndExpression)
        {
            isAndExpression = new AndExpression();
            if (!IsEqualityExpression(out var equalityExpression)) return false;
            while (CheckToken(SyntaxKind.AmpersandToken))
            {
                if (!IsEqualityExpression(out var moreEqualityExpression)) return false;
            }

            return true;
        }

        // [26] EqualityExpression = RelationalExpression ('==' | '!=') RelationalExpression.
        private bool IsEqualityExpression(out EqualityExpression equalityExpression)
        {
            equalityExpression = new EqualityExpression();
            if (!IsRelationalExpression(out var relationalExpression)) return false;
            if (CheckToken(SyntaxKind.EqualsEqualsToken) || CheckToken(SyntaxKind.ExclamationEqualToken)) ;
            if (!IsRelationalExpression(out var otherRelationalExpression)) return false;

            return true;
        }

        // [27] RelationalExpression = ShiftExpression {('<' | '>' | '<=' | '>=') ShiftExpression}.
        private bool IsRelationalExpression(out RelationalExpression relationalExpression)
        {
            relationalExpression = new RelationalExpression();
            if (!IsShiftExpression(out var shiftExpression)) return false;
            while (CheckToken(SyntaxKind.LessThanToken) || CheckToken(SyntaxKind.GreaterThanToken) || CheckToken(SyntaxKind.LessThanEqualToken) || CheckToken(SyntaxKind.GreaterThanEqualToken))
            {
                if (!IsShiftExpression(out var moreShiftExpressions)) return false;
            }

            return true;
        }

        // [28] ShiftExpression = AdditiveExpression {('<<' | '>>') AdditiveExpression}.
        private bool IsShiftExpression(out ShiftExpression shiftExpression)
        {
            shiftExpression = new ShiftExpression();
            if (!IsAdditiveExpression(out var additiveExpression)) return false;
            while (CheckToken(SyntaxKind.LessThanLessThanToken) || CheckToken(SyntaxKind.GreaterThanGreaterThanToken))
            {
                if (!IsAdditiveExpression(out var moreAdditiveExpressions)) return false;
            }

            return true;
        }

        // [29] AdditiveExpression = MultiplicativeExpression {('+' | '-') MultiplicativeExpression}.
        private bool IsAdditiveExpression(out AdditiveExpression additiveExpression)
        {
            additiveExpression = new AdditiveExpression();
            if (!IsMultiplicativeExpression(out var multiplicativeExpression)) return false;
            while (CheckToken(SyntaxKind.PlusToken) || CheckToken(SyntaxKind.MinusToken))
            {
                if (!IsMultiplicativeExpression(out var moreMultiplicativeExpression)) return false;
            }

            return true;
        }

        // [30] MultiplicativeExpression = CastExpression {('*' | '/' | '%') CastExpression}.
        private bool IsMultiplicativeExpression(out MultiplicativeExpression multiplicativeExpression)
        {
            multiplicativeExpression = new MultiplicativeExpression();
            if (!IsCastExpression(out var castExpression)) return false;
            while (CheckToken(SyntaxKind.AsteriskToken) || CheckToken(SyntaxKind.SlashToken) || CheckToken(SyntaxKind.PercentToken))
            {
                if (!IsCastExpression(out var moreCastExpression)) return false;
            }

            return true;
        }

        // [31] CastExpression = {'(' TypeName ')'} UnaryExpression.
        private bool IsCastExpression(out CastExpression castExpression)
        {
            castExpression = new CastExpression();
            while (CheckToken(SyntaxKind.OpenBraceToken))
            {
                if (!IsTypeName(out var typeName)) return false;
                if (!CheckToken(SyntaxKind.ClosingBraceToken)) return false;
            }

            if (!IsUnaryExpression(out var unaryExpression)) return false;

            return true;
        }

        // [32] UnaryExpression = PostfixExpression | ('++' | '--') UnaryExpression |
        // ('&' | '*' | '-' | '~' | '!') CastExpression.
        private bool IsUnaryExpression(out UnaryExpression unaryExpression)
        {
            unaryExpression = new UnaryExpression();
            if (IsPostfixExpression(out var postfixExpression))
            {
                return true;
            }
            else if (CheckToken(SyntaxKind.PlusPlusToken) || CheckToken(SyntaxKind.MinusMinusToken))
            {
                if (!IsUnaryExpression(out var moreUnaryExpression)) return false;

                return true;
            }
            else if (CheckToken(SyntaxKind.AmpersandToken) || CheckToken(SyntaxKind.AsteriskToken) || CheckToken(SyntaxKind.MinusToken) || CheckToken(SyntaxKind.TildeToken) || CheckToken(SyntaxKind.ExclamationToken))
            {
                if (!IsCastExpression(out var castExpression)) return false;

                return true;
            }

            return false;
        }

        // [33] PostfixExpression = PrimaryExpression | 
        // PostfixExpression '[' Expression ']' |
        // PostfixExpression '(' [AssignmentExpression {',' AssignmentExpression}] ')' |
        // PostfixExpression '.' Ident | 
        // PostfixExpression '->' Ident |
        // PostfixExpression ('++' | '--').

        // PS = '[' Expression ']' PS' |
        // PS' = 
        private bool IsPostfixExpression(out PostfixExpression postfixExpression)
        {
            postfixExpression = new PostfixExpression();
            if (IsPrimaryExpression(out var primaryExpression)) return true;
            else if (IsPostfixExpression(out var morePostfixExpression))
            {
                if (CheckToken(SyntaxKind.OpenSquareBracketToken))
                {
                    if (!IsExpression(out var expression)) return false;
                    if (!CheckToken(SyntaxKind.ClosingSquareBracketToken)) return false;

                    return true;
                }
                else if (CheckToken(SyntaxKind.OpenBraceToken))
                {
                    if (IsAssignmentExpression(out var assignmentExpression))
                    {
                        while (CheckToken(SyntaxKind.CommaToken))
                        {
                            if (!IsAssignmentExpression(out var moreAssignmentExpression)) return false;
                        }
                    }

                    if (!CheckToken(SyntaxKind.ClosingBraceToken)) return false;

                    return true;
                }
                else if (CheckToken(SyntaxKind.DotToken) || CheckToken(SyntaxKind.DashGreaterThanToken))
                {
                    if (!CheckToken(SyntaxKind.IdentifierToken)) return false;

                    return true;
                }
                else if (CheckToken(SyntaxKind.PlusPlusToken) || CheckToken(SyntaxKind.MinusMinusToken))
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        // [34] PrimaryExpression = Ident | Number | String | '(' Expression ')'.
        private bool IsPrimaryExpression(out PrimaryExpression primaryExpression)
        {
            primaryExpression = new PrimaryExpression();
            if (CheckToken(SyntaxKind.IdentifierToken))
            {
                return true;
            }
            else if (CheckToken(SyntaxKind.NumberToken))
            {
                return true;
            }
            else if (CheckToken(SyntaxKind.StringToken))
            {
                return true;
            }
            else if (CheckToken(SyntaxKind.OpenBraceToken))
            {
                if (!IsExpression(out var expression)) return false;
                if (!CheckToken(SyntaxKind.ClosingBraceToken)) return false;

                return true;
            }

            return false;
        }

        // [35] Statement = LabeledStatement | CompoundStatement | ExpressionStatement |
        // SelectionStatement | IterationStatement | JumpStatement.
        private bool IsStatement(out Statement statement)
        {
            statement = new Statement();
            if (!IsLabeledStatement(out var labeledStatement) || !IsCompoundStatement(out var compoundStatement) || !IsExpressionStatement(out var expressionStatement) || !IsSelectionStatement(out var selectionStatement) || !IsIterationStatement(out var iterationStatement) || !IsJumpStatement(out var jumpStatement))
            {
                return false;
            }

            return true;
        }

        // [36] LabeledStatement = 'case' ConstantExpression ':' Statement | 'default' ':' Statement.
        private bool IsLabeledStatement(out LabeledStatement labeledStatement)
        {
            labeledStatement = new LabeledStatement();
            if (CheckToken(SyntaxKind.CaseKeyword))
            {
                if (!IsConstantExpression(out var constantExpression)) return false;
                if (!CheckToken(SyntaxKind.ColonToken)) return false;
                if (!IsStatement(out var statement)) return false;

                return true;

            }
            else if (CheckToken(SyntaxKind.DefaultKeyword))
            {
                if (!CheckToken(SyntaxKind.ColonToken)) return false;
                if (!IsStatement(out var statement)) return false;

                return true;
            }
            return false;
        }

        // [37] CompoundStatement = '{' {[Declaration] [Statement]} '}'.
        private bool IsCompoundStatement(out CompoundStatement compoundStatement)
        {
            compoundStatement = new CompoundStatement();
            var openBrace = token;
            if (!CheckToken(SyntaxKind.OpenBracketToken)) return false;
            compoundStatement.OpenBrace = openBrace;
            while (ParseDeclaration(out var declaration) || IsStatement(out var statement))
            {
                compoundStatement.Declarations.Add(declaration);
            }
            var closingBrace = token;
            if (!CheckToken(SyntaxKind.ClosingBracketToken)) return false;
            compoundStatement.ClosingBrace = closingBrace;

            return true;
        }

        // [38] ExpressionStatement = [Expression] ';'.
        private bool IsExpressionStatement(out ExpressionStatement expressionStatement)
        {
            expressionStatement = new ExpressionStatement();
            if (IsExpression(out var expression)) ;
            if (!CheckToken(SyntaxKind.SemiColonToken)) return false;

            return true;
        }

        // [39] SelectionStatement = 'if' '(' Expression ')' Statement ['else' Statement] | 'switch' '(' Expression ')' Statement.
        private bool IsSelectionStatement(out SelectionStatement selectionStatement)
        {
            selectionStatement = new SelectionStatement();
            if (CheckToken(SyntaxKind.IfKeyword))
            {
                if (!CheckToken(SyntaxKind.OpenBraceToken)) return false;
                if (!IsExpression(out var expression)) return false;
                if (!CheckToken(SyntaxKind.ClosingBraceToken)) return false;
                if (!IsStatement(out var ifStatement)) return false;
                if (CheckToken(SyntaxKind.ElseKeyword))
                {
                    if (!IsStatement(out var elseStatement)) return false;
                }

                return true;
            }
            else if (CheckToken(SyntaxKind.SwitchKeyword))
            {
                if (!CheckToken(SyntaxKind.OpenBraceToken)) return false;
                if (!IsExpression(out var expression)) return false;
                if (!CheckToken(SyntaxKind.ClosingBraceToken)) return false;
                if (!IsStatement(out var switchStatement)) return false;

                return true;
            }

            return false;
        }

        // [40] IterationStatement = 'while' '(' Expression ')' Statement |
        // 'do' Statement 'while' '(' Expression ')' ';' | 'for' '(' ExpressionStatement ExpressionStatement [Expression] ')' Statement.
        private bool IsIterationStatement(out IterationStatement iterationStatement)
        {
            iterationStatement = new IterationStatement();
            if (CheckToken(SyntaxKind.WhileKeyword))
            {
                if (!CheckToken(SyntaxKind.OpenBraceToken)) return false;
                if (!IsExpression(out var expression)) return false;
                if (!CheckToken(SyntaxKind.ClosingBraceToken)) return false;
                if (!IsStatement(out var statement)) return false;

                return true;
            }
            else if (CheckToken(SyntaxKind.DoKeyword))
            {
                if (!IsStatement(out var statement)) return false;
                if (!CheckToken(SyntaxKind.WhileKeyword)) return false;
                if (!CheckToken(SyntaxKind.OpenBraceToken)) return false;
                if (!IsExpression(out var expression)) return false;
                if (!CheckToken(SyntaxKind.ClosingBraceToken)) return false;
                if (!CheckToken(SyntaxKind.SemiColonToken)) return false;

                return true;
            }
            else if (CheckToken(SyntaxKind.ForKeyword))
            {
                if (!CheckToken(SyntaxKind.OpenBraceToken)) return false;
                if (!IsExpressionStatement(out var expressionStatement)) return false;
                if (!IsExpressionStatement(out var expressionStatement1)) return false;
                if (IsExpression(out var expression)) ;
                if (!CheckToken(SyntaxKind.ClosingBraceToken)) return false;
                if (!IsStatement(out var statement)) return false;

                return true;
            }

            return false;
        }

        // [41] JumpStatement = 'continue' ';' | 'break' ';' | 'return' [Expression] ';'.
        private bool IsJumpStatement(out JumpStatement jumpStatement)
        {
            jumpStatement = new JumpStatement();
            if (CheckToken(SyntaxKind.ContinueKeyword))
            {
                if (!CheckToken(SyntaxKind.SemiColonToken)) return false;

                return true;
            }
            else if (CheckToken(SyntaxKind.BreakKeyword))
            {
                if (!CheckToken(SyntaxKind.SemiColonToken)) return false;

                return true;
            }
            else if (CheckToken(SyntaxKind.ReturnKeyword))
            {
                if (IsExpression(out var expression)) ;
                if (!CheckToken(SyntaxKind.SemiColonToken)) return false;

                return true;
            }

            return false;
        }

        public enum IncDecOps { None, PreInc, PreDec, PostInc, PostDec }
    }
}
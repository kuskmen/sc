namespace sc.Parse.Units
{
    public class DeclarationSpecifierSyntax : SyntaxNode
    {
        public DeclarationSpecifierSyntax()
            : this(null, null)
        {
        }

        public DeclarationSpecifierSyntax(
            SyntaxToken keyword,
            TypeSpecifierSyntax typeSpecifier)
            : base(SyntaxKind.DeclarationSpecifierSyntax)
        {
            Keyword = keyword;
            TypeSpecifier = typeSpecifier;
        }

        public SyntaxToken Keyword { get; set; }
        public TypeSpecifierSyntax TypeSpecifier { get; set; }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

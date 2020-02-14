namespace sc.Parse.Units
{
    public class TypeSpecifierSyntax : SyntaxNode
    {
        public TypeSpecifierSyntax()
            : this(null)
        {
        }

        public TypeSpecifierSyntax(SyntaxToken keyword)
            : base(SyntaxKind.TypeSpecifierSyntax)
        {
            Keyword = keyword;
        }

        public SyntaxToken Keyword { get; set; }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

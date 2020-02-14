namespace sc.Parse.Units
{
    public abstract class IdentUnit : SyntaxNode
    {
        protected IdentUnit(SyntaxToken ident)
            : base(SyntaxKind.IdentUnit)
        {
            Ident = ident;
        }

        public SyntaxToken Ident { get; set; }
    }
}
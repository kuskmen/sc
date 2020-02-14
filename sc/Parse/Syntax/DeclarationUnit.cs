namespace sc.Parse.Units
{
    internal class DeclarationUnit : IdentUnit
    {
        public DeclarationUnit(
            SyntaxToken ident,
            TypeSpecifierSyntax typeSpecifier)
            : base(ident)
        {
            TypeSpecifier = typeSpecifier;
        }

        public TypeSpecifierSyntax TypeSpecifier { get; }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

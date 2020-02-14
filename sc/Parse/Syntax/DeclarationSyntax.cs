namespace sc.Parse.Units
{
    public class DeclarationSyntax : SyntaxNode
    {
        public DeclarationSyntax()
            : this(new DeclarationSpecifierSyntax(), new DeclaratorSyntax())
        {
        }

        public DeclarationSyntax(
            DeclarationSpecifierSyntax declarationSpecifier, 
            DeclaratorSyntax declarator)
            : base(SyntaxKind.DeclarationSyntax)
        {
            DeclarationSpecifier = declarationSpecifier;
            Declarator = declarator;
        }

        public DeclarationSpecifierSyntax DeclarationSpecifier { get; set; }
        public DeclaratorSyntax Declarator { get; set; }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

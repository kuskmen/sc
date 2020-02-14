namespace sc.Parse.Units
{
    using System.Collections.Generic;

    public class DeclaratorSyntax : SyntaxNode
    {
        public DeclaratorSyntax()
            : this(new List<SyntaxToken>(),
                   new DirectDeclaratorSyntax())
        {
        }

        public DeclaratorSyntax(
            ICollection<SyntaxToken> asteriks, 
            DirectDeclaratorSyntax directDeclarator)
            : base(SyntaxKind.DeclaratorSyntax)
        {
            Asteriks = asteriks;
            DirectDeclarator = directDeclarator;
        }

        public ICollection<SyntaxToken> Asteriks { get; }
        public DirectDeclaratorSyntax DirectDeclarator { get; set; }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }

}
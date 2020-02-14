using System.Collections.Generic;

namespace sc.Parse.Units
{
    public class CompoundStatement : SyntaxNode
    {
        public CompoundStatement()
            : this(null, new List<DeclarationSyntax>(), null)
        { 
        }

        public CompoundStatement(
            SyntaxToken openBrace,
            ICollection<DeclarationSyntax> declarations,
            SyntaxToken closingBrace)
            : base(SyntaxKind.CompoundStatement)
        {
            OpenBrace = openBrace;
            Declarations = declarations;
            ClosingBrace = closingBrace;
        }

        public SyntaxToken OpenBrace { get; set; }
        public ICollection<DeclarationSyntax> Declarations { get; }
        public SyntaxToken ClosingBrace { get; set; }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

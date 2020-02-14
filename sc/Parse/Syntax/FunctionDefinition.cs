using System.Collections.Generic;

namespace sc.Parse.Units
{
    public class FunctionDefinition : SyntaxNode
    {
        public FunctionDefinition()
            : this(
                  new DeclarationSpecifierSyntax(), 
                  new DeclaratorSyntax(), 
                  new List<DeclarationSyntax>(), 
                  new CompoundStatement())
        { 
        }

        public FunctionDefinition(
            DeclarationSpecifierSyntax declarationSpecifier, 
            DeclaratorSyntax declarator,
            ICollection<DeclarationSyntax> declarations,
            CompoundStatement compoundStatement)
            : base(SyntaxKind.FunctionDefinition)
        {
            DeclarationSpecifier = declarationSpecifier;
            Declarator = declarator;
            Declarations = declarations;
            CompoundStatement = compoundStatement;
        }

        public DeclarationSpecifierSyntax DeclarationSpecifier { get; set; }
        public DeclaratorSyntax Declarator { get; set; }
        public ICollection<DeclarationSyntax> Declarations { get; }
        public CompoundStatement CompoundStatement { get; set; }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

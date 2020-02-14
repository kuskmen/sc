using System.Collections.Generic;

namespace sc.Parse.Units
{
    internal class StructDeclarationSyntax : SyntaxNode
    {
        public StructDeclarationSyntax()
            : this(new TypeSpecifierSyntax(), new List<DeclaratorSyntax>())
        {
        }

        public StructDeclarationSyntax(
            TypeSpecifierSyntax typeSpecifier,
            ICollection<DeclaratorSyntax> declarators)
            : base(SyntaxKind.StructDeclarationSyntax)
        {
            TypeSpecifier = typeSpecifier;
            Declarators = declarators;
        }

        public TypeSpecifierSyntax TypeSpecifier { get; set; }
        public ICollection<DeclaratorSyntax> Declarators { get; set; }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

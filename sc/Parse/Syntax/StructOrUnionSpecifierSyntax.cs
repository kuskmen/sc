using System.Collections.Generic;

namespace sc.Parse.Units
{
    internal class StructOrUnionSpecifierSyntax : TypeSpecifierSyntax
    {
        public StructOrUnionSpecifierSyntax()
            : this(null)
        { 
        }

        public StructOrUnionSpecifierSyntax(SyntaxToken keyword) 
            : this(keyword, null, new List<StructDeclarationSyntax>())
        {
        }

        public StructOrUnionSpecifierSyntax(
            SyntaxToken keyword,
            SyntaxToken ident,
            ICollection<StructDeclarationSyntax> structDeclarations) 
            : base(keyword)
        {
            Ident = ident;
            StructDeclaration = structDeclarations;
        }

        public SyntaxToken Ident { get; set; }

        public ICollection<StructDeclarationSyntax> StructDeclaration { get; set; }
    }
}

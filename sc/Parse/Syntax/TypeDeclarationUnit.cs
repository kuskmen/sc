namespace sc.Parse.Units
{
    using System.Collections.Generic;
    using System.Linq;

    internal class TypeDeclarationUnit : IdentUnit
    {
        public TypeDeclarationUnit()
            : this(null)
        {
        }

        public TypeDeclarationUnit(SyntaxToken identToken)
            : this(identToken, 
                  null,
                  Enumerable.Empty<FieldDeclarationUnit>(),
                  Enumerable.Empty<MethodDeclarationUnit>())
        {
        }

        public TypeDeclarationUnit(
            SyntaxToken ident,
            SyntaxToken keyword,
            IEnumerable<FieldDeclarationUnit> fdunits,
            IEnumerable<MethodDeclarationUnit> mdunits) 
            : base(ident)
        {
            Keyword = keyword;
            Fields = fdunits;
            Methods = mdunits;
        }

        public SyntaxToken Keyword { get; }
        public IEnumerable<FieldDeclarationUnit> Fields { get; }
        public IEnumerable<MethodDeclarationUnit> Methods { get; }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }

    internal class PredefinedTypeUnit : IdentUnit
    {
        public PredefinedTypeUnit(SyntaxToken keyword) : 
            base(null)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

namespace sc.Parse.Units
{
    using System.Collections.Generic;

    internal class FieldDeclarationUnit : DeclarationUnit
    {
        public FieldDeclarationUnit()
            : base(null, 
                  new TypeSpecifierSyntax())
        { 
        }

        public FieldDeclarationUnit(
            SyntaxToken ident,
            TypeSpecifierSyntax returnType,
            IEnumerable<VariableDeclarationUnit> vdunits)
            : base(ident, returnType)
        {
            VariableDeclarationUnits = vdunits;
        }

        public IEnumerable<VariableDeclarationUnit> VariableDeclarationUnits { get; set; }
    }
}

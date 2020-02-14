namespace sc.Parse.Units
{
    using System.Collections.Generic;
    using System.Linq;

    internal class MethodDeclarationUnit : DeclarationUnit
    {
        public MethodDeclarationUnit()
            : this(null, 
                  new TypeSpecifierSyntax(), 
                  Enumerable.Empty<ParameterUnit>())
        {
        }

        public MethodDeclarationUnit(
            SyntaxToken ident, 
            TypeSpecifierSyntax returnType, 
            IEnumerable<ParameterUnit> parameters)
            : base(ident, returnType)
        {
            Parameters = parameters;
        }

        public IEnumerable<ParameterUnit> Parameters { get; }
    }
}

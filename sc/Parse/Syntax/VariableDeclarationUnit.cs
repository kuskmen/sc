namespace sc.Parse.Units
{
    using System.Collections.Generic;

    internal class VariableDeclarationUnit
    {
        public VariableDeclarationUnit(
            IEnumerable<SyntaxToken> identifiers) 
        {
            Identifiers = identifiers;
        }

        public IEnumerable<SyntaxToken> Identifiers { get; }
    }
}

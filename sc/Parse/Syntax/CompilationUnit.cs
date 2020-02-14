namespace sc.Parse.Units
{
    using System.Collections.Generic;
    
    internal class CompilationUnit
    {
        public CompilationUnit(IEnumerable<SyntaxNode> units)
        {
            Units = units;
        }

        public IEnumerable<SyntaxNode> Units { get; }
    }
}

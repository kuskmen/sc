namespace sc.Parse.Units
{
    using System.Collections.Generic;

    public class Program : SyntaxNode
    {
        public Program()
            : this(new List<DeclarationSyntax>(), new List<FunctionDefinition>())
        { 
        }

        public Program(
            ICollection<DeclarationSyntax> declarations, 
            ICollection<FunctionDefinition> functionDefinitions)
            : base(SyntaxKind.Program)
        {
            Declarations = declarations;
            FunctionDefinitions = functionDefinitions;
        }

        public ICollection<DeclarationSyntax> Declarations { get; }
        public ICollection<FunctionDefinition> FunctionDefinitions { get; }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

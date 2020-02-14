namespace sc.Parse.Units
{
    internal class IterationStatement : SyntaxNode
    {
        public IterationStatement()
            : base(SyntaxKind.IterationStatement)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

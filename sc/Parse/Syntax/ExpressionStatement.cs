namespace sc.Parse.Units
{
    internal class ExpressionStatement : SyntaxNode
    {
        public ExpressionStatement()
            : base(SyntaxKind.ExpressionStatement)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

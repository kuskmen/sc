namespace sc.Parse.Units
{
    internal class UnaryExpression : SyntaxNode
    {
        public UnaryExpression()
            : base(SyntaxKind.UnaryExpression)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

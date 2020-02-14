namespace sc.Parse.Units
{
    internal class InclusiveOrExpression : SyntaxNode
    {
        public InclusiveOrExpression()
            : base(SyntaxKind.InclusiveOrExpression)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

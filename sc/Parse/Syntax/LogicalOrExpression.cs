namespace sc.Parse.Units
{
    internal class LogicalOrExpression : SyntaxNode
    {
        public LogicalOrExpression()
            : base(SyntaxKind.LogicalOrExpression)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

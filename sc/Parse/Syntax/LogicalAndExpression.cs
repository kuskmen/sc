namespace sc.Parse.Units
{
    internal class LogicalAndExpression : SyntaxNode
    {
        public LogicalAndExpression()
            : base(SyntaxKind.LogicalAndExpression)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

namespace sc.Parse.Units
{
    internal class ExclusiveOrExpression : SyntaxNode
    {
        public ExclusiveOrExpression()
            : base(SyntaxKind.ExclusiveOrExpression)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

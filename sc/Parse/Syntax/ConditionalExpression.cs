namespace sc.Parse.Units
{
    internal class ConditionalExpression : SyntaxNode
    {
        public ConditionalExpression()
            : base(SyntaxKind.ConditionalExpression)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

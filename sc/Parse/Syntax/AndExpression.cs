namespace sc.Parse.Units
{
    internal class AndExpression : SyntaxNode
    {
        public AndExpression()
            : base(SyntaxKind.AndExpression)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

namespace sc.Parse.Units
{
    internal class PrimaryExpression : SyntaxNode
    {
        public PrimaryExpression()
            : base(SyntaxKind.PrimaryExpression)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

namespace sc.Parse.Units
{
    internal class PostfixExpression : SyntaxNode
    {
        public PostfixExpression()
            : base(SyntaxKind.PostfixExpression)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

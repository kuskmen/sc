namespace sc.Parse.Units
{
    internal class MultiplicativeExpression : SyntaxNode
    {
        public MultiplicativeExpression()
            : base(SyntaxKind.MultiplicativeExpression)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

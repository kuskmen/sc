namespace sc.Parse.Units
{
    internal class CastExpression : SyntaxNode
    {
        public CastExpression()
            : base(SyntaxKind.CastExpression)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

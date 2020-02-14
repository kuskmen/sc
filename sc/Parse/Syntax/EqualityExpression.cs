namespace sc.Parse.Units
{
    internal class EqualityExpression : SyntaxNode
    {
        public EqualityExpression()
            : base(SyntaxKind.EqualityExpression)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

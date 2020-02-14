namespace sc.Parse.Units
{
    internal class ConstantExpression : SyntaxNode
    {
        public ConstantExpression()
            : base(SyntaxKind.ConstantExpression)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

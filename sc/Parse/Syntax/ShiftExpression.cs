namespace sc.Parse.Units
{
    internal class ShiftExpression : SyntaxNode
    {
        public ShiftExpression()
            : base(SyntaxKind.ShiftExpression)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

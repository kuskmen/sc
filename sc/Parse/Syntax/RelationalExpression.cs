namespace sc.Parse.Units
{
    internal class RelationalExpression : SyntaxNode
    {
        public RelationalExpression()
            : base(SyntaxKind.RelationalExpression)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

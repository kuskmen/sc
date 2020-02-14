namespace sc.Parse.Units
{
    internal class AdditiveExpression : SyntaxNode
    {
        public AdditiveExpression() 
            : base(SyntaxKind.AdditiveExpression)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

namespace sc.Parse.Units
{
    internal class Expression : SyntaxNode
    {
        public Expression()
            : base(SyntaxKind.Expression)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

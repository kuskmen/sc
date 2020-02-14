namespace sc.Parse.Units
{
    internal class AssignmentExpression : SyntaxNode
    {
        public AssignmentExpression()
            : base(SyntaxKind.AssignmentExpression)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

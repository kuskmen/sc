namespace sc.Parse.Units
{
    internal class JumpStatement : SyntaxNode
    {
        public JumpStatement()
            : base(SyntaxKind.JumpStatement)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

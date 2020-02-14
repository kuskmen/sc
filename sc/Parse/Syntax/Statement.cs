namespace sc.Parse.Units
{
    internal class Statement : SyntaxNode
    {
        public Statement()
            : base(SyntaxKind.Statement)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

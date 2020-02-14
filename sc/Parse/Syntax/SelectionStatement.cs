namespace sc.Parse.Units
{
    internal class SelectionStatement : SyntaxNode
    {
        public SelectionStatement()
            : base(SyntaxKind.SelectionStatement)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

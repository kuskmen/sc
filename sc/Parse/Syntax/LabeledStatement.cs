namespace sc.Parse.Units
{
    internal class LabeledStatement : SyntaxNode
    {
        public LabeledStatement()
            : base(SyntaxKind.LabeledStatement)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

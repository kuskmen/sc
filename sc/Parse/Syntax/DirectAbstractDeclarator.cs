namespace sc.Parse.Units
{
    internal class DirectAbstractDeclarator : SyntaxNode
    {
        public DirectAbstractDeclarator()
            : base(SyntaxKind.DirectAbstractDeclarator)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

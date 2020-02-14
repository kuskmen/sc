namespace sc.Parse.Units
{
    internal class AbstractDeclarator : SyntaxNode
    {
        public AbstractDeclarator()
            : base(SyntaxKind.AbstractDeclaratorSyntax)
        {

        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

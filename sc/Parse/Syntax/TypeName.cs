namespace sc.Parse.Units
{
    internal class TypeName : SyntaxNode
    {
        public TypeName()
            : base(SyntaxKind.TypeName)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}

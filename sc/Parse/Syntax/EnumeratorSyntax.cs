namespace sc.Parse.Units
{
    internal class EnumeratorSyntax : IdentUnit
    {
        public EnumeratorSyntax()
            : this(null)
        {
        }

        public EnumeratorSyntax(SyntaxToken ident)
            : base(ident)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}
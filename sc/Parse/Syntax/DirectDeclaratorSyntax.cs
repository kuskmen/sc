namespace sc.Parse.Units
{
    public class DirectDeclaratorSyntax : IdentUnit
    {
        public DirectDeclaratorSyntax()
            : this(null)
        { 
        }

        public DirectDeclaratorSyntax(SyntaxToken ident)
            : base(ident)
        {
        }

        internal override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }

}
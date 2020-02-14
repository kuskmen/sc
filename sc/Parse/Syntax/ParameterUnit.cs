namespace sc.Parse.Units
{
    using System;

    internal class ParameterUnit : IdentUnit
    {
        public ParameterUnit(SyntaxToken ident, Type type)
            : base(ident)
        {
            Type = type;
        }

        public Type Type { get; }

        internal override void Visit()
        {
            throw new NotImplementedException();
        }
    }
}

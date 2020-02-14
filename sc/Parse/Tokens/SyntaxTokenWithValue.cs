namespace sc
{
    public class SyntaxTokenWithValue<T> : SyntaxToken
    {
        public T ValueField;

        public SyntaxTokenWithValue(int line, int column, SyntaxKind kind, T value) 
            : base(line, column, kind)
        {
            ValueField = value;
        }

        public override object Value
        {
            get => this.ValueField;
        }
    }
}
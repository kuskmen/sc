namespace sc
{
    using System.Text;

    public class SyntaxToken
    {
        public SyntaxToken(int line, int column, SyntaxKind kind)
        {
            Line = line;
            Column = column;
            Kind = kind;
        }

        public int Line { get; }

        public int Column { get; }

        public SyntaxKind Kind { get; }

        public virtual string Text
        {
            get => "Not implemented";
        }

        public virtual object Value 
        {
            get
            {
                switch (this.Kind)
                {
                    case SyntaxKind.TrueKeywordToken:
                        return true;
                    case SyntaxKind.FalseKeywordToken:
                        return false;
                    default:
                        return this.Text;
                }
            }
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            s.AppendFormat("line {0}, column {1}: {2} - {3}", Line, Column, Kind, GetType());
            return s.ToString();
        }
        
    }
}
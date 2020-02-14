using System.Collections.Generic;

namespace sc.Parse.Units
{
    internal class EnumSpecifierSyntax : TypeSpecifierSyntax
    {
        public EnumSpecifierSyntax()
           : this(null)
        {
        }

        public EnumSpecifierSyntax(SyntaxToken keyword)
            : this(keyword, null, 
                  new List<EnumeratorSyntax>())
        {
        }

        public EnumSpecifierSyntax(
            SyntaxToken keyword,
            SyntaxToken ident, 
            ICollection<EnumeratorSyntax> enumerators)
            : base(keyword)
        {
            Ident = ident;
            Enumerators = enumerators;
        }

        public SyntaxToken Ident { get; set; }
        public ICollection<EnumeratorSyntax> Enumerators { get; set; }
    }
}

using sc.Parse.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace sc
{
    public static class Extensions
    {
        internal static bool ContainsIdentifier(this ICollection<SyntaxNode> scope, string identifier)
            => scope
                .Where(unit => unit is IdentUnit)
                .Cast<IdentUnit>()
                .Any(identUnit => identUnit.Ident.Value == identifier);
    }
}

namespace sc.Parse.Units
{
    using System.Collections.Generic;
    using System.Diagnostics;

    public abstract class SyntaxNode
    {
        public SyntaxNode Parent { get; set; }

        public IList<SyntaxNode> Children { get; } = new List<SyntaxNode>();

        public SyntaxNode(SyntaxKind kind)
        {
            Kind = kind;
        }

        public SyntaxKind Kind { get; }

        public void AddChild(SyntaxNode node)
        {
            Debug.Assert(node != null);

            node.Parent = this;
            Children.Add(node);
        }

        internal abstract void Visit();
    }
}

namespace sc
{
    using global::sc.Parse.Units;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public class Table
	{
		private readonly Stack<ICollection<SyntaxNode>> unitTable;
		private readonly ICollection<SyntaxNode> universeScope;
		private List<string> usingNamespaces = new List<string>();

		public Table(List<string> references)
		{
			this.unitTable = new Stack<ICollection<SyntaxNode>>();
			this.universeScope = BeginScope();
			foreach (string assemblyRef in references)
			{
				Assembly.LoadWithPartialName(assemblyRef);
				//Assembly.Load(assemblyRef);
			}
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			int i = unitTable.Count;
			sb.AppendFormat("=========\n");
			foreach (var scope in unitTable)
			{
				sb.AppendFormat("---[{0}]---\n", i--);
				foreach (var unit in scope)
				{
					sb.AppendFormat("{0}\n", unit.GetType());
				}
			}
			sb.AppendFormat("=========\n");
			return sb.ToString();
		}

		public void AddUsingNamespace(string usingNamespace)
		{
			usingNamespaces.Add(usingNamespace);
		}

		internal SyntaxNode Add(SyntaxNode unit)
		{
			unitTable.Peek().Add(unit);
			return unit;
		}

		internal SyntaxNode AddToUniverse(SyntaxNode unit)
		{
			universeScope.Add(unit);
			return unit;
		}

		internal ICollection<SyntaxNode> BeginScope()
		{
			unitTable.Push(new List<SyntaxNode>());
			return unitTable.Peek();
		}

		internal void EndScope()
		{
			Debug.WriteLine(ToString());

			unitTable.Pop();
		}

		internal SyntaxNode GetUnit(string ident)
		{
			foreach (var scope in unitTable)
			{
				return scope
						.Where(unit => unit is IdentUnit)
						.Cast<IdentUnit>()
						.FirstOrDefault(identUnit => identUnit.Ident.Value == ident);
			}

			return null;
			//return ResolveExternalMember(ident);
		}

		internal ICollection<TypeDeclarationUnit> GetTypes()
		{
			var types = new List<TypeDeclarationUnit>();
			foreach (var table in unitTable)
			{
				foreach(var symbol in table)
				{
					if (symbol is TypeDeclarationUnit ts)
						types.Add(ts);
				}
			}

			return types;
		}

		public Type ResolveExternalType(string ident)
		{
			// Type
			// Namespace.Type

			Type type = Type.GetType(ident, false, false);
			if (type != null) return type;
			foreach (string ns in usingNamespaces)
			{
				string nsTypeName = ns + Type.Delimiter + ident;
				foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					type = assembly.GetType(ident);
					if (type != null) return type;
					type = assembly.GetType(nsTypeName);
					if (type != null) return type;
				}
			}
			return null;
		}
	}
}
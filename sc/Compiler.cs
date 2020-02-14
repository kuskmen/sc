namespace sc
{
    using global::sc.Diagnostics;
    using System.Collections.Generic;
    using System.IO;

    public static class Compiler
    {
        private static readonly List<string> References = new List<string>();

        public static bool Compile(string file, string assemblyName) 
            => Compile(file, assemblyName, new DefaultDiagnostics());

        public static bool Compile(string file, string assemblyName, IDiagnostics diag)
        {
            var reader = new StreamReader(file);
            var scanner = new Scanner(reader);
            var symbolTable = new Table(References);
            var emit = new Emit(assemblyName, symbolTable);
            var parser = new Parser(scanner, emit, symbolTable, diag);

            diag.BeginSourceFile(file);
            bool isProgram = parser.Parse(out var syntaxTree);

            diag.Traverse(syntaxTree);

            diag.EndSourceFile();

            if (isProgram)
            {
                emit.WriteExecutable();
            }

            return isProgram;
        }

        public static void AddReferences(IEnumerable<string> references) => References.AddRange(references);
    }
}
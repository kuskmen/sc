namespace sc
{
    using sc.Diagnostics;
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
            var lexer = new Lexer(reader);
            var emit = new Emit(assemblyName);
            var parser = new Parser(lexer, diag);

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
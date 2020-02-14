namespace sc.Diagnostics
{
    using global::sc.Parse.Units;
	using System;

	public class DefaultDiagnostics : IDiagnostics
	{
		public int ErrorCount { get; private set; } = 0;
		public int WarningCount { get; private set; } = 0;
		public int NoteCount { get; private set; } = 0;

		public void Error(int line, int column, string message)
		{
			Console.WriteLine($"Error on line {line}, column {column}: {message}");
			ErrorCount++;
		}

		public void Warning(int line, int column, string message)
		{
			Console.WriteLine($"Warning on line {line}, column {column}: {message}");
			WarningCount++;
		}

		public void Note(int line, int column, string message)
		{
			Console.WriteLine("Забележка на линия {0}, колона {1}: {2}", line, column, message);
			NoteCount++;
		}

		public void BeginSourceFile(string sourceFile)
		{
			// no op
		}

		public void EndSourceFile()
		{
			// no op
		}

		public int GetErrorCount()
		{
			return ErrorCount;
		}

		public void Traverse(Program syntaxTree)
		{
			throw new NotImplementedException();
		}
	}

	public interface IDiagnostics
	{
		int GetErrorCount();

		void Error(int line, int column, string message);
		void Warning(int line, int column, string message);
		void Note(int line, int column, string message);

		void BeginSourceFile(string sourceFile);

		void EndSourceFile();

        void Traverse(Program syntaxTree);
    }
}
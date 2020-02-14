namespace sc.Tests
{
    using sc.Diagnostics;
    using sc.Parse.Units;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;

    public class VerifyDiagnostics : IDiagnostics
    {
        class DiagnosticItem
        {
            public int Line { get; }
            public string Message { get; }

            public DiagnosticItem(int line, string message)
            {
                this.Line = line;
                this.Message = message;
            }

            public override string ToString()
            {
                return Message + "\t on line: " + Line;
            }

        }

        private List<DiagnosticItem> SeenErrors = new List<DiagnosticItem>();
        private readonly List<DiagnosticItem> ExpectedErrors = new List<DiagnosticItem>();
        private string currentSourceFile = null;

        public VerifyDiagnostics()
        {
        }

        public int GetErrorCount() => CompareErrorLists();

        public void Error(int line, int column, string message)
        {
            SeenErrors.Add(new DiagnosticItem(line, message));
        }

        public void Note(int line, int column, string message)
        {
            throw new NotImplementedException();
        }

        public void Warning(int line, int column, string message)
        {
            throw new NotImplementedException();
        }

        public void BeginSourceFile(string sourceFile)
        {
            currentSourceFile = sourceFile;
        }

        public void EndSourceFile()
        {
            currentSourceFile = null;
        }

        private void GetExpectedErrors()
        {
            StreamReader reader = new StreamReader(currentSourceFile);

            Scanner scanner = new Scanner(reader);
            scanner.SkipComments = false;
            Scanner CommentScanner;
            var t = scanner.Next();

            while (!(t.Kind == SyntaxKind.EndOfFileToken))
            {
                if (t.Kind == SyntaxKind.CommentToken)
                {
                    CommentScanner = new Scanner(new StringReader((string)t.Value));
                    var ErrorMessage = CommentScanner.Next();
                    do
                    {
                        if ((ErrorMessage.Kind == SyntaxKind.IdentifierToken) && ErrorMessage.Value == "expectederror")
                            ErrorMessage = CommentScanner.Next();

                        if (ErrorMessage.Kind == SyntaxKind.StringToken)
                            ExpectedErrors.Add(new DiagnosticItem(t.Line, (string)ErrorMessage.Value));

                        ErrorMessage = CommentScanner.Next();
                    } while (!(ErrorMessage.Kind == SyntaxKind.EndOfFileToken));
                }
                t = scanner.Next();
            }
        }

        private void DumpExpectedErrors()
        {
            Debug.WriteLine("Errors expected but not seen: " + ExpectedErrors.Count);

            foreach (DiagnosticItem error in ExpectedErrors)
            {
                Debug.WriteLine(error.ToString());
            }
            Debug.WriteLine("\n");

        }

        private void DumpSeenErrors()
        {
            Debug.WriteLine("Errors seen but not expected: " + SeenErrors.Count);

            foreach (DiagnosticItem error in SeenErrors)
            {
                Debug.WriteLine(error.ToString());
            }
            Debug.WriteLine("\n");
        }

        private int CompareErrorLists()
        {
            GetExpectedErrors();
            for (int i = SeenErrors.Count - 1; i >= 0; --i)
            {
                DiagnosticItem seenError = SeenErrors[i];
                // Check whether we had more than one error on line. In that case
                // we can neglect the exact ordering.
                for (int j = ExpectedErrors.Count - 1; j >= 0; --j)
                {
                    DiagnosticItem expectedError = ExpectedErrors[j];
                    // If the messages and lines correspond to each other we 
                    // pop the pair (seen-expected) from the lists.
                    if (seenError.Line == expectedError.Line)
                        if (seenError.Message == expectedError.Message)
                        {
                            SeenErrors.Remove(seenError);
                            ExpectedErrors.Remove(expectedError);
                            // Make sure that if we have repeating expected errors 
                            // we are removing only one of them.
                            break;
                        }
                }
            }
            // Dump the diffs
            if (SeenErrors.Count > 0)
            {
                DumpSeenErrors();
            }

            if (ExpectedErrors.Count > 0)
            {
                DumpExpectedErrors();
            }
            return SeenErrors.Count + ExpectedErrors.Count;
        }

        public void Traverse(Program root)
        {
            for(var childIndex = 0; childIndex < root.Children.Count; ++childIndex)
            {
                
                var rootOfChildSubtree = root.Children[childIndex];
                while(rootOfChildSubtree.Children.Count > 0)
                    rootOfChildSubtree = rootOfChildSubtree.Children[0];

                var processingNode = rootOfChildSubtree;
                while(processingNode.Parent !=  root)
                {
                    foreach(var node in processingNode.Parent.Children)
                        node.Visit();
                    
                    processingNode = processingNode.Parent;
                }
            }
        }
    }
}

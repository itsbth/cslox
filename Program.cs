namespace CSLox
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    internal class Program
    {
        private static bool hadError;

        internal static void Error(int line, string message)
        {
            Report(line, string.Empty, message);
        }

        internal static void Error(Token token, string message)
        {
            if (token.Type == TokenType.EOF)
            {
                Report(token.Line, " at end", message);
            }
            else
            {
                Report(token.Line, " at '" + token.Lexeme + "'", message);
            }
        }

        private static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("usage: <app> [file]");
                return;
            }

            if (args.Length == 1)
            {
                RunFile(args[0]);
                return;
            }

            RunRepl();
        }

        private static void RunRepl()
        {
            while (true)
            {
                hadError = false;
                Console.Write("> ");
                var code = Console.ReadLine();
                if (code == null || code.Trim() == "q")
                {
                    break;
                }

                Run(code);
            }
        }

        private static void RunFile(string fileName)
        {
            var code = File.ReadAllText(fileName, encoding: Encoding.UTF8);
            Run(code);
            if (hadError)
            {
                Environment.Exit(255);
            }
        }

        private static void Run(string code)
        {
            var scanner = new Scanner(code);
            List<Token> tokens = scanner.ScanTokens();
            foreach (var token in tokens)
            {
                Console.WriteLine(token);
            }

            var parser = new Parser(tokens);
            var tree = parser.Parse();
            if (tree != null)
            {
                var printer = new AstPrinter();
                Console.WriteLine(printer.Print(tree));
                var interpreter = new Interpreter();
            }
        }

        private static void Report(int line, string where, string message)
        {
            Console.Error.WriteLine($"[line {line}] Error{where}: {message}");
            hadError = true;
        }
    }
}

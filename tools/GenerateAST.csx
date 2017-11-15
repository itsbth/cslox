#! "netcoreapp2.0"
// vim: ft=cs
#r "nuget:NetStandard.Library,2.0.0"

using System;
using System.IO;
using System.Collections;

// string LCFirst(this string s) {
//     return s.Substring(0, 1).Lower() + s.Substring(1);
// }


void defineVisitor(
    StreamWriter writer, string baseName, List<string> types)
{
    writer.WriteLine("    public interface IVisitor<R> {");

    foreach (string type in types) {
        string typeName = type.Split(":")[0].Trim();
        writer.WriteLine("      R Visit" + typeName + baseName + "(" +
            typeName + " " + baseName.ToLower() + ");");
    }

    writer.WriteLine("    }");
}

void defineType(
    StreamWriter writer, string baseName,
    string className, string fieldList)
{
    writer.WriteLine("");
    writer.WriteLine("    public sealed class " + className + " : " +
        baseName + " {");

    // Constructor.
    writer.WriteLine("      public " + className + "(" + fieldList + ") {");

    // Store parameters in fields.
    String[] fields = fieldList.Split(", ");
    foreach (string field in fields) {
      string name = field.Split(" ")[1];
      writer.WriteLine("        this." + name + " = " + name + ";");
    }

    writer.WriteLine("      }");

    writer.WriteLine("");
    writer.WriteLine("      public override R Accept<R>(IVisitor<R> visitor) {");
    writer.WriteLine("        return visitor.Visit" + className + baseName + "(this);");
    writer.WriteLine("      }");

    // Fields.
    writer.WriteLine();
    foreach (string field in fields) {
      writer.WriteLine("      public " + field + " { get; }");
    }

    writer.WriteLine("    }");
  }

void defineAst(
    string outputDir, string baseName, List<string> types)
{
    string path = outputDir + "/" + baseName + ".cs";
    var writer = new StreamWriter(path);

    writer.WriteLine("namespace cslox {");
    writer.WriteLine("");
    writer.WriteLine("  using System;");
    writer.WriteLine("  using System.Collections;");
    writer.WriteLine("");
    writer.WriteLine("  abstract class " + baseName + " {");
    defineVisitor(writer, baseName, types);
    foreach (string type in types) {
      string className = type.Split(":")[0].Trim();
      string fields = type.Split(":")[1].Trim(); 
      defineType(writer, baseName, className, fields);
    }
    writer.WriteLine("");
    writer.WriteLine("    public abstract R Accept<R>(IVisitor<R> visitor);");
    writer.WriteLine("  }");
    writer.WriteLine("}");
    writer.Close();
}

var outputDir = "../ast/";

defineAst(outputDir, "Expr", new List<string>{
    "Binary   : Expr Left, Token Operator, Expr Right",
    "Grouping : Expr Expression",
    "Literal  : Object Value",
    "Unary    : Token Operator, Expr Right",
    "Ternary  : Expr Condition, Expr IfTrue, Expr IfFalse"
});

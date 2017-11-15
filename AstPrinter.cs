using System;
using System.Linq;
using System.Text;

namespace cslox
{
    internal class AstPrinter : Expr.IVisitor<string>
    {
        public AstPrinter()
        {
        }

        public string VisitBinaryExpr(Expr.Binary expr) => Parenthize(expr.Operator.Lexeme, expr.Left, expr.Right);

        public string VisitGroupingExpr(Expr.Grouping expr) => Parenthize("group", expr.Expression);

        public string VisitLiteralExpr(Expr.Literal expr) => expr.Value.ToString();

        public string VisitTernaryExpr(Expr.Ternary expr) => Parenthize("if", expr.Condition, expr.IfTrue, expr.IfFalse);

        public string VisitUnaryExpr(Expr.Unary expr) => Parenthize(expr.Operator.Lexeme, expr.Right);

        internal string Print(Expr tree) => tree.Accept(this);

        private string Parenthize(string name, params Expr[] rest)
        {
            var sb = new StringBuilder().Append("(").Append(name);
            foreach (var expr in rest)
            {
                sb.Append(" ");
                sb.Append(expr.Accept(this));
            }
            sb.Append(")");
            return sb.ToString();
        }
    }
}
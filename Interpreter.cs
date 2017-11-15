namespace CSLox
{
    internal class Interpreter : Expr.IVisitor<object>
    {
        public Interpreter()
        {
        }

        public object VisitBinaryExpr(Expr.Binary expr)
        {
            throw new System.NotImplementedException();
        }

        public object VisitGroupingExpr(Expr.Grouping expr)
        {
            throw new System.NotImplementedException();
        }

        public object VisitLiteralExpr(Expr.Literal expr)
        {
            throw new System.NotImplementedException();
        }

        public object VisitTernaryExpr(Expr.Ternary expr)
        {
            throw new System.NotImplementedException();
        }

        public object VisitUnaryExpr(Expr.Unary expr)
        {
            throw new System.NotImplementedException();
        }
    }
}
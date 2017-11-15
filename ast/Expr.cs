namespace cslox {

  using System;
  using System.Collections;

  abstract class Expr {
    public interface IVisitor<R> {
      R VisitBinaryExpr(Binary expr);
      R VisitGroupingExpr(Grouping expr);
      R VisitLiteralExpr(Literal expr);
      R VisitUnaryExpr(Unary expr);
      R VisitTernaryExpr(Ternary expr);
    }

    public sealed class Binary : Expr {
      public Binary(Expr Left, Token Operator, Expr Right) {
        this.Left = Left;
        this.Operator = Operator;
        this.Right = Right;
      }

      public override R Accept<R>(IVisitor<R> visitor) {
        return visitor.VisitBinaryExpr(this);
      }

      public Expr Left { get; }
      public Token Operator { get; }
      public Expr Right { get; }
    }

    public sealed class Grouping : Expr {
      public Grouping(Expr Expression) {
        this.Expression = Expression;
      }

      public override R Accept<R>(IVisitor<R> visitor) {
        return visitor.VisitGroupingExpr(this);
      }

      public Expr Expression { get; }
    }

    public sealed class Literal : Expr {
      public Literal(Object Value) {
        this.Value = Value;
      }

      public override R Accept<R>(IVisitor<R> visitor) {
        return visitor.VisitLiteralExpr(this);
      }

      public Object Value { get; }
    }

    public sealed class Unary : Expr {
      public Unary(Token Operator, Expr Right) {
        this.Operator = Operator;
        this.Right = Right;
      }

      public override R Accept<R>(IVisitor<R> visitor) {
        return visitor.VisitUnaryExpr(this);
      }

      public Token Operator { get; }
      public Expr Right { get; }
    }

    public sealed class Ternary : Expr {
      public Ternary(Expr Condition, Expr IfTrue, Expr IfFalse) {
        this.Condition = Condition;
        this.IfTrue = IfTrue;
        this.IfFalse = IfFalse;
      }

      public override R Accept<R>(IVisitor<R> visitor) {
        return visitor.VisitTernaryExpr(this);
      }

      public Expr Condition { get; }
      public Expr IfTrue { get; }
      public Expr IfFalse { get; }
    }

    public abstract R Accept<R>(IVisitor<R> visitor);
  }
}

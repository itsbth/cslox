namespace CSLox
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using static TokenType;

    internal class Parser
    {
        private readonly List<Token> tokens;
        private int current = 0;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        private bool IsAtEnd { get => Peek().Type == EOF; }

        public Expr Parse()
        {
            try
            {
                return Expression();
            }
            catch (ParseError)
            {
                return null;
            }
        }

        private Expr Expression() => Comma();

        private Expr Comma()
        {
            Expr lhs = Ternary();

            while (Match(COMMA))
            {
                Token op = Previous();
                Expr rhs = Ternary();
                lhs = new Expr.Binary(lhs, op, rhs);
            }

            return lhs;
        }

        private Expr Ternary()
        {
            Expr cond = Equality();

            if (Match(QUESTION))
            {
                Expr ifTrue = Equality();
                Consume(COLON, "Expected ':' to complete ternary expression.");
                Expr ifFalse = Expression();
                cond = new Expr.Ternary(cond, ifTrue, ifFalse);
            }

            return cond;
        }

        private Expr Equality()
        {
            Expr lhs = Comparison();

            while (Match(EQUAL_EQUAL, BANG_EQUAL))
            {
                Token op = Previous();
                Expr rhs = Comparison();
                lhs = new Expr.Binary(lhs, op, rhs);
            }

            return lhs;
        }

        private Expr Comparison()
        {
            Expr expr = Addition();

            while (Match(GREATER, GREATER_EQUAL, LESS, LESS_EQUAL))
            {
                Token op = Previous();
                Expr right = Addition();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Addition()
        {
            Expr expr = Multiplication();

            while (Match(MINUS, PLUS))
            {
                Token op = Previous();
                Expr right = Multiplication();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Multiplication()
        {
            Expr expr = Unary();

            while (Match(SLASH, STAR))
            {
                Token op = Previous();
                Expr right = Unary();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }

        private Expr Unary()
        {
            if (Match(BANG, MINUS))
            {
                Token op = Previous();
                Expr right = Unary();
                return new Expr.Unary(op, right);
            }

            return Primary();
        }

        private Expr Primary()
        {
            if (Match(FALSE))
            {
                return new Expr.Literal(false);
            }

            if (Match(TRUE))
            {
                return new Expr.Literal(true);
            }

            if (Match(NIL))
            {
                return new Expr.Literal(null);
            }

            if (Match(NUMBER, STRING))
            {
                return new Expr.Literal(Previous().Literal);
            }

            if (Match(LEFT_PAREN))
            {
                Expr expr = Expression();
                Consume(RIGHT_PAREN, "Expected ')' after expression.");
                return new Expr.Grouping(expr);
            }

            throw Error(Peek(), "Expected expression.");
        }

        private Token Consume(TokenType type, string message)
        {
            if (Check(type))
            {
                return Advance();
            }

            throw Error(Peek(), message);
        }

        private Exception Error(Token token, string message)
        {
            Program.Error(token, message);
            throw new ParseError();
        }

        private void Synchronize()
        {
            Advance();

            while (!IsAtEnd)
            {
                if (Previous().Type == SEMICOLON)
                {
                    return;
                }

                switch (Peek().Type)
                {
                    case CLASS:
                    case FUN:
                    case VAR:
                    case FOR:
                    case IF:
                    case WHILE:
                    case PRINT:
                    case RETURN:
                        return;
                }

                Advance();
            }
        }

        private bool Match(params TokenType[] types)
        {
            if (types.Any(Check))
            {
                Advance();
                return true;
            }

            return false;
        }

        private Token Advance()
        {
            if (!IsAtEnd)
            {
                current += 1;
            }

            return Previous();
        }

        private bool Check(TokenType arg) => Peek().Type == arg;

        private Token Peek() => tokens[current];

        private Token Previous() => tokens[current - 1];
    }
}
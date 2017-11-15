namespace CSLox
{
    internal enum TokenType
    {
#pragma warning disable SA1136 // Enum values should be on separate lines
#pragma warning disable SA1602 // Enumeration items should be documented
        // Single-character tokens.
        LEFT_PAREN, RIGHT_PAREN, LEFT_BRACE, RIGHT_BRACE,
        COMMA, DOT, MINUS, PLUS, SEMICOLON, SLASH, STAR,
        QUESTION, COLON,

        // One or two character tokens.
        BANG, BANG_EQUAL,
        EQUAL, EQUAL_EQUAL,
        GREATER, GREATER_EQUAL,
        LESS, LESS_EQUAL,

        // Literals.
        IDENTIFIER, STRING, NUMBER,

        // Keywords.
        AND, CLASS, ELSE, FALSE, FUN, FOR, IF, NIL, OR,
        PRINT, RETURN, SUPER, THIS, TRUE, VAR, WHILE,

        EOF,
#pragma warning restore SA1602 // Enumeration items should be documented
#pragma warning restore SA1136 // Enum values should be on separate lines
    }
}
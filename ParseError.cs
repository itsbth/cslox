namespace CSLox
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    internal class ParseError : Exception
    {
        public ParseError()
        {
        }

        public ParseError(string message)
            : base(message)
        {
        }

        public ParseError(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ParseError(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
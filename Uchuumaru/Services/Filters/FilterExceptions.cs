using System;

namespace Uchuumaru.Services.Filters
{
    public class ExpressionExistsException : Exception
    {
        public ExpressionExistsException(string expression) : base($"Expression {expression} already exists.")
        {
        }
    }
    
    public class ExpressionDoesNotExistException : Exception
    {
        public ExpressionDoesNotExistException(string expression) : base($"Expression {expression} does not exist.")
        {
        }
    }
}
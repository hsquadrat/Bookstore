using System.Runtime.Serialization;

namespace Bookstore.Application.Exceptions;

[Serializable]
public class BookForIsbnDuplicateException : Exception
{
    public BookForIsbnDuplicateException()
    {
    }

    public BookForIsbnDuplicateException(string? message) : base(message)
    {
    }

    public BookForIsbnDuplicateException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected BookForIsbnDuplicateException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
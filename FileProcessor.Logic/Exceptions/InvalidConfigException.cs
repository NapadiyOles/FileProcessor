namespace FileProcessor.Logic.Exceptions;

public class InvalidConfigException : Exception
{
    public InvalidConfigException()
    {
    }

    public InvalidConfigException(string message) : base(message)
    {
        
    }

    public InvalidConfigException(string message, Exception inner) : base(message, inner)
    {
    }
}
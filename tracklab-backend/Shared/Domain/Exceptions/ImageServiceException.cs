namespace TrackLab.Shared.Domain.Exceptions;

public class ImageServiceException : Exception
{
    public ImageServiceException(string message) : base(message) { }
    
    public ImageServiceException(string message, Exception innerException) : base(message, innerException) { }
}

public class ImageUploadException : ImageServiceException
{
    public ImageUploadException(string message) : base(message) { }
    
    public ImageUploadException(string message, Exception innerException) : base(message, innerException) { }
}

public class ImageDeleteException : ImageServiceException
{
    public ImageDeleteException(string message) : base(message) { }
    
    public ImageDeleteException(string message, Exception innerException) : base(message, innerException) { }
}

public class InvalidImageUrlException : ImageServiceException
{
    public InvalidImageUrlException(string message) : base(message) { }
} 
namespace NDB.Abstraction.Common;

public sealed class FileObject
{
    public string Filename { get; init; } = string.Empty;
    public string MimeType { get; init; } = string.Empty;
    public string Base64 { get; init; } = string.Empty;
}

public class FileByteObject
{
    public byte[]? File { get; set; }
    public string Filename { get; init; } = string.Empty;
    public string MimeType { get; init; } = string.Empty;
}

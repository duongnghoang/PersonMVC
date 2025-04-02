namespace PersonMVC.Responses;

public class FileResponse(byte[] fileContent, string contentType, string fileName)
{
    public byte[] FileContent { get; set; } = fileContent;
    public string ContentType { get; set; } = contentType;
    public string FileName { get; set; } = fileName;
}
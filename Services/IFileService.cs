namespace Coswalt.Services;

public interface IFileService
{
    string? OpenFile();
    string? OpenFileContent();
    string? SaveFile();
    void SaveFileContent(string content);
}
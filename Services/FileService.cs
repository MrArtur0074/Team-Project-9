using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace Coswalt.Services;

public class FileService : IFileService
{
    public string? OpenFile() {
        var openFileDialog = new OpenFileDialog {
            Title = "Open file",
            Filter = "All files (*.*)|*.*"
        };

        if (openFileDialog.ShowDialog() == true) {
            MessageBox.Show($"File selected: {openFileDialog.FileName}", "File opened", MessageBoxButton.OK,
                MessageBoxImage.Information);
            return openFileDialog.FileName;
        }

        return null;
    }

    public string? OpenFileContent() {
        var path = OpenFile();
        if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
            return File.ReadAllText(path);
        }

        return null;
    }

    public string? SaveFile() {
        var saveFileDialog = new SaveFileDialog {
            Title = "Save file",
            Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
        };

        if (saveFileDialog.ShowDialog() == true) {
            MessageBox.Show($"Файл будет сохранён сюда: {saveFileDialog.FileName}", "Файл сохранён",
                MessageBoxButton.OK, MessageBoxImage.Information);
            return saveFileDialog.FileName;
        }

        return null;
    }

    public void SaveFileContent(string content) {
        var path = SaveFile();
        if (!string.IsNullOrEmpty(path)) {
            File.WriteAllText(path, content);
        }
    }
}
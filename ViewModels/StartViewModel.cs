using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Coswalt.Services;

namespace Coswalt.ViewModels;

public class StartViewModel : ObservableObject
{
    private readonly IFileService _fileService;
    
    public ICommand OpenFileCommand { get; }

    public StartViewModel(IFileService fileService) {
        _fileService = fileService;
        OpenFileCommand   = new RelayCommand(OpenFile);
    }

    private void OpenFile() {
        var path = _fileService.OpenFile();
        if (!string.IsNullOrEmpty(path)) {
            // e.g., Load project from path
        }
    }
}
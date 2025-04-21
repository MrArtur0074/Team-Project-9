using System.Windows.Input;
using Microsoft.Win32;
using Oswalt.Models;
using Oswalt.Services;
using System.Windows;
using CommunityToolkit.Mvvm.Input; // Для RelayCommand

namespace Oswalt.ViewModels;

public class DwgExportViewModel : ViewModelBase
{
    private readonly Wing _wing;
    private readonly IDwgExportService _dwgExportService;

    public DwgExportViewModel(Wing wing, IDwgExportService dwgExportService)
    {
        _wing = wing;
        _dwgExportService = dwgExportService;

        ExportCommand = new RelayCommand(ExecuteExport);
    }

    public ICommand ExportCommand { get; }

    private void ExecuteExport()
    {
        var dialog = new SaveFileDialog
        {
            Title = "Export DWG",
            Filter = "AutoCAD DWG Files (*.dwg)|*.dwg|All files (*.*)|*.*",
            DefaultExt = "dwg"
        };

        if (dialog.ShowDialog() == true)
        {
            var filePath = dialog.FileName;
            _dwgExportService.ExportAsync(_wing, filePath);

            // Можно добавить уведомление об успешном экспорте
            MessageBox.Show($"Файл успешно экспортирован: {filePath}",
                          "Экспорт завершен",
                          MessageBoxButton.OK,
                          MessageBoxImage.Information);
        }
    }
}
using System.IO;
using System.Windows.Input;
using Microsoft.Win32;
using Oswalt.Models;
using Oswalt.Services;
using ReactiveUI;

namespace Project9.ViewModels;

public class AirfoilConfigViewModel : ViewModelBase
{
    private readonly Wing _wing;
    private readonly UndoRedoService _undoRedo;

    public AirfoilConfigViewModel(Wing wing, UndoRedoService undoRedo)
    {
        _wing = wing;
        _undoRedo = undoRedo;
    }

    public Airfoil RootAirfoil => _wing.RootAirfoil;
    public Airfoil TipAirfoil => _wing.TipAirfoil;

    public ICommand ImportRootAirfoilCommand =>
        ReactiveCommand.Create(() => ProcessImport(nameof(Wing.RootAirfoil)));

    public ICommand ImportTipAirfoilCommand =>
        ReactiveCommand.Create(() => ProcessImport(nameof(Wing.TipAirfoil)));

    private void ProcessImport(string paramName)
    {
        var dialog = new OpenFileDialog
        {
            Title = "Select Airfoil DAT File",
            Filter = "DAT files (*.dat)|*.dat|All files (*.*)|*.*",
            Multiselect = false
        };

        if (dialog.ShowDialog() == true)
        {
            string filePath = dialog.FileName;
            string fileContent = File.ReadAllText(filePath);
            Airfoil airfoil = AirfoilParserService.Parse(fileContent);
            _undoRedo.Execute(new SetPropertyCommand<Airfoil>(_wing, paramName, airfoil));
        }
    }
}

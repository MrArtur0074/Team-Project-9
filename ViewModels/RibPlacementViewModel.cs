using System.Windows.Input;
using Oswalt.Models;
using Oswalt.Models.Commands;
using Oswalt.Services;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Win32;
using System.Windows;

namespace Project9.ViewModels;

public partial class RibPlacementViewModel : ObservableObject
{
    private readonly Wing _wing;
    private readonly UndoRedoService _undoRedo;
    private int _selectedRibIndex;

    public RibPlacementViewModel(Wing wing, UndoRedoService undoRedo)
    {
        _wing = wing;
        _undoRedo = undoRedo;

        // Инициализация команд
        AddRibCommand = new RelayCommand<int>(AddRib);
        RemoveRibCommand = new RelayCommand<int>(RemoveRib);
        MoveRibCommand = new RelayCommand<(int, double)>(MoveRib);
        ResetRibsCommand = new RelayCommand<int>(ResetRibs);
        ClearRibsCommand = new RelayCommand(ClearRibs);
    }

    public RibCollection Ribs => _wing.Ribs;

    public int SelectedRibIndex
    {
        get => _selectedRibIndex;
        set => SetProperty(ref _selectedRibIndex, value);
    }

    public ICommand AddRibCommand { get; }
    public ICommand RemoveRibCommand { get; }
    public ICommand MoveRibCommand { get; }
    public ICommand ResetRibsCommand { get; }
    public ICommand ClearRibsCommand { get; }

    private void AddRib(int index)
    {
        _undoRedo.Execute(new RibCommand(Ribs, RibCommand.RibAction.Add, index));
    }

    private void RemoveRib(int index)
    {
        _undoRedo.Execute(new RibCommand(Ribs, RibCommand.RibAction.Remove, index));
    }

    private void MoveRib((int index, double shift) args)
    {
        _undoRedo.Execute(new RibCommand(Ribs, RibCommand.RibAction.Move, args.index, args.shift));
    }

    private void ResetRibs(int n)
    {
        _undoRedo.Execute(new RibCommand(Ribs, RibCommand.RibAction.Reset, -1, 0.0, n));
    }

    private void ClearRibs()
    {
        _undoRedo.Execute(new RibCommand(Ribs, RibCommand.RibAction.Clear));
    }
}
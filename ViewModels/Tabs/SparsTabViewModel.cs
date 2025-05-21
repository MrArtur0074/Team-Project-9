using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Coswalt.Models;

namespace Coswalt.ViewModels.Tabs;

public class SparsTabViewModel : BaseViewModel
{
    public ObservableCollection<Spar> Spars { get; set; } = [];

    public ICommand AddCircleSparCommand { get; }
    public ICommand AddRectSparCommand { get; }
    public ICommand DeleteSparCommand { get; }

    private int sparCounter = 1;

    public SparsTabViewModel() {
        AddCircleSparCommand = new RelayCommand(AddCircleSpar);
        AddRectSparCommand = new RelayCommand(AddRectSpar);
        DeleteSparCommand = new RelayCommand<Spar>(DeleteSpar);
    }

    private void AddCircleSpar() {
        var spar = new CircleSpar(
            ribCount: 2,
            startRib: 0,
            endRib: 1,
            startChordOffset: 0.3,
            endChordOffset: 0.3,
            alignment: Spar.AlignmentType.Linear,
            yOffset: 0.0,
            radius: 10.0
        );
        Spars.Add(spar);
    }

    private void AddRectSpar() {
        var spar = new RectSpar(
            ribCount: 2,
            startRib: 0,
            endRib: 1,
            startChordOffset: 0.3,
            endChordOffset: 0.3,
            alignment: Spar.AlignmentType.Linear,
            yOffset: 0.0,
            height: 5.0,
            width: 5.0
        ) {
            ProfileAlignment = RectSpar.ProfileAlignmentType.Upper
        };
        Spars.Add(spar);
    }

    private void DeleteSpar(Spar? spar) {
        if (spar != null) {
            Spars.Remove(spar);
        }
    }

    private bool _isCollapsed;

    public bool IsCollapsed {
        get => _isCollapsed;
        set {
            if (_isCollapsed != value) {
                _isCollapsed = value;
                OnPropertyChanged();
            }
        }
    }
}
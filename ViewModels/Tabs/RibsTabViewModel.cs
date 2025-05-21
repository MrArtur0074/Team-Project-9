using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Coswalt.ViewModels.Tabs;

public class RibsTabViewModel : BaseViewModel
{
    private int _ribsNumber;
    private double _tipExclusionRatio;
    private GeometryTabViewModel _geometryTab;

    public int RibsNumber {
        get => _ribsNumber;
        set
        {
            if (value >= 2) {
                _ribsNumber = value;
                OnPropertyChanged();
            }
        }
    }

    public double TipExclusionRatio {
        get => _tipExclusionRatio;
        set
        {
            if (value >= 0.1 && value <= 0.5) {
                _tipExclusionRatio = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsTipExclusionRatioVisible {
        get => _geometryTab.SelectedPlanform == "Elliptical";
    }

    public ICommand NextTabCommand { get; }

    public RibsTabViewModel() {
        _geometryTab =
            (GeometryTabViewModel)((MainViewModel)App.Current.MainWindow.DataContext).Tabs["Geometry"];

        _geometryTab.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(GeometryTabViewModel.SelectedPlanform)) {
                OnPropertyChanged(nameof(IsTipExclusionRatioVisible));
            }
        };

        NextTabCommand = ((MainViewModel)App.Current.MainWindow.DataContext).NextTabCommand;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string name = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
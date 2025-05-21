using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Coswalt.Models;

namespace Coswalt.ViewModels.Tabs;

public class GeometryTabViewModel : BaseViewModel
{
    private string _selectedPlanform = "Straight";

    private double _rootChord;
    private double _span;
    private double _incidenceAngle;

    private string? _taperRatio;
    private string? _sweepCoefficient;

    public ObservableCollection<string> PlanformOptions { get; } = ["Straight", "Tapered", "Elliptical"];

    public string SelectedPlanform {
        get => _selectedPlanform;
        set {
            if (SetProperty(ref _selectedPlanform, value)) {
                OnPropertyChanged(nameof(IsTaperedVisible));
                OnPropertyChanged(nameof(IsEllipticalVisible));
            }
        }
    }

    public bool IsTaperedVisible {
        get => SelectedPlanform == "Tapered";
    }
    
    public bool IsEllipticalVisible {
        get => SelectedPlanform == "Elliptical";
    }

    public string? TaperRatio {
        get => _taperRatio;
        set {
            if (_taperRatio != value) {
                _taperRatio = value;
                OnPropertyChanged();
            }
        }
    }

    public string? SweepCoefficient {
        get => _sweepCoefficient;
        set {
            if (_sweepCoefficient != value) {
                _sweepCoefficient = value;
                OnPropertyChanged();
            }
        }
    }

    public double Span {
        get => _span;
        set {
            if (value is >= WingConstraints.MinWingSpan and <= WingConstraints.MaxWingSpan) {
                _span = value;
                OnPropertyChanged();
            }
        }
    }

    public double IncidenceAngle {
        get => _incidenceAngle;
        set {
            if (value is >= WingConstraints.MinIncidenceAngle and <= WingConstraints.MaxIncidenceAngle) {
                _incidenceAngle = value;
                OnPropertyChanged();
            }
        }
    }

    public double RootChordLength {
        get => _rootChord;
        set {
            if (value is >= 100 and <= 1000) {
                _rootChord = value;
                OnPropertyChanged();
            }
        }
    }
}
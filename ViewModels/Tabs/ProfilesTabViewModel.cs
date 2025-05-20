using System.Collections.ObjectModel;
using System.IO;
using Coswalt.Services;

namespace Coswalt.ViewModels.Tabs;

public class ProfilesTabViewModel : BaseViewModel
{
    private readonly IAirfoilService _airfoilService;

    public ObservableCollection<string> AirfoilOptions { get; } = []; // TODO: fix it

    private string _selectedRootAirfoil;

    public string SelectedRootAirfoil {
        get => _selectedRootAirfoil;
        set => SetProperty(ref _selectedRootAirfoil, value);
    }

    private string _selectedTipAirfoil;

    public string SelectedTipAirfoil {
        get => _selectedTipAirfoil;
        set => SetProperty(ref _selectedTipAirfoil, value);
    }

    public ProfilesTabViewModel() {
        string csvPath = Path.Combine(AppContext.BaseDirectory,
            Config.Config.GetValue("AirfoilsCsvPath") ?? String.Empty);
        string dirPath = Path.Combine(AppContext.BaseDirectory,
            Config.Config.GetValue("AirfoilsDirPath") ?? String.Empty);

        _airfoilService = new CsvAirfoilService(csvPath, dirPath);
        LoadAirfoils();
    }

    private async void LoadAirfoils() {
        try {
            var airfoils = await _airfoilService.GetAvailableAirfoilsAsync();
            AirfoilOptions.Clear();
            foreach (var airfoil in airfoils) {
                AirfoilOptions.Add(airfoil);
            }
        } catch (Exception e) {
            await Console.Error.WriteLineAsync("Profiles load error: " + e.Message);
        }
    }
}
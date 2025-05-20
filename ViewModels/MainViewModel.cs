using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Coswalt.ViewModels.Tabs;
using Coswalt.Views;
using Coswalt.Services;

namespace Coswalt.ViewModels;

public class MainViewModel : BaseViewModel
{
    public ICommand ShowEditorCommand { get; }
    public ICommand SwitchToGeometryTabCommand { get; }
    public ICommand SwitchToProfilesTabCommand { get; }
    public ICommand SwitchToRibsTabCommand { get; }
    public ICommand SwitchToSparsTabCommand { get; }
    public ICommand NextTabCommand { get; }
    public ICommand BackTabCommand { get; }

    public ICommand OpenFileCommand { get; }
    public ICommand SaveFileCommand { get; }
    public ICommand ReturnToStartCommand { get; }

    public ICommand MinimizeWindowCommand { get; }
    public ICommand ToggleMaximizeCommand { get; }
    public ICommand CloseWindowCommand { get; }

    public ICommand AboutCommand { get; }

    private IFileService _fileService;
    private bool _isEditorVisible;

    public bool IsEditorVisible {
        get => _isEditorVisible;
        set => SetProperty(ref _isEditorVisible, value);
    }

    private object _currentView;

    public object CurrentView {
        get => _currentView;
        set => SetProperty(ref _currentView, value);
    }

    private object _currentEditPanelView;

    public object CurrentEditPanelView {
        get => _currentEditPanelView;
        set => SetProperty(ref _currentEditPanelView, value);
    }

    private string _currentTab = "Geometry";

    private readonly List<string> _tabOrder = new() {
        "Geometry", "Profiles", "Ribs", "Spars"
    };

    private readonly StartView _startView = new();
    private readonly EditorView _editorView = new();

    private string _selectedTabName;

    public string SelectedTabName {
        get => _selectedTabName;
        set => SetProperty(ref _selectedTabName, value);
    }

    public Dictionary<string, BaseViewModel> Tabs { get; } = new();

    public MainViewModel() {
        _fileService               = new FileService();
        CurrentView                = _startView;
        ShowEditorCommand          = new RelayCommand(ShowEditor);
        SwitchToGeometryTabCommand = new RelayCommand(SwitchTab("Geometry"));
        SwitchToProfilesTabCommand = new RelayCommand(SwitchTab("Profiles"));
        SwitchToRibsTabCommand     = new RelayCommand(SwitchTab("Ribs"));
        SwitchToSparsTabCommand    = new RelayCommand(SwitchTab("Spars"));
        NextTabCommand             = new RelayCommand(SwitchToNextTab);
        BackTabCommand             = new RelayCommand(SwitchToPreviousTab);

        OpenFileCommand      = new RelayCommand(OpenFile);
        SaveFileCommand      = new RelayCommand(SaveFile);
        ReturnToStartCommand = new RelayCommand(ReturnToStartScreen);

        MinimizeWindowCommand = new RelayCommand(MinimizeWindow);
        ToggleMaximizeCommand = new RelayCommand(ToggleMaximize);
        CloseWindowCommand    = new RelayCommand(CloseWindow);
        AboutCommand          = new RelayCommand(OpenAboutPage);
    }

    private void ShowEditor() {
        IsEditorVisible = true;
        CurrentView     = _editorView;

        Tabs.Clear();
        Tabs["Geometry"] = new GeometryTabViewModel();
        Tabs["Profiles"] = new ProfilesTabViewModel();
        Tabs["Ribs"]     = new RibsTabViewModel();
        Tabs["Spars"]    = new SparsTabViewModel();

        SwitchTab("Geometry");
    }

    private Action SwitchTab(string tabName) {
        return () =>
        {
            _currentTab     = tabName;
            SelectedTabName = tabName;

            if (!Tabs.ContainsKey(tabName)) {
                CurrentEditPanelView = null;
                OnPropertyChanged(nameof(CurrentEditPanelView));
                return;
            }

            switch (tabName) {
                case "Geometry":
                    CurrentEditPanelView = new GeometryTabView {
                        DataContext = Tabs["Geometry"]
                    };
                    break;
                case "Profiles":
                    CurrentEditPanelView = new ProfilesTabView {
                        DataContext = Tabs["Profiles"]
                    };
                    break;
                case "Ribs":
                    CurrentEditPanelView = new RibsTabView {
                        DataContext = Tabs["Ribs"]
                    };
                    break;
                case "Spars":
                    CurrentEditPanelView = new SparsTabView {
                        DataContext = Tabs["Spars"]
                    };
                    break;
                default:
                    CurrentEditPanelView = null;
                    break;
            }

            OnPropertyChanged(nameof(CurrentEditPanelView));
        };
    }

    private void SwitchToNextTab() {
        int index = _tabOrder.IndexOf(_currentTab);
        if (index >= 0 && index < _tabOrder.Count - 1) {
            SwitchTab(_tabOrder[index + 1])();
        }
    }

    private void SwitchToPreviousTab() {
        int index = _tabOrder.IndexOf(_currentTab);
        if (index > 0) {
            SwitchTab(_tabOrder[index - 1])();
        } else {
            CurrentView     = _startView;
            IsEditorVisible = false;
        }
    }

    private void OpenFile() {
        var path = _fileService.OpenFile();
        if (!string.IsNullOrEmpty(path)) {
            // обработка открытия файла
        }
    }

    private void SaveFile() {
        var path = _fileService.SaveFile();
        if (!string.IsNullOrEmpty(path)) {
            
        }
    }

    private void ReturnToStartScreen() {
        CurrentView     = _startView;
        IsEditorVisible = false;
    }

    private void MinimizeWindow() {
        var window = Application.Current.MainWindow;
        if (window != null) {
            window.WindowState = WindowState.Minimized;
        }
    }

    private void ToggleMaximize() {
        var window = Application.Current.MainWindow;
        if (window != null) {
            window.WindowState = window.WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }
    }

    private void CloseWindow() {
        Application.Current.MainWindow?.Close();
    }

    private void OpenAboutPage() {
        var url = "https://stupendous-dracopelta-55b.notion.site/Project-9-183b680cf8cb80388981d5a2d192824a?pvs=4";
        try {
            Process.Start(new ProcessStartInfo {
                FileName = url, UseShellExecute = true
            });
        } catch (Exception ex) {
            MessageBox.Show($"Couldn't open web-site: {ex.Message}", "Error: ", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
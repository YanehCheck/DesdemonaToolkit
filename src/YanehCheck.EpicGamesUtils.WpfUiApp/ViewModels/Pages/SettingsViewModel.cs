﻿using System.Reflection;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.ViewModels.Pages;

public partial class SettingsViewModel : ObservableObject, IViewModel, INavigationAware {
    private bool _isInitialized = false;

    [ObservableProperty]
    private string _appVersion = string.Empty;

    [ObservableProperty]
    private ApplicationTheme _currentTheme = ApplicationTheme.Unknown;

    public void OnNavigatedTo() {
        if(!_isInitialized) {
            InitializeViewModel();
        }
    }

    public void OnNavigatedFrom() { }

    private void InitializeViewModel() {
        CurrentTheme = ApplicationThemeManager.GetAppTheme();
        AppVersion = $"Development version: {GetAssemblyVersion()}";

        _isInitialized = true;
    }

    private string GetAssemblyVersion() {
        return Assembly.GetExecutingAssembly().GetName().Version?.ToString()
               ?? string.Empty;
    }

    [RelayCommand]
    private void OnChangeTheme(string parameter) {
        switch(parameter) {
            case "theme_light":
                if(CurrentTheme == ApplicationTheme.Light) {
                    break;
                }

                ApplicationThemeManager.Apply(ApplicationTheme.Light);
                CurrentTheme = ApplicationTheme.Light;

                break;

            default:
                if(CurrentTheme == ApplicationTheme.Dark) {
                    break;
                }

                ApplicationThemeManager.Apply(ApplicationTheme.Dark);
                CurrentTheme = ApplicationTheme.Dark;

                break;
        }
    }
}
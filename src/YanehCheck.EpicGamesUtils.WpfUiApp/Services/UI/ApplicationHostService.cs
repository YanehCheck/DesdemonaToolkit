﻿using Microsoft.Extensions.Hosting;
using Wpf.Ui;
using YanehCheck.EpicGamesUtils.WpfUiApp.Views.Pages;
using YanehCheck.EpicGamesUtils.WpfUiApp.Views.Windows;

namespace YanehCheck.EpicGamesUtils.WpfUiApp.Services.UI;

/// <summary>
/// Managed host of the application.
/// </summary>
public class ApplicationHostService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    private INavigationWindow _navigationWindow;

    public ApplicationHostService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Triggered when the application host is ready to start the service.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await HandleActivationAsync();
    }

    /// <summary>
    /// Triggered when the application host is performing a graceful shutdown.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }

    /// <summary>
    /// Creates main window during activation.
    /// </summary>
    private async Task HandleActivationAsync()
    {
        if (!Application.Current.Windows.OfType<MainWindow>().Any())
        {
            _navigationWindow = (
                _serviceProvider.GetService(typeof(IExtendedNavigationWindow)) as IExtendedNavigationWindow
            )!;
            _navigationWindow!.ShowWindow();

            _navigationWindow.Navigate(typeof(HomePage));
        }

        await Task.CompletedTask;
    }
}
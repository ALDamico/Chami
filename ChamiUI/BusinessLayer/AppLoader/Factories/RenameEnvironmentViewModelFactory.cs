using System;
using ChamiUI.BusinessLayer.Services;
using ChamiUI.PresentationLayer.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ChamiUI.BusinessLayer.AppLoader.Factories;

internal static class RenameEnvironmentViewModelFactory
{
    internal static RenameEnvironmentViewModel BuildRenameEnvironmentViewModel(IServiceProvider sp)
    {
        var mainWindowViewModel = sp.GetRequiredService<MainWindowViewModel>();
        var initialName = mainWindowViewModel.SelectedEnvironment.Name;
        var renameService = sp.GetRequiredService<RenameEnvironmentService>();
        var viewModel = new RenameEnvironmentViewModel(renameService)
        {
            Name = initialName
        };
        renameService.EnvironmentRenamed += mainWindowViewModel.OnEnvironmentRenamed;

        return viewModel;
    }
}
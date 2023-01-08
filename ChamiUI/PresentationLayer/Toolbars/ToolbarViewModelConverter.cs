using System;
using System.Globalization;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using AsyncAwaitBestPractices.MVVM;
using ChamiUI.Controls;
using ChamiUI.Localization;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.PresentationLayer.Toolbars;

public class ToolbarViewModelConverter
{
    private BooleanToVisibilityConverter _booleanToVisibilityConverter;

    protected BooleanToVisibilityConverter BooleanToVisibilityConverter
    {
        get => _booleanToVisibilityConverter ??= new BooleanToVisibilityConverter();
        set => _booleanToVisibilityConverter = value;
    }

    public ToolBar ConvertToToolBar(ToolBarViewModel toolBarViewModel, CultureInfo cultureInfo = null)
    {
        var toolBar = new ToolBar();
        toolBar.Visibility =
            (Visibility) BooleanToVisibilityConverter.Convert(toolBarViewModel.IsVisible, typeof(Visibility), null,
                cultureInfo);
        toolBar.Tag = toolBarViewModel.Name;
        
        foreach (var buttonViewModel in toolBarViewModel.ToolbarButtonViewModels)
        {
            string actualCaption = null;
            if (buttonViewModel.Caption != null)
            {
                actualCaption = ChamiUIStrings.ResourceManager.GetString(buttonViewModel.Caption, cultureInfo);    
            }

            string actualToolTip = null;
            if (buttonViewModel.ToolTip != null)
            {
                actualToolTip = ChamiUIStrings.ResourceManager.GetString(buttonViewModel.ToolTip, cultureInfo);
            }
            
            var buttonMetadata = new ToolbarButtonMetadata()
            {
                Name = toolBarViewModel.Name,
                CaptionKey = buttonViewModel.Caption,
                ToolTipKey = buttonViewModel.ToolTip
            };

            var button = new IconButton();
            button.Tag = buttonMetadata;
            button.ButtonText = actualCaption;
            button.ToolTip = actualToolTip;
            button.Icon = buttonViewModel.Icon;
            button.IconColor = buttonViewModel.ForegroundColor;

            if (buttonViewModel.CommandName != null)
            {
                var command = Activator.CreateInstance(null, buttonViewModel.CommandName);
                if (command != null)
                {
                    button.ClickCommand = (IAsyncCommand) command.Unwrap();
                }

            }
            
            toolBar.Items.Add(button);
        }

        return toolBar;
    }

    public ToolBarViewModel ConvertToToolBarViewModel(ToolBar toolBar, CultureInfo cultureInfo)
    {
        var toolbarViewModel = new ToolBarViewModel();
        toolbarViewModel.Name = (string) toolBar.Tag;
        toolbarViewModel.IsVisible =
            (bool) BooleanToVisibilityConverter.ConvertBack(toolBar.Visibility, typeof(bool), null, cultureInfo);
        foreach (var element in toolBar.Items)
        {
            var buttonViewModel = new ToolbarButtonViewModel();

            if (element is IconButton iconButton)
            {
                var buttonMetadata = (ToolbarButtonMetadata) iconButton.Tag;
                buttonViewModel.Caption = buttonMetadata.CaptionKey;
                buttonViewModel.CommandName = iconButton.ClickCommand.GetType().FullName;
                buttonViewModel.Icon = iconButton.Icon;
                buttonViewModel.ForegroundColor = iconButton.GetActualForegroundColor();
                buttonViewModel.ToolTip = buttonMetadata.CaptionKey;
            }

            toolbarViewModel.ToolbarButtonViewModels.Add(buttonViewModel);
        }

        return toolbarViewModel;
    }
}
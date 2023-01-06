using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using AsyncAwaitBestPractices.MVVM;
using ChamiUI.Controls;
using ChamiUI.PresentationLayer.ViewModels;
using Newtonsoft.Json;

namespace ChamiUI.PresentationLayer.Toolbars;

public class ToolBarConverter
{
    public ToolBarConverter()
    {
        _booleanToVisibilityConverter = new BooleanToVisibilityConverter();
    }

    private readonly BooleanToVisibilityConverter _booleanToVisibilityConverter;

    public ToolBar Deserialize(string json, CultureInfo cultureInfo = null)
    {
        if (cultureInfo == null)
        {
            cultureInfo = CultureInfo.CurrentUICulture;
        }

        var converted = JsonConvert.DeserializeObject<ToolBarViewModel>(json);
        if (converted == null)
        {
            return null;
        }

        var toolBar = new ToolBar();
        toolBar.Visibility =
            (Visibility)_booleanToVisibilityConverter.Convert(converted.IsVisible, typeof(Visibility), null,
                cultureInfo);
        toolBar.Tag = converted.Name;

        foreach (var buttonViewModel in converted.ToolbarButtonViewModels)
        {
            var button = new IconButton();
            button.ButtonText = buttonViewModel.Caption;
            button.ToolTip = buttonViewModel.ToolTip;
            button.Icon = buttonViewModel.Icon;
            button.IconColor = buttonViewModel.ForegroundColor;

            var command = Activator.CreateInstance(null, buttonViewModel.CommandName);
            if (command != null)
            {
                button.ClickCommand = (IAsyncCommand)command.Unwrap();
            }

            toolBar.Items.Add(button);
        }

        return toolBar;
    }

    public string Serialize(ToolBar toolBar, CultureInfo cultureInfo = null)
    {
        if (cultureInfo == null)
        {
            cultureInfo = CultureInfo.CurrentUICulture;
        }

        if (toolBar == null)
        {
            return null;
        }

        var toolbarViewModel = new ToolBarViewModel();
        toolbarViewModel.Name = (string)toolBar.Tag;
        toolbarViewModel.IsVisible =
            (bool) _booleanToVisibilityConverter.ConvertBack(toolBar.Visibility, typeof(bool), null, cultureInfo);

        foreach (var element in toolBar.Items)
        {
            var buttonViewModel = new ToolbarButtonViewModel();
            if (element is IconButton iconButton)
            {
                buttonViewModel.Caption = iconButton.ButtonText;
                buttonViewModel.CommandName = iconButton.ClickCommand.GetType().FullName;
                buttonViewModel.Icon = iconButton.Icon;
                buttonViewModel.ForegroundColor = iconButton.GetActualForegroundColor();
                buttonViewModel.ToolTip = (string) iconButton.ToolTip;
            }
            toolbarViewModel.ToolbarButtonViewModels.Add(buttonViewModel);
        }

        return JsonConvert.SerializeObject(toolbarViewModel);
    }
}
using System;
using System.Collections.Generic;
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

    public List<ToolBar> Deserialize(string json, CultureInfo cultureInfo = null)
    {
        if (cultureInfo == null)
        {
            cultureInfo = CultureInfo.CurrentUICulture;
        }

        var toolBars = new List<ToolBar>();

        var converted = JsonConvert.DeserializeObject<ToolBarViewModel>(json);
        if (converted == null)
        {
            return null;
        }

        int row = 0;
        int column = 0;

        foreach (var toolbarViewModel in converted.ToolbarButtonViewModels)
        {
            var toolBar = new ToolBar();
            toolBar.Visibility =
                (Visibility)_booleanToVisibilityConverter.Convert(converted.IsVisible, typeof(Visibility), null,
                    cultureInfo);
            toolBar.Tag = converted.Name;
            foreach (var buttonViewModel in toolbarViewModel)
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

                toolBar.Band = column;
                toolBar.BandIndex = row;
                toolBar.Items.Add(button);

                column++;
            }

            row++;

            toolBars.Add(toolBar);
        }

        return toolBars;
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
            (bool)_booleanToVisibilityConverter.ConvertBack(toolBar.Visibility, typeof(bool), null, cultureInfo);
        var currentButtonList = new List<ToolbarButtonViewModel>();
        foreach (var element in toolBar.Items)
        {
            var buttonViewModel = new ToolbarButtonViewModel();

            if (element is IconButton iconButton)
            {
                buttonViewModel.Caption = iconButton.ButtonText;
                buttonViewModel.CommandName = iconButton.ClickCommand.GetType().FullName;
                buttonViewModel.Icon = iconButton.Icon;
                buttonViewModel.ForegroundColor = iconButton.GetActualForegroundColor();
                buttonViewModel.ToolTip = (string)iconButton.ToolTip;
            }

            currentButtonList.Add(buttonViewModel);
        }

        toolbarViewModel.ToolbarButtonViewModels.Add(currentButtonList);
        return JsonConvert.SerializeObject(toolbarViewModel);
    }
}
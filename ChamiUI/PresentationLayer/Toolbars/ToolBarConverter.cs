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
        _toolbarViewModelConverter = new ToolbarViewModelConverter();
    }

    private readonly BooleanToVisibilityConverter _booleanToVisibilityConverter;
    private readonly ToolbarViewModelConverter _toolbarViewModelConverter;

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

        return _toolbarViewModelConverter.ConvertToToolBar(converted, cultureInfo);
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

        return JsonConvert.SerializeObject(_toolbarViewModelConverter.ConvertToToolBarViewModel(toolBar, cultureInfo));
    }
}
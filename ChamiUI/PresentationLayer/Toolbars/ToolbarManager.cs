using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.PresentationLayer.Toolbars;

public class ToolbarManager
{
    public ToolbarManager(CultureInfo cultureInfo)
    {
        ToolBars = new List<ToolBar>();
        _toolbarViewModelConverter = new ToolbarViewModelConverter();
        _cultureInfo = cultureInfo;

        if (_cultureInfo == null)
        {
            _cultureInfo = CultureInfo.CurrentUICulture;
        }
    }
    public List<ToolBar> ToolBars { get; }
    private readonly ToolbarViewModelConverter _toolbarViewModelConverter;
    private readonly CultureInfo _cultureInfo;

    public void AddToolBar(ToolBarViewModel toolBarViewModel)
    {
        ToolBars.Add(_toolbarViewModelConverter.ConvertToToolBar(toolBarViewModel, _cultureInfo));
    }
}
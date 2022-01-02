using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ChamiUI.PresentationLayer.Utils;
using ChamiUI.PresentationLayer.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ChamiUI.PresentationLayer.Toolbars
{
    public class ToolbarLocator
    {
        public ToolbarLocator(IEnumerable<ToolbarInfoViewModel> toolbarInfos)
        {
            _dictionary = new ResourceDictionary();
            _toolbarInfoViewModels = toolbarInfos;
            _toolbarInfoViewModelCache = new Dictionary<string, ToolbarInfoViewModel>();
            foreach (var toolbarInfo in _toolbarInfoViewModels)
            {
                _toolbarInfoViewModelCache[toolbarInfo.ToolbarName] = toolbarInfo;
            }
        }
        
        public void AddSource(ResourceDictionary dictionary)
        {
            foreach (string key in dictionary.Keys)
            {
                if (dictionary[key] is ToolBar toolBar)
                {
                    dictionary.TryGetResource(key, out var existingToolbarObject);
                    if (existingToolbarObject != null)
                    {
                        _dictionary.Remove(key);
                    }
                    
                    _dictionary.Add(key, dictionary[key]);                    
                }
            }
        }
        
        private readonly IEnumerable<ToolbarInfoViewModel> _toolbarInfoViewModels;
        private readonly ResourceDictionary _dictionary;
        private readonly Dictionary<string, ToolbarInfoViewModel> _toolbarInfoViewModelCache;

        public IEnumerable<ToolBar> GetToolbars()
        {
            List<ToolBar> detectedToolbars = new List<ToolBar>();
            foreach (var toolbarInfo in _toolbarInfoViewModels)
            {
                var toolbarInternalName = toolbarInfo.ToolbarName;
                
                detectedToolbars.Add(GetToolbarByName(toolbarInternalName));
            }

            return detectedToolbars;
        }

        public ToolBar GetToolbarByName(string toolbarName)
        {
            if (toolbarName == null)
            {
                throw new ArgumentNullException();
            }

            _dictionary.TryGetResource(toolbarName, out var resource);
            if (resource is ToolBar toolBar)
            {
                var toolbarInfo = GetToolbarInfoByName(toolbarName);

                if (toolbarInfo != null)
                {
                    toolBar.Band = toolbarInfo.BandOccupied;
                    toolBar.BandIndex = toolbarInfo.OrdinalPositionInBand;
                    toolbarInfo.ToolBar = toolBar;
                }

                return toolBar;
            }

            return null;
        }

        private ToolbarInfoViewModel GetToolbarInfoByName(string name)
        {
            return _toolbarInfoViewModelCache[name];
        }
    }
}
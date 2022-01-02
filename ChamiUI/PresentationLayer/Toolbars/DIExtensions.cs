using System.Collections.Generic;
using ChamiUI.PresentationLayer.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ChamiUI.PresentationLayer.Toolbars
{
    public static class DIExtensions
    {
        public static IServiceCollection AddToolbarLocator(this IServiceCollection serviceCollection,
            IEnumerable<ToolbarInfoViewModel> toolbarInfo)
        {
            serviceCollection.AddSingleton<ToolbarLocator>(new ToolbarLocator(toolbarInfo));
            return serviceCollection;
        }
    }
}
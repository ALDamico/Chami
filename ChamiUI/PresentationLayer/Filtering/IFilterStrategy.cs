using System;
using System.Windows.Data;

namespace ChamiUI.PresentationLayer.Filtering
{
    public interface IFilterStrategy
    {
        string Name { get;  }
        string SearchedText { get; set; }
        void OnFilter(object sender, FilterEventArgs args);
        StringComparison Comparison { get; set; }
    }
}
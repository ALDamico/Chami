using System;
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.BusinessLayer.Validators;
using ChamiUI.PresentationLayer.Events;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public abstract class NewEnvironmentViewModelBase: ViewModelBase
    {
        public NewEnvironmentViewModelBase()
        {
            Validator = new EnvironmentViewModelValidator();
            DataAdapter = new EnvironmentDataAdapter(App.GetConnectionString());
        }
        protected EnvironmentViewModelValidator Validator { get; }
        protected EnvironmentDataAdapter DataAdapter { get; }
        
        public abstract bool IsSaveButtonEnabled { get;  }
    }
}
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Windows;
using AsyncAwaitBestPractices.MVVM;
using Chami.Db.Entities;
using ChamiUI.BusinessLayer.Services;
using ChamiUI.Localization;

namespace ChamiUI.PresentationLayer.ViewModels
{
    public sealed class NewTemplateWindowViewModel : NewEnvironmentViewModelBase
    {
        public NewTemplateWindowViewModel(NewEnvironmentService newEnvironmentService) : base(newEnvironmentService)
        {
            Environment = new EnvironmentViewModel() {EnvironmentType = EnvironmentType.TemplateEnvironment};
            SaveCommand = new AsyncCommand<Window>(ExecuteSave, CanExecuteSave);
        }

        protected override async Task ExecuteSave(Window param)
        {
            var closeWindow = true;
            try
            {
                await _newEnvironmentService.SaveEnvironment(Environment);
                
            }
            catch (SQLiteException ex)
            {
                closeWindow = false;
                string message;
                string caption;

                if (ex.ErrorCode == (int) SQLiteErrorCode.Constraint_Unique)
                {
                    message = string.Format(ChamiUIStrings.SaveEnvironmentErrorMessage, Environment.Name);
                    caption = ChamiUIStrings.SaveEnvironmentErrorCaption;
                }
                else
                {
                    message = string.Format(ChamiUIStrings.SaveEnvironmentUnknownErrorMessage, ex.Message,
                        ex.StackTrace);
                    caption = ChamiUIStrings.SaveEnvironmentUnknownErrorCaption;
                }

                ShowMessageBox(null, message, caption, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            if (closeWindow)
            {
                await CloseCommand.ExecuteAsync(param);
            }
        }

        public string TemplateName
        {
            get => Environment.Name;
            set
            {
                Environment.Name = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Environment));
                OnPropertyChanged(nameof(IsSaveButtonEnabled));
                SaveCommand.RaiseCanExecuteChanged();
            }
        }


        public override bool IsSaveButtonEnabled
        {
            get
            {
                bool result = !string.IsNullOrWhiteSpace(TemplateName);

                var validationResult = Validator.Validate(Environment);
                result &= validationResult.IsValid;

                return result;
            }
        }
    }
}
using ChamiUI.BusinessLayer.Adapters;
using ChamiUI.PresentationLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChamiUI.BusinessLayer.Factories
{
    public static class SettingsViewModelFactory
    {
        public static SettingsViewModel GetSettings(SettingsDataAdapter dataAdapter, WatchedApplicationDataAdapter watchedApplicationDataAdapter)
        {
            var settings = dataAdapter.GetSettings();
            var watchedApplications = watchedApplicationDataAdapter.GetActiveWatchedApplications();
            settings.WatchedApplicationSettings.WatchedApplications = new ObservableCollection<WatchedApplicationViewModel>(watchedApplications);
            return settings;
        }
    }
}

using System.Collections.Generic;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Mementos
{
    public class EnvironmentCaretaker
    {
        public EnvironmentCaretaker()
        {
            TemplateDictionary = new Dictionary<string, EnvironmentMemento>();
            _originator = new EnvironmentOriginator();
        }
        public Dictionary<string, EnvironmentMemento> TemplateDictionary { get; }
        private EnvironmentOriginator _originator;

        public void SaveState(string templateName, EnvironmentViewModel state)
        {
            _originator.State = state;
            TemplateDictionary[templateName] = _originator.SaveMemento();
        }

        public EnvironmentViewModel RestoreState(string templateName)
        {
            TemplateDictionary.TryGetValue(templateName, out var val);
            if (val != null)
            {
                _originator.State = val.Environment;    
            }

            
            return _originator.State;
        }
    }
}
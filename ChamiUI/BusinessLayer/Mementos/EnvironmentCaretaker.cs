using System;
using System.Collections.Generic;
using ChamiUI.PresentationLayer.ViewModels;

namespace ChamiUI.BusinessLayer.Mementos
{
    public class EnvironmentCaretaker
    {
        public EnvironmentCaretaker()
        {
            _states = new Dictionary<string, EnvironmentMemento>();
            _originator = new EnvironmentOriginator();
        }

        private readonly Dictionary<string, EnvironmentMemento> _states;
        private readonly EnvironmentOriginator _originator;

        public EnvironmentViewModel ResumeState(string stateName)
        {
            if (stateName == null)
            {
                throw new InvalidOperationException();
            }

            var stateFound = _states.TryGetValue(stateName, out var memento);
            
            if (stateFound)
            {
                _originator.RestoreState(memento);
                return memento.State;
            }

            return null;
        }

        public void SaveState(string stateName, EnvironmentViewModel model)
        {
            if (string.IsNullOrWhiteSpace(stateName))
            {
                stateName = "None";
            }

            _originator.SetState(model);
            _states[stateName] = _originator.CreateMemento() as EnvironmentMemento;
            
            
        }
    }
}
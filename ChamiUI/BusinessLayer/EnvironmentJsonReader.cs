using ChamiUI.PresentationLayer.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ChamiUI.BusinessLayer.Converters;

namespace ChamiUI.BusinessLayer
{
    public class EnvironmentJsonReader : IEnvironmentReader<EnvironmentViewModel>
    {
        private string _inputFile;
        public EnvironmentJsonReader(string inputFile)
        {
            _inputFile = inputFile;
            _stream = File.Open(inputFile, FileMode.Open);
        }

        private Stream _stream;

        public EnvironmentViewModel Process()
        {
            if (_stream == null)
            {
                throw new NullReferenceException("The input stream was null!");
            }

            var streamReader = new StreamReader(_stream).ReadToEnd();

            var environment =
                JsonConvert.DeserializeObject<EnvironmentViewModel>(streamReader,
                    new EnvironmentViewModelJsonConverter());
            return environment;
        }

        public ICollection<EnvironmentViewModel> ProcessMultiple()
        {
            if (_stream == null)
            {
                throw new NullReferenceException("The input stream was null!");
            }

            var streamReader = new StreamReader(_stream).ReadToEnd();

            var environments =
                JsonConvert.DeserializeObject<IEnumerable<EnvironmentViewModel>>(streamReader,
                    new EnvironmentViewModelJsonConverter());
            return environments as List<EnvironmentViewModel>;
        }
    }
}
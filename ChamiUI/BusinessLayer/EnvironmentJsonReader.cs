using ChamiUI.PresentationLayer.ViewModels;
using Newtonsoft.Json;
using System;
using System.IO;

namespace ChamiUI.BusinessLayer
{
    public class EnvironmentJsonReader
    {
        public EnvironmentJsonReader(Stream stream)
        {
            _stream = stream;
        }
        private Stream _stream;

        public EnvironmentViewModel Process()
        {
            if (_stream == null)
            {
                throw new NullReferenceException("The input stream was null!");
            }

            var streamReader = new StreamReader(_stream).ReadToEnd();

            var environment = JsonConvert.DeserializeObject<EnvironmentViewModel>(streamReader);
            return environment;
        }
    }
}